
//龙跃
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AssetManagement
{

    public class HttpRequest: IEnumerator,IDisposable
    {
        public bool isDown;
        public bool succeed;
        public string webUrl;
        public byte[] data;
        public float progress;
        public bool isDispose;
        public Action<HttpRequest> FileCompleted;//下载完成回调函数,回调时自动解绑
        public object Current => null;

        public bool MoveNext(){ return !isDown || !isDispose; }

        public void Reset() {}

        public void Dispose()
        {
            //下载成功或失败，都调用
            FileCompleted = null;
            //data = null;
            isDispose = true;
        }
    }

    /// <summary>
    /// 负责下载 files.txt记录的文件 一定要使用这个下载 
    /// </summary>
    public class AssetHttpRequestManager : SingleTemplate<AssetHttpRequestManager>
    {
     
        //private Dictionary<string, AssetHttpRequest> items_dic = new Dictionary<string, AssetHttpRequest>(128);
        private List<AssetHttpRequest> items_ing = new List<AssetHttpRequest>(3);
        private List<AssetHttpRequest> items_wait = new List<AssetHttpRequest>(128);

        private bool invaild_sort;
        private bool pause = false;
        public static  void ProcesswebUrls(string fileName,string version,out List<string> urls,out string savePath)
        {
            urls = new List<string>(AssetDefine.RemoteDownloadUrls.Length);
           
            for (int i = 0;i< AssetDefine.RemoteDownloadUrls.Length;i++)
            {
                string urlstr = fileName;
                if(!string.IsNullOrEmpty(version))
                {
                    //大文件，不能使用 ?v=md5 来校验缓存，避免在cdn出现回源问题
                    //只有，version,file 这些关键信息文件需要校验
                    urlstr = fileName + string.Format("?v={0}", version);
                }
                urls.Add(Path.Combine(AssetDefine.RemoteDownloadUrls[i], urlstr));
            }

            //下载后保存的路径
            //固定在扩展卡里
            savePath = Path.Combine(AssetDefine.ExternalSDCardsPath, fileName);
           
        }

        public HttpRequest TryGet(string filePath)
        {
            foreach (var item in items_ing)
            {
                //下载正在进行
                if (item.filePath == filePath) return item.request;
            }

            foreach (var item in items_wait)
            {
                //下载正在等待
                if (item.filePath == filePath) return item.request;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        ///  <param name="filePath">资源包相对路径比如：001/scene/scene_01.asset</param>
        /// <param name="rtype">使用的下载类型</param>
        /// <param name="priority">优先级</param>
        /// <param name="md5"></param>
        /// <param name="writeDsk">是否写入磁盘</param>
        /// <param name="crc"></param>
        /// <returns></returns>
        protected HttpRequest GetRequest(string filePath, int rtype ,int priority,string md5,bool writeDsk = true,uint crc = 0)
        {
            HttpRequest reset = this.TryGet(filePath);

            if (reset != null) return reset;//正在下载

            List<string> urls = null;
            string savePath = null;

            ProcesswebUrls(filePath,null ,out urls, out savePath);

            //开启下载文件到磁盘
            AssetHttpRequest rest = null;
            if (rtype == 1)
                rest = new AssetFileHttpRequest(urls, savePath);
            else if (rtype == 2)
                rest = new AssetByteHttpRequest(urls, savePath, writeDsk);
            else if (rtype == 3)
                rest = new AssetBundHttpRequest(urls, savePath, crc);

            rest.filePath = filePath;
            rest.priority = priority;
            rest.md5_verify = md5;
            items_wait.Add(rest);

            invaild_sort = true;


            return rest.request;
        }

        /// <summary>
        /// 从web端下载文件到磁盘
        /// 内存消耗最小
        /// </summary>
        /// <param name="filePath">资源包相对路径比如：001/scene/scene_01.asset</param>
        /// <param name="priority">优先级，数字越大下载越优先</param>
        /// <param name="md5">记录文件上的MD5码，用来确认下载的文件是否正确，注意 MD5码校验吃消耗性能</param>
        /// <returns></returns>
        public HttpRequest RequestFile(string filePath,int priority = 3,string md5 = "")
        {

            return GetRequest(filePath,1,priority, md5);
        }

        /// <summary>
        /// 从web端下载文件到byte[],同时写入磁盘
        /// 吃内存
        /// </summary>
        /// <param name="filePath">资源包相对路径比如：001/scene/scene_01.asset</param>
        /// <param name="writeDsk">是否自动写入到磁盘中，默认开启</param>
        /// <param name="outFun">回调</param>
        /// <param name="priority">优先级，数字越大下载越优先</param>
        /// <param name="md5">记录文件上的MD5码，用来确认下载的文件是否正确，注意 MD5码校验吃消耗性能</param>
        /// <returns></returns>

        public HttpRequest RequestBytes(string filePath, bool writeDsk,Action<HttpRequest> outFun = null, int priority = 1, string md5 = "")
        {
            HttpRequest quest = GetRequest(filePath, 2, priority, md5, writeDsk);
            if(outFun != null)
                quest.FileCompleted += outFun;


            return quest;
        }

        /// <summary>
        /// 一般不用,正常的AssetBundle下载请使用 RequestFile()
        /// </summary>
        /// <returns></returns>
        public HttpRequest RequestAssetBundle(string filePath, uint crc, Action<HttpRequest> outFun = null, int priority = 2, string md5 = "")
        {
            HttpRequest quest = GetRequest(filePath, 3, priority, md5, true, crc);

            if (outFun != null)
                quest.FileCompleted += outFun;

            return quest;
        }

        //暂停
        public  void OnPause()
        {
            pause = true;
        }

        //开始下载
        public  void OnStart()
        {
            pause = false;
        }


        private static int SortCompare(AssetHttpRequest AF1, AssetHttpRequest AF2)
        {

            //if (AF1.priority > AF2.priority) return -1;
            //else if (AF1.priority < AF2.priority) return 1;
            //return 0;

            return -AF1.priority.CompareTo(AF2.priority);
        }
        public void Update()
        {
            if (pause) return;

            if (items_ing.Count == 0 && items_wait.Count == 0) return;


            if (invaild_sort)//对wait进行排序
            {
                items_wait.Sort(SortCompare);//降序 
                invaild_sort = false;
            }


            //补充下载数量
            while (items_ing.Count < DownloadSetting.Instance.MaxWebRqeustNum && items_wait.Count > 0)
            {
                items_ing.Add(items_wait[0]);

                items_wait.RemoveAt(0);
            }


            for(int i = 0;i< items_ing.Count;i++)
            {
                AssetHttpRequest item = items_ing[i];

                bool _remove = item.Update();

                if (_remove)
                {
                    item.Dispose();
                    items_ing.RemoveAt(i);
                    //i--;
                    break;//一帧处理一个下载成功
                }
              
            }

        }

    }
}
