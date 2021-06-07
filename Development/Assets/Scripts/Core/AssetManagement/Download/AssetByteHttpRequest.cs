//龙跃
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace AssetManagement
{
    /// <summary>
    /// web上的文件，以btye 形式下载
    /// </summary>
    public class AssetByteHttpRequest : AssetHttpRequest
    {
        /// <summary>
        /// 下载后写入磁盘
        /// </summary>
        protected bool writeDisk;
        public AssetByteHttpRequest(List<string> weburls, string savePath,bool writeDsk = true) :base(weburls, savePath)
        {
            writeDisk = writeDsk;
        }

        protected override void EstablishRequest(string weburl)
        {
            _webRequest = new UnityWebRequest(weburl);

            DownloadHandlerBuffer hand = new DownloadHandlerBuffer();

            _webRequest.downloadHandler = hand;
        }

        protected override bool OnVerifyData()
        {
            if (_state != State.LoadFinish) return false;

            byte[] data = _webRequest.downloadHandler.data;
            if (data == null)
            {
                _state = State.DataNullError;
              
                //等待管理器重启下载
                return false;
            }

            if (!string.IsNullOrEmpty(md5_verify))
            {
                //检测 md5 非常消耗性能，注意
                if (LFileUtil.md5file(data) != md5_verify)
                {
                    //MD5码校验失败
                    _state = State.MD5Error;
                    
                   
                    //等待管理器重启下载
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 下载成功,并且文件校验，处理都成功了
        /// </summary>
        protected override void OnDealCompleted()
        {
            base.OnDealCompleted();

            byte[] data = _webRequest.downloadHandler.data;

            if(writeDisk)
            {
                string folderName = Path.GetDirectoryName(_savePath);
                if (!File.Exists(folderName)) Directory.CreateDirectory(folderName);

                FileInfo fileInfo = new FileInfo(_savePath);
                FileStream fs = fileInfo.Create();

                //fs.Write(字节数组, 开始位置, 数据长度);
                fs.Write(data, 0, data.Length);

                fs.Flush();     //文件写入存储到硬盘
                fs.Close();     //关闭文件流对象
                fs.Dispose();   //销毁文件对象
            }

            _reqest.data = data;


        }
    }
}
