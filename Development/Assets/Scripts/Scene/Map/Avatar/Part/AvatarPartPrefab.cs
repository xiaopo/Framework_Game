
//龙跃
using AssetManagement;
using UnityEngine;

namespace Game.MScene
{

    //模型,控制器,动画加载
    public class AvatarPartPrefab : AvatarPartBase
    {

        protected Transform mtransfrom;
        public Transform transfrom => mtransfrom;
        public Transform parent;
        protected GameObject mgameObject;
        public GameObject gameObject => mgameObject;

      
        protected AssetLoaderParcel mloader;
        protected  ActionX<AssetLoaderParcel> mNextFun;

        protected bool mIsDone;
        public bool IsDown => mIsDone;
        protected bool isRequested;

        //资源名字
        public string assetName { get; set; }
       
        /// <summary>
        /// 加载模型
        /// </summary>
        /// <param name="assetName"></param>
        public virtual void SendRequest()
        {
            if (isRequested) return;

            isRequested = true;
            mIsDone = false;
            //1.从对象池里拿
            mgameObject = GopManager.Instance.TryGet(GopManager.Avatar).Get(assetName, false);

            //2.走加载

            if (mgameObject == null)
            {
                mloader = AssetUtility.LoadAsset<GameObject>(assetName);
                mNextFun = LoadObjctComplete;
            }

        }


        public override void Update()
        {

            mIsDone = mloader == null;//加载完成

            //加载逻辑
            if (mloader != null && mloader.IsDone())
            {
                AssetLoaderParcel mm = mloader;
                mloader = null;
                mNextFun.Invoke(mm);
            }
        }


        private void LoadObjctComplete(AssetLoaderParcel loader)
        {
         
            if (loader.isFailed)
                GameDebug.Log(string.Format("AvatarPart:LoadComplete 加载失败！{0}", loader.assetName));
            else
            {
                mgameObject = loader.Instantiate<GameObject>();
                mtransfrom = mgameObject.transform;

                mtransfrom.SetParentOEx(parent);

                LoadObjectSuccess();
            }
        }
        
        protected virtual void LoadObjectSuccess()
        {
           
        }

        //重用
        public virtual void Reset()
        {
            isRequested = false;
            assetName = null;
        }
    
        public virtual void Dispose()
        {

            Reset();

            if (mloader != null) mloader.CancelLoading();

            mloader = null;
            mgameObject = null;
            mtransfrom = null;

        }

    }
}
