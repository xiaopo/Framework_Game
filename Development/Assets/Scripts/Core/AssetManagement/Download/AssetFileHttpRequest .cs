//龙跃
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AssetManagement
{
    /// <summary>
    /// 内存使用量都很低
    /// 它将下载的字节直接写入文件，因此无论下载的文件大小如何，
    /// </summary>
    public class AssetFileHttpRequest : AssetHttpRequest
    {
        public AssetFileHttpRequest(List<string> weburls, string savePath) : base(weburls, savePath)
        {

        }

        public AssetFileHttpRequest():base()
        {

        }

        protected override void EstablishRequest(string weburl)
        {
            _webRequest = new UnityWebRequest(weburl, UnityWebRequest.kHttpVerbGET);

            DownloadHandlerFile hand = new DownloadHandlerFile(_savePath);

            //执行abort后，正在下载的文件会被移除
            hand.removeFileOnAbort = true;

            _webRequest.downloadHandler = hand;
        }


        protected override bool OnVerifyData()
        {
            //注意 访问 _webRequest.downloadHandler.data 会报错
            if (_state != State.LoadFinish) return false;

            if (!string.IsNullOrEmpty(md5_verify))
            {
                //检测 md5 非常消耗性能，注意
                if (LFileUtil.fileMD5(_savePath) != md5_verify)
                {
                    //MD5码校验失败
                    _state = State.MD5Error;

                    //等待管理器重启下载
                    return false;
                }
            }

            return true;
        }

        protected override void OnDealCompleted()
        {
            base.OnDealCompleted();

            //文件已自动保存到磁盘
            _reqest.data = null;

        }
    }
}
