//龙跃
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AssetManagement
{ 
    public enum LoadState
    {
        Loading,
        LoadDone
    }
    public class AssetBundleManager : SingleTemplate<AssetBundleManager>
    { 
        private Dictionary<string, Action> _doneCallBacks = new Dictionary<string, Action>();
        /// <summary>
        /// 正在加载的对象
        /// </summary>
        private Dictionary<string, AssetBundleLoader> _assetLoading = new Dictionary<string, AssetBundleLoader>(24);

        /// <summary>
        /// 多次加载失败的对象
        /// </summary>
        private Dictionary<string, AssetBundleLoader> _asseLoadloser = new Dictionary<string, AssetBundleLoader>(24);

        public void AddListener(LoadState sate,string abpath,Action baction)
        {
            Action action = null;

            if(sate == LoadState.LoadDone)
            {
                if (_doneCallBacks.TryGetValue(abpath, out action))
                    action += baction;//可能会存在内存泄漏，待确认
                else
                    _doneCallBacks.Add(abpath, action);
            }

        }

        /// <summary>
        /// 加载失败
        /// </summary>
        /// <param name="abPath"></param>
        /// <returns></returns>
        public AssetBundleLoader GetLoser(string abPath)
        {
            AssetBundleLoader loader = null;
            if (_asseLoadloser.TryGetValue(abPath,out loader))
            {
                return loader;
            }

            return null;
        }


        /// <summary>
        /// 只会加载没有载入到内存的AssetBundle
        /// </summary>
        /// <param name="abName"></param>
        public void LoadAssetBundle(string abName)
        {
            if (this.LoadAssetBundleInternal(abName))
            {
                this.LoadDependence(abName);
            }
        }

        private void LoadDependence(string abName)
        {
            
            List<string> dependence = AssetProgram.Instance.loadOptions.GetAllDepenceies(abName);
            if (dependence != null)
            {
                foreach (var depenName in dependence)  //加载依赖
                    this.LoadAssetBundleInternal(depenName);
            }

        }
        private bool LoadAssetBundleInternal(string abName)
        {
            if (string.IsNullOrEmpty(abName)) return false;

            if (AssetbundleCache.TryGet(abName) != null) return false;//已载入内存

            if(_asseLoadloser.ContainsKey(abName))
                _asseLoadloser.Remove(abName); //再次加载时需要移除历史记录

            AssetBundleLoader loader;
            if(_assetLoading.TryGetValue(abName, out loader))
            {
                if (loader.IsLoading) return false;//正在加载，请等待
            }
            else
            {
                //加载
                loader = new AssetBundleLoader(abName);

                _assetLoading.Add(abName, loader);
            }
        
            return true;
        }

        private List<string> tempList = new List<string>(10);
        public void Update()
        {
            if (_assetLoading.Count == 0) return;
      
            foreach(var item in _assetLoading)
            {
                AssetBundleLoader abLoader = item.Value;
                string abPath = abLoader.abPath;
                bool bremove = false;
                if (abLoader.IsFailed)
                {
                    //多次尝试后，最终加载失败
                   
                    _asseLoadloser.Add(abPath, abLoader);
                    bremove = true;
                }
                else if (abLoader.IsDone)
                {
                    //载入完成
                    AssetBundle ab = abLoader.AssetBundle;
                    AssetbundleCache.Add(abPath, ab);
                    bremove = true;

                }
                else
                {
                    abLoader.Update();
                }

                if(bremove)
                {
                    tempList.Add(item.Key);
                    abLoader.Dispose();

                    //回调
                    //失败后会继续回调，但是数据是空
                    Action outAction = null;
                    if (_doneCallBacks.TryGetValue(abPath, out outAction))
                    {
                        outAction.Invoke();
                        _doneCallBacks.Remove(abPath);
                    }
                }
            }

            foreach(var item in tempList) _assetLoading.Remove(item);
     

            tempList.Clear();
        }

        public void Dispose()
        {
            foreach (var item in _assetLoading)
                item.Value.Dispose();

            _doneCallBacks.Clear();
            _asseLoadloser.Clear();
            _asseLoadloser.Clear();


            AssetbundleCache.Dispose();
        }

        #region 获取状态函数
        //包是否在下载中
        public bool IsDownloading(string abName, bool dep = true)
        {
            bool downloading = false;
            AssetBundleLoader abl;
            if (_assetLoading.TryGetValue(abName, out abl))
                downloading = abl.IsDownloading;


            if(!downloading && dep)
            {
                //检查依赖
                List<string> dependence = AssetProgram.Instance.loadOptions.GetAllDepenceies(abName);
                foreach (var depName in dependence)
                {
                    if (_assetLoading.TryGetValue(depName, out abl))
                    {
                       if(abl.IsDownloading)
                        {
                            downloading = true;
                            break;
                        }
                    }
                }
            }

            return downloading;
        }

        public float GetOneProgress(string abName)
        {
            if (AssetbundleCache.Has(abName)) return 1.0f;

            if (_assetLoading.TryGetValue(abName, out var abl))
                return abl.Progress();

            return 0.0f;
        }

        public float GetAssetBunldsProgress(string abName, bool dep = true)
        {
           
            int count = 1;
            float progress = GetOneProgress(abName);

            if (dep)
            {
                //检查依赖
                List<string> dependence = AssetProgram.Instance.loadOptions.GetAllDepenceies(abName);
                foreach (var depName in dependence)
                {
                    progress += GetOneProgress(abName);
                }

                count += dependence.Count;
            }

            return progress / count;
        }

        #endregion
    }
}