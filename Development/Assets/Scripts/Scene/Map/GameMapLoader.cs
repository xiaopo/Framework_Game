using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{ 
    public enum LoadMapMode
    {   

        Single = 0,
        //上一个场景,全部GameObject继续显示
        Additive_Add = 1,
        //上一个场景，全部GameObject隐藏
        Additive_Over = 2

    }
    /// <summary>
    /// 游戏世界地图加载器
    /// 
    /// </summary>
    public class GameMapLoader
    {
        public ActionX LoadComplete = null;
        protected LoadMapMode loadSceneMode;
        protected float clampTime;
        protected string signalName;

        public GameMapLoader()
        {
            signalName = "GameMapLoader";
        }

        public void Preload(string assetName)
        {
            AssetsGetManger.Instance.PreloadAsset(assetName, typeof(GameObject));
        }
        public bool Has(string assetName)
        {
            Scene scene = SceneManager.GetSceneByName(assetName);

            return scene != null && scene.isLoaded;
        }

        /// <summary>
        /// 保持一个最小的加载时间，让加载界面多活一会
        /// </summary>
        /// <param name="clampTime">最小保持加载时间</param>
        /// <param name="assetName"></param>
        /// <param name="loadType"></param>
        public AssetLoaderParcel LoadClamp(float clampTime,string assetName, LoadMapMode loadType = LoadMapMode.Single)
        {
            this.clampTime = clampTime;

            SignalActionTool.Single(clampTime, signalName);


            return Load(assetName, loadType);

          
        }

        public AssetLoaderParcel Load(string assetName, LoadMapMode loadType = LoadMapMode.Single)
        {
            loadSceneMode = loadType;

            Scene scene = SceneManager.GetSceneByName(assetName);
            if (scene != null && scene.isLoaded)
            {
                if (scene == SceneManager.GetActiveScene())
                    GameDebug.LogRed("场景不能重复加载  " + assetName);
                else
                    this.OnLoadComplete(scene);//激活

                return null;
            }

            SceneManager.sceneLoaded += OnUnitySceneLoaded;


            AssetLoaderParcel abBundle = AssetsGetManger.Instance.LoadAsset(assetName,typeof(GameObject));
            abBundle.loadSceneMode = loadType == LoadMapMode.Single ? LoadSceneMode.Single : LoadSceneMode.Additive;
            abBundle.onComplete = OnAssetLoadComplete;//覆盖

            return abBundle;
        }


        public void UnLoadAll()
        {
            TimerManager.AddCoroutine(UnLoadAll_Interior());
        }

        internal IEnumerator UnLoadAll_Interior()
        {
            if(SceneManager.sceneCount == 1)
            {
                if(SceneManager.GetSceneAt(0).name == "Empty") yield return null;
            }

            bool isEmpty = false;
            while(!isEmpty)
            {
                Scene cur = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                yield return Unload(cur, out isEmpty);
            }

            yield return null;
        }

        public  AsyncOperation Unload(string assetName, out bool isEmpty)
        {
            Scene cur = SceneManager.GetSceneByName(assetName);
            return Unload(cur, out isEmpty);
        }

        public  AsyncOperation Unload(Scene cur, out bool isEmpty)
        {
           
            isEmpty = true;
            if (cur == null)  return null;

            if (SceneManager.sceneCount == 1)
            {
                //当只剩最后一个场景时，API无法继续卸载
                //创建一个空场景 来填充最后一个场景
                Scene empty = SceneManager.CreateScene("Empty");
                SceneManager.SetActiveScene(empty);
                isEmpty = true;
            }
            else
                isEmpty = false;

            OnUnLoadScene(cur);
            AsyncOperation any = SceneManager.UnloadSceneAsync(cur);

            return any;
        }



        protected  void OnAssetLoadComplete(AssetLoaderParcel loader)
        {
            loader.onComplete -= OnAssetLoadComplete;

        }

       
        protected  void OnUnitySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnUnitySceneLoaded;

            OnLoadComplete(scene);

            InitScene(scene);
        }

        protected virtual void OnLoadComplete(Scene scene)
        {
            if (loadSceneMode == LoadMapMode.Additive_Over)
            {
                //叠加模式时影藏上一个场景的物体
                Scene preScene = SceneManager.GetActiveScene();
                if(preScene != null)
                {
                    GameObject[] objects = preScene.GetRootGameObjects();
                    foreach (var obj in objects)
                    {
                        obj.SetActive(false);

                    }
                }
                
            }

            SignalActionTool.Check(delegate () 
            {
                SceneManager.SetActiveScene(scene);
                LoadComplete?.Invoke();
                LoadComplete = null;

            }, this.signalName);
           
        }

        //只处理加载场景
        protected virtual void InitScene(Scene scene)
        {
            //GameObject[] objects = scene.GetRootGameObjects();
        }

        protected virtual void OnUnLoadScene(Scene cur)
        {

        }

        
    }
}