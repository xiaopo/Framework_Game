//龙跃
using System.Collections.Generic;
using UnityEngine;

namespace AssetManagement 
{ 
    /// <summary>
    /// 带加载对象缓存功能
    /// 不带对象池功能
    /// </summary>
    public class AssetsGetManger: SingleTemplate<AssetsGetManger>
    {

        private Dictionary<string, AssetLoaderParcel> _assetLoader = new Dictionary<string, AssetLoaderParcel>(100);
        private Dictionary<string, AssetLoaderParcel> _assetLoadCompletes = new Dictionary<string, AssetLoaderParcel>(100);



        /// <summary>
        /// 预加载AssetBundle进入内存，不实例化
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssetLoaderParcel PreloadAsset(string assetName, System.Type type)
        {
            AssetLoaderParcel loader = OnLoadAsset(assetName, type);
            loader.IsPreload = true;

            return loader;
        }
        public AssetLoaderParcel LoadAsset(string assetName, System.Type type)
        {
            AssetLoaderParcel loader = OnLoadAsset(assetName, type);
            loader.IsPreload = false;

            return loader;
        }
        protected AssetLoaderParcel OnLoadAsset(string assetName, System.Type type)
        {
#if UNITY_EDITOR
            string fileName = System.IO.Path.GetFileNameWithoutExtension(assetName);
            if (string.IsNullOrEmpty(fileName))
            {
                GameDebug.LogErrorFormat("【AssetsGetManger】LoadBundleAsset fileName is IsNullOrEmpty assetName={0}", assetName);
                return null;
            }
#endif

            if (string.IsNullOrEmpty(assetName))
            {
                GameDebug.LogErrorFormat("【AssetsGetManger】LoadBundleAsset is IsNullOrEmpty assetName={0}", assetName);
                return null;
            }


            string assetPath;
            bool isEditorLoad = AssetProgram.Instance.loadOptions.IsEditorLoad(assetName);
            if (isEditorLoad)
            {
                //编辑器加载方式要把,weaponIcon.png 转换成 /Asset/GUI/Image/weponIcon.png
                assetPath = AssetProgram.Instance.loadOptions.GetAssetPathAtName(assetName);
                if (string.IsNullOrEmpty(assetPath))
                {
                    //编辑器内资源丢失
                    GameDebug.LogErrorFormat("【AssetsGetManger】编辑器 Asset Name 不存在 assetName={0} type={1}", assetName, type);
                    return null;
                }

            }
            else
            {
                //AssetBundle包模式名字必须是小写
                assetName = assetName.ToLower();

                //AB加载方式
                assetPath = assetName;

                if(!AssetProgram.Instance.loadOptions.ContainsAsset(assetName,2))
                {
                    //包内无资源
                    GameDebug.LogErrorFormat("【AssetsGetManger】AssetBundle内 Asset Name 不存在 assetName={0} type={1}", assetName, type);
                    return null;
                }

            }

            AssetLoaderParcel loader = null;
            if(_assetLoadCompletes.TryGetValue(assetPath,out loader))
            {
                //检测到已经加载过，在下一帧回调结果
                //不能直接回调完成 
                _assetLoadCompletes.Remove(assetPath);
                _assetLoader.Add(assetPath, loader);
            }
            else
            {
                //需要加载
                if (!_assetLoader.TryGetValue(assetPath, out loader))
                {
                    if (isEditorLoad)
                        loader = new AssetLoaderEditor(assetPath, type);//编辑器加载模式
                    else
                        loader = new AssetLoaderParcel(assetPath, type);//assetBundle加载模式

                    _assetLoader.Add(assetPath, loader);
                }
            }
            

            loader.isActive = true; //激活

            return loader;
        }

        private List<string> tempList = new List<string>(10);
        public void Update()
        {
            if (_assetLoader.Count == 0) return;
           

            foreach (var item in _assetLoader)
            {
                AssetLoaderParcel assetLoader = item.Value;
                if (!assetLoader.isActive) continue;//没有激活

                if (assetLoader.IsDone())
                {
                    if (assetLoader.onComplete != null)
                    {
                        assetLoader.onComplete.Invoke(assetLoader);
                        //自动解绑
                        assetLoader.onComplete = null;
                    }

                    GameDebug.Log("AssetsGetManger.Update:: 加载成功: " + assetLoader.assetName);
                    assetLoader.isActive = false;
                    tempList.Add(item.Key);

                    //缓存起来
                    _assetLoadCompletes.Add(item.Key,item.Value);
                }
                else
                    assetLoader.Update();
            }

            foreach (var item in tempList)
                _assetLoader.Remove(item);


            tempList.Clear();
        }

        /// <summary>
        /// 取消一个加载
        /// 注意：
        /// 1.资源载入内存会取消 
        /// 2.AB载入内存不会取消
        /// </summary>
        public void OnCancelLoading(string assetName)
        {
            string assetPath = GetAssetPath(assetName);
            if (_assetLoader.TryGetValue(assetPath, out AssetLoaderParcel loader))
            {
                loader.onComplete = null;
                _assetLoader.Remove(assetPath);

                loader.isActive = false;
                //缓存起来
                //_assetLoadCompletes.Add(assetPath, loader);

            }


        }



        //获得一个正在加载的控制器
        public AssetLoaderParcel GetLoadinger(string assetName)
        {
            string assetPath = GetAssetPath(assetName);
            if (_assetLoader.TryGetValue(assetPath, out AssetLoaderParcel loader)) return loader;

            return null;
        }


        public string GetAssetPath(string assetName)
        {
            string assetPath;
            bool isEditorLoad = AssetProgram.Instance.loadOptions.IsEditorLoad(assetName);
            if (isEditorLoad)
                assetPath = AssetProgram.Instance.loadOptions.GetAssetPathAtName(assetName);
            else
                assetPath = assetName.ToLower();

            return assetPath;
        }

        public void Dispose()
        {
            foreach (var item in _assetLoader) item.Value.PDispose(this);


            foreach (var item in _assetLoadCompletes) item.Value.PDispose(this);

            _assetLoader.Clear(); _assetLoadCompletes.Clear();
        }

    }
}