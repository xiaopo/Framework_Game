//龙跃
using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 此磁盘里加载 AB 资源
/// </summary>
namespace AssetManagement
{
    public class AssetBundleLoader
    {
        public enum LoaderType:int
        {
            Init = 1,      //默认
            LocalSave = 2, //扩展卡加载
            Buidling = 3,  //首包加载
            WebRequest = 4,  //下载
            Loser = 5,//彻底失败
            WebSucceed = 6,//下载成功
            LoadSucced = 7//彻底加载成功
        }

        public bool IsDone { get { return _loadType == LoaderType.LoadSucced; } }

        public bool IsFailed{ get { return _loadType == LoaderType.Loser; }}
  


        protected AssetBundleCreateRequest _abcreateRequest;
        protected HttpRequest _webCreateRequest;
        protected bool _isDownloading;
        protected bool _isLoading;
        protected LoaderType _loadType;

        public string abPath;

        protected string sdcard_path;
        protected uint crc;
        protected string streaming_path;

        protected string _message;

        protected float _factorization = 1;
        public string Message 
        { 
            get { return _message; } 
            set 
            {
                _message =  string.Format("{0}\n{1}", value, abPath);
            }
        }
        public AssetBundleLoader(string abPath)
        {
            this.abPath = abPath;

            _loadType = LoaderType.Init;

            sdcard_path = Path.Combine(AssetDefine.ExternalSDCardsPath, abPath);
            streaming_path = Path.Combine(AssetDefine.BuildinAssetPath, abPath);
            crc = AssetProgram.Instance.loadOptions.GetABCRCByABName(abPath);
            _factorization = 1.0f;
        }

        public void Update()
        {
            if (IsDone || IsFailed) return;

            if (CheckLoadingAsync()) return;

            if (CheckWebrequestAsync()) return;
           
           
            //分别从  扩展卡目标  安装目标  网络 获取资源
          
            if (_loadType == LoaderType.WebSucceed)
            {
                //下载成功后开启加载

                _isLoading = true;
                _abcreateRequest = LoadFromFileAsync(sdcard_path, crc);
            }
            else
            {
                //加载流程
                ExcutePipeline();
            }
          
        }

        protected AssetBundleCreateRequest LoadFromFileAsync(string path,uint crc)
        {
            try
            { 
                return AssetBundle.LoadFromFileAsync(sdcard_path, crc);
            }
            catch(Exception e)
            {
                 Message = e.Message;
                _loadType = LoaderType.Loser;
                GameDebug.LogError(Message);
                return null;
            }
        }

        public void ExcutePipeline()
        {
           
            string disk_path = null;

            if (File.Exists(sdcard_path) && _loadType < LoaderType.LocalSave)
            {
                //扩展卡目录
                disk_path = sdcard_path;
                _loadType = LoaderType.LocalSave;
            }
            else if (File.Exists(streaming_path) && _loadType < LoaderType.Buidling)
            {

                //安装目录
                disk_path = streaming_path;
                _loadType = LoaderType.Buidling;
            }
            else
                _loadType = LoaderType.WebRequest;//去下载


            if (_loadType == LoaderType.LocalSave || _loadType == LoaderType.Buidling)
            {
                _isLoading = true;
                _factorization = 1.0f;
                //如果出现 crc 码不对。会发起一次下载
                _abcreateRequest = LoadFromFileAsync(disk_path, crc);
            }
            else if(_loadType == LoaderType.WebRequest)
            {
                _isDownloading = true;
                _factorization = 0.5f;
                //string md5 = UpdateManager.Instance.TryGetFileMD5(abPath);//不用校验
                //需要去网络上下载,把资源下载到本地
                _webCreateRequest = AssetHttpRequestManager.Instance.RequestFile(abPath, 3);

            }
        }


        public bool CheckLoadingAsync()
        {

            if (_abcreateRequest == null) return false;

            //正在加载
            if (!_abcreateRequest.isDone) return true;

            
            if (_abcreateRequest.assetBundle == null)
            {
                //加载异常，进行纠正
                if (_loadType < LoaderType.WebRequest)
                {
                  
                    _loadType = _loadType + 1;
                    _abcreateRequest = null;
                    _isLoading = false;

                    Message = "AssetBundleCreateRequest.assetBundle == null ！即将执行矫正加载";
                    GameDebug.LogRed(Message);
                    return true;//下一帧 重启加载
                }
                else
                {
                    _loadType = LoaderType.Loser;
                    Message = "彻底加载失败！ AssetBundleCreateRequest.assetBundle == null ！";
                    GameDebug.LogRed(Message);
                    return true;
                }

            }
            else
            {
                //加载成功
                _loadType = LoaderType.LoadSucced;
                return true;
            }
           
        }


        public bool CheckWebrequestAsync()
        {

            if (_webCreateRequest == null) return false;
            

            if (_webCreateRequest.isDown)
            {
                _isDownloading = false;
            

                if (!_webCreateRequest.succeed)
                {
                    _loadType = LoaderType.Loser;
                    _message = string.Format("webRequest下载AssetBundle失败！{0}", _webCreateRequest.webUrl);
                    //HttpRequest 被 AssetHttpRequestManager 自动释放
                    //这里一定要置空
                    _webCreateRequest = null;
                    GameDebug.LogRed(Message);
                    return true; //下载失败
                }
                else
                {
                    //下载成功
                    _loadType = LoaderType.WebSucceed;
                }


            }
            else
                return true;//等待web 下载完成
            

            return false;

        }

        public float Progress()
        {
            if (IsDone || IsFailed) return 1.0f;

            float progress = 0;
            if (_webCreateRequest != null)
                progress = _webCreateRequest.progress * _factorization;

            if (_abcreateRequest != null)
                progress += _abcreateRequest.progress * _factorization;


            return progress;
        }


        public bool IsLoading
        {
            get
            {
                return _isDownloading || _isLoading;
            }
        }


        public bool IsDownloading { get { return _isDownloading; } }

        public AssetBundle AssetBundle
        {
            get
            {
                if (_abcreateRequest != null && _abcreateRequest.isDone)
                    return _abcreateRequest.assetBundle;

                return null;
            }
        }

        public void Dispose()
        {
            _abcreateRequest = null;
            _webCreateRequest = null;
            _isDownloading = false;
            _isLoading = false;
        }

    }
}