//龙跃
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AssetManagement
{
    public enum AssetType
    {
        Scene = 1,
        Other = 2 
    }

    public class AssetLoaderParcel:IAssetLoader
    {
      
        protected string _assetName;
        public override string assetName { get { return _assetName; } }
        protected string _abpath;
        protected System.Type _generateType;
        protected AsyncOperation _asyncOperation;
        protected AssetType _assetType;
        protected string _message;

        protected bool _isDownloading = false;
        protected bool _isLoading = false;
        protected bool _isFailed = false;
        protected bool _preloadDone = false;
        public bool isActive = false;
    

        public bool isFailed 
        {
            get { return _isFailed; }
            set
            {
                _isFailed = value;
                if (value)
                {
                    _isLoading = false;
                    _isDownloading = false;
                }
            }
        }
        /// <summary>
        /// 开启预加载模式
        /// </summary>
        public bool IsPreload { get; set; }
      
        public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        public Action<AssetLoaderParcel> onComplete;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
            }
        }


        public AssetLoaderParcel(string assetName,System.Type type)
        {
            _assetName = assetName;
            _abpath = AssetProgram.Instance.loadOptions.GetABPathByAssetName(assetName);
            _generateType = type;
            _assetType = AssetType.Other;
            isFailed = false;
        }

        public override void Update()
        {
            if (isFailed || !isActive) return;

            if (_asyncOperation != null)
            {

                //当 _asyncOperation != null 表明，正在从AssetBundle包里加载资源，下面加载AssetBundle的逻辑已经执行完毕
                return;
            }
            string errorStr;
            BundleInfo abinfo = null;
            AssetbundleCache.Achieve(_abpath,out abinfo,out errorStr);//包括依赖加载完成

            if(!string.IsNullOrEmpty(errorStr))
            {
                //加载 AssetBundle 失败
                Message = errorStr;
                isFailed = true;
         
                return;
            }

            if (_isDownloading)
            {
                //等待加载 AssetBundle 包.......
                if (abinfo == null) return;

                _isDownloading = false;//assetBundle 包准备好了

                if (IsPreload)
                {
                    _preloadDone = true;
                    return;//预加载模式，只加载 AB资源
                }
            }
 
            if (abinfo != null)
            {

                if (abinfo.ab.isStreamedSceneAssetBundle)
                {
                    _isLoading = true;
                    _assetType = AssetType.Scene;
                    //加载
                    //包里只有一个场景
                    string scenePath = abinfo.ab.GetAllScenePaths()[0];
                    GameDebug.LogGreen(string.Format("开始加载场景！！！{0}", scenePath));
                    _asyncOperation = SceneManager.LoadSceneAsync(scenePath, loadSceneMode);

                    //场景 AssetBundle 引用
                    AssetbundleCache.AddRefrence(_abpath);
               
                }
                else 
                {
                    _assetType = AssetType.Other;
                    if (abinfo.ab.Contains(_assetName))
                    {
                        _isLoading = true;
                        //加载资源
                        try {
                             _asyncOperation = abinfo.ab.LoadAssetAsync(_assetName, _generateType);
                        }
                        catch(Exception e)
                        {
                            Message = e.Message;
                            isFailed = true;
                            GameDebug.LogError(Message);
                        }
                    }
                    else
                    {
                        isFailed = true;
                        Message = string.Format("【AssetLoaderParcel】AssetBundle 包中不存在 assetName。 assetBundle Name={0} type={1}", abinfo.path, _assetName);
                        GameDebug.LogWarning(Message);
                    }
                     
                }

            }
            else
            {

                _isDownloading = true;

                //开启从磁盘里面加载AssetBundle包
                AssetBundleManager.Instance.LoadAssetBundle(_abpath);
            }

        }

        #region 下载状态返回
        /// <summary>
        /// 下载进度
        /// </summary>
        /// <returns></returns>
        public override float Progress()
        {
            float factorization = 0.5f;
            float progress = AssetBundleManager.Instance.GetAssetBunldsProgress(_abpath)* factorization;

            if (_asyncOperation != null)
                progress += _asyncOperation.progress * factorization;

            return progress;
        }

        /// <summary>
        /// 包含2个结果
        /// 1.加载成功
        /// 2.加载失败
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            if (_isFailed || _preloadDone) return true;

            if (_asyncOperation != null)
            {
                //加载资源完成
                if(_asyncOperation.isDone || _assetType == AssetType.Scene)
                {
                    _isLoading = false;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 下载成功
        /// </summary>
        /// <returns></returns>
        public override bool IsSucceed() { return IsDone() && !_isFailed; }

        #endregion

        #region 获得对象 方法
        /// <summary>
        /// 获得源资源，引用计数+1
        /// </summary>
        /// <returns></returns>
        public virtual T GetRawObject<T>() where T : Object
        {
            return AssetRawobjCache.GetRawAsset(this._abpath, this._assetName, _asyncOperation as AssetBundleRequest) as T;
        }


        /// <summary>
        /// 实例化控制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual T Instantiate<T>(Transform parent = null) where T : Object
        {

            return AssetRawobjCache.Instantiate(this._abpath, this._assetName, parent, _asyncOperation as AssetBundleRequest) as T;
        }

        public virtual GameObject Instantiate(Transform parent = null)
        {
            GameObject obj = Instantiate<GameObject>(parent);
            if (obj != null) obj.transform.ResetTRS();

            return obj;
        }
        #endregion

        public override void Dispose()
        {
            _asyncOperation = null;
            _isDownloading = false;

        }

        #region 状态方法
        public bool IsDownloading {   get {  return _isDownloading; } }
        public bool IsLoading { get { return false; } }
        #endregion

    }
}