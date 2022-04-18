//龙跃
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace AssetManagement
{
    /// <summary>
    /// 下载层面
    /// </summary>
    abstract public class AssetHttpRequest : IDisposable
    {
        public enum State
        {
            None,//初始
            DataNullError,//错误
            MD5Error,//MD5错误
            Downloading,//下载中
            LoadFinish,//下载完成
            Completed,//等待释放
            Dispose
        }

        public State state { get { return _state; } }
        public HttpRequest request { get { return _reqest; } }

        //UnityWebRequest 不支持重用
        protected UnityWebRequest _webRequest;
        protected HttpRequest _reqest = null;

        protected List<string> _webUrls;//下载路径队列
        public List<string> WebUrls { set { _webUrls = value; } get { return _webUrls; } }
        protected string _savePath;//保存路径
        public string SavePath { set { _savePath = value; } get { return _savePath; } }
        protected State _state = State.None;

        public string md5_verify;//记录文件上的md5码
        public int priority;//下载优先级
        public string filePath;//文件相对路径：比如001/scene/1.asset

        public AssetHttpRequest()
        {
            _reqest = new HttpRequest();
        }
        public AssetHttpRequest(List<string> weburls,string savePath)
        {
            
            _webUrls = weburls;
            _savePath = savePath;
            _reqest = new HttpRequest();
        }


        protected virtual bool NeedSendRequest
        {
            get
            {
                if (_webUrls.Count == 0) return false;
                //只有初始状态，错误状态 可以开启下载
                return _state == State.None || IsError;
            }
        }

        public virtual bool Update()
        {
            bool _remove = false;

            //if (HadNextChange)
            //{
            //    if (IsError)
            //    {
            //        TellWhat();

            //        if (NeedSendRequest)
            //        {
            //            TrySendRequest();//重新下载
            //            GameDebug.LogGreen(string.Format("[{0}]{1} {2}", "AssetHttpRequest", "尝试更换下载 URL,再次下载！", _webRequest.url));
            //        }
            //        else
            //            _remove = true;

            //    }
            //    else
            //    {
            //        if (NeedSendRequest) TrySendRequest();//开启下载
            //    }

            //}
            //else
            //{
            //    if (IsError)
            //    {
            //        TellWhat();
            //        _remove = true;
            //    }
            //}

            if (IsError)
            {
                TellWhat();
                if (HadNextChange && NeedSendRequest)
                {
                    TrySendRequest();//重新下载
                    GameDebug.LogGreen(string.Format("[{0}]{1} {2}", "AssetHttpRequest", "尝试更换下载 URL,再次下载！", _webRequest.url));
                }
                else
                    _remove = true;
            }
            else if (NeedSendRequest) TrySendRequest();//开启下载


            if (_webRequest != null)
                _reqest.progress = _webRequest.downloadProgress;

            if (!_remove && IsSuccess)
            {
                //下载成功完成
                if (TryDealData()) _remove = true;//数据校验和处理成功
            }

            return _remove;
        }

        /// <summary>
        /// 开始
        /// </summary>
        protected virtual HttpRequest TrySendRequest()
        {

            if (_webRequest != null)//重复用不了
            {
                _webRequest.Dispose();
                _webRequest = null;
            }

            //删除本地旧文件
            if (File.Exists(this._savePath)) File.Delete(_savePath);


            _reqest.webUrl = _webUrls[0];
            _webUrls.RemoveAt(0);
            EstablishRequest(_reqest.webUrl);
           
            _state = State.Downloading;

            _webRequest.SendWebRequest();

            return _reqest;
        }

        /// <summary>
        /// 建立http请求
        /// </summary>
        protected virtual void EstablishRequest(string weburl)
        {
            throw new Exception("need override");
        }


        //管理器调用下载完成
        protected virtual bool TryDealData()
        {
            _state = State.LoadFinish;

            if (OnVerifyData())
            {
                OnDealCompleted();

                SafeCallOut();

                return true;
            }

            return false;

        }


        protected void SafeCallOut()
        {
            //通知加载层
            if(_reqest.FileCompleted != null)
            {
                try {
                    _reqest.FileCompleted.Invoke(_reqest);
                }
                catch(Exception e){
                    GameDebug.LogRed(string.Format("[{0}] 回调报错 !\n{1}", "AssetHttpRequest:SafeCallOut（）", e.Message));
                }

                _reqest.FileCompleted = null;
            }
         
        }

        /// <summary>
        /// 确认数据
        /// </summary>
        protected virtual bool OnVerifyData()
        {
           
            return true;
        }

        /// <summary>
        /// 下载成功,并且文件校验，处理都成功了
        /// </summary>
        protected virtual void OnDealCompleted()
        {
            //通知加载层
            _reqest.succeed = true;
            _reqest.isDown = true;
            _state = State.Completed;
        }

        /// <summary>
        /// 是否有在次下载的机会
        /// </summary>
        protected bool HadNextChange
        {

            get
            {
                if (_state == State.None) return true;

                return _webUrls.Count > 0 ;
            }
        }

        /// <summary>
        /// 下载是否完成
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                
                if (_webRequest != null && _state == State.Downloading)
                {
#if UNITY_2020
                    return _webRequest.result == UnityWebRequest.Result.Success;
#else
                    return _webRequest.isDone && _webRequest.downloadHandler.isDone   
#endif
                }

                return false;
            }
        }


        public bool IsError
        {
            get
            {
                if (_webRequest == null) return false;


                if (_state == State.MD5Error || _state == State.DataNullError) return true;
#if UNITY_2020
                return _webRequest.result == UnityWebRequest.Result.ConnectionError
                        || _webRequest.result == UnityWebRequest.Result.ProtocolError
                        || _webRequest.result == UnityWebRequest.Result.DataProcessingError;
#else
               return _webRequest.isNetworkError || _webRequest.isHttpError;
#endif

            }
        }

        public virtual bool TellWhat(string msg = null)
        {
            if(IsError)
            {
                if(_state == State.MD5Error)
                    GameDebug.LogRed(string.Format("[{0}] dowded data is null !\n{1}", "AssetHttpRequest", _webRequest.url));
                else if(_state == State.DataNullError)
                    GameDebug.LogRed(string.Format("{0}]dowded data md5 error !\n{1}", "AssetHttpRequest", _webRequest.url));

                if(_webRequest.result == UnityWebRequest.Result.ConnectionError)
                    GameDebug.LogRed(string.Format("[{0}]Failed to communicate with the server {1}", "AssetHttpRequest", _webRequest.url));
                else if (_webRequest.result == UnityWebRequest.Result.ProtocolError)
                    GameDebug.LogRed(string.Format("[{0}]The server returned an error response {1}", "AssetHttpRequest", _webRequest.url));
                else if (_webRequest.result == UnityWebRequest.Result.DataProcessingError)
                    GameDebug.LogRed(string.Format("[{0}]The data was corrupted or not in the correct format {1}", "AssetHttpRequest", _webRequest.url));

                if(msg != null)
                {
                    GameDebug.Log(string.Format("[{0}]{1} {2}", "AssetHttpRequest", msg, _webRequest.url));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 此方法可以在任何时候调用。如果 UnityWebRequest 尚未完成，UnityWebRequest 将尽快停止上传或下载数据
        /// 中止的 UnityWebRequest 被视为发生系统错误。isNetworkError 或 isHttpError 属性都会返回 true，
        /// error 属性的状态将为"用户中止"
        /// </summary>
        public virtual void Abort()
        {
            _webRequest.Abort();
        }


        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            if (_state == State.Dispose) return;

            _state = State.Dispose;

            _reqest.Dispose();
            _reqest = null;

            _webRequest.Dispose();
            _webRequest = null;
        }

    }
}
