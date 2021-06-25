
//龙跃
using AssetManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MScene
{
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    //模型加载
    public  class AvatarPartAnim: AvatarPartMaterial
    {
       
        public string controllerName { get; set; } //动画控制器名字
        protected Animator manimator;
        public Animator animator => manimator;

        public string animsPackName { get; set; }//动画包
        //资源
        protected AnimationPack animPrepack;
        protected List<AssetLoaderParcel> assetsLoader = new List<AssetLoaderParcel>();

        protected override void LoadObjectSuccess()
        {
            if (!string.IsNullOrEmpty(controllerName))
            {
                //加载控制器
                mloader = AssetUtility.LoadAsset<RuntimeAnimatorController>(controllerName);
                mNextFun = LoadCtrlComplete;
            }
        }


        #region 流程加载 Obj,controller,Animations
        protected virtual void LoadCtrlComplete(AssetLoaderParcel loader)
        {

            manimator = mgameObject.TryGetComponent<Animator>();
            InsteadController(loader.GetRawObject<RuntimeAnimatorController>());

            if (!string.IsNullOrEmpty(animsPackName))
            {
                //加载动画
                mloader = AssetUtility.LoadAsset<AnimationPack>(animsPackName);
                mNextFun = LoadAnimPackComplete;
            }

        }

        protected virtual void LoadAnimPackComplete(AssetLoaderParcel loader)
        {
            //注意：动画资源的名字是和AnimatorController的StateName对应的
            animPrepack = loader.GetRawObject<AnimationPack>();
            if (animPrepack != null && animPrepack.Count > 1)
            {
                foreach (var item in animPrepack.clipMap)
                    clipOverrides[item.Key] = item.Value;

                overrideController.ApplyOverrides(clipOverrides);
                clipOverrides.Clear();
            }
            else
                GameDebug.LogWarning(string.Format("{0} 动画资源包有问题！请查看", gameObject.name));

        }

        protected AnimationClipOverrides clipOverrides;
        protected AnimatorOverrideController overrideController;
       
        /// <summary>
        /// 设置控制器
        /// </summary>
        /// <param name="newContrl"></param>
        /// <param name="contrlName"></param>
        protected void InsteadController(RuntimeAnimatorController newContrl)
        {

            if(overrideController.runtimeAnimatorController != null)
                AssetUtility.DiscardRawAsset(overrideController.runtimeAnimatorController);
   

            overrideController = new AnimatorOverrideController();
            clipOverrides = new AnimationClipOverrides(overrideController.overridesCount);
            overrideController.GetOverrides(clipOverrides);


            overrideController.runtimeAnimatorController = newContrl;
            manimator.runtimeAnimatorController = overrideController;
        }

        #endregion

        #region 外部调用直接改变 Controller 和 Animation，身体必须先加载好
        public void SetControllerAsync(string controllerName, ActionX completefun = null, ActionX loadFailed = null)
        {
            if (gameObject == null || string.IsNullOrEmpty(controllerName))
            {
                loadFailed?.Invoke();
                return;
            }

            if (manimator.runtimeAnimatorController != null)
            {
                string ctlName = System.IO.Path.GetFileNameWithoutExtension(controllerName);
                if (manimator.runtimeAnimatorController.name == ctlName)
                {
                    completefun?.Invoke();
                    return;
                }
            }


            AssetLoaderParcel loader = AssetUtility.LoadAsset<RuntimeAnimatorController>(controllerName);
            loader.onComplete += delegate (AssetLoaderParcel loa)
            {
                assetsLoader.Remove(loa);

                if (loa.IsSucceed())
                {
                    InsteadController(loa.GetRawObject<RuntimeAnimatorController>());
                    completefun?.Invoke();
                }
                else
                    loadFailed?.Invoke();


            };

            assetsLoader.Add(loader);

        }

        /// <summary>
        /// 装配 AssembleAnimation
        ///  0 无  1 已赋值   2 执行赋值  3 需要加载动画
        /// </summary>
        public int AssembleAnimation(string animName, string emptyKey, ActionX completefun = null, ActionX loadFailed = null)
        {
            if (CanNotUse() || string.IsNullOrEmpty(emptyKey) || string.IsNullOrEmpty(animName)) return 0;
            AnimationClip eclip = overrideController[emptyKey];
            if (eclip != null )
            {
                string newClipName = System.IO.Path.GetFileNameWithoutExtension(animName);
                if (newClipName == eclip.name)
                {
                    //已经赋值
                    return 1;
                }

                overrideController[emptyKey] = null;
               
                AnimationClip cacheClip = AssetRawobjCache.GetRawAsset<AnimationClip>(animName);
                if (cacheClip != null)
                {
                    //动画使用前，请先预加载
                    //赋值
                    overrideController[emptyKey] = cacheClip;//播放的时需要等待一帧
                    return 2;
                }
                else
                {
                    //需要加载
                    //记得取消加载和返回Raw资源
                    AssetLoaderParcel loader = AssetUtility.LoadAsset<AnimationClip>(animName);
                    loader.onComplete += delegate (AssetLoaderParcel loa) 
                    {
                        assetsLoader.Remove(loa);

                        if (loa.IsSucceed())
                        {
                            AnimationClip newClip = loa.GetRawObject<AnimationClip>();
                            overrideController[emptyKey] = newClip;
                            completefun?.Invoke();
                        }
                        else
                            loadFailed?.Invoke();


                    };

                    assetsLoader.Add(loader);
                    return 3;
                }

            }
            else
            {
                if (loadFailed != null) { loadFailed.Invoke(); }
                GameDebug.Log(string.Format("动画控制器无{0}原始动画\n  attempt get {1}", emptyKey, animName));
            }

            return 0;

        }

        #endregion
        public bool CanNotUse()
        {
            return manimator == null || overrideController == null;

        }


        public override void Reset()
        {
            base.Reset();

           
            //取消加载
            foreach (var item in assetsLoader)
                item.CancelLoading();

            assetsLoader.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();

            animsPackName = null;
            controllerName = null;

            //取消资源管理器的引用计数
            AssetUtility.DiscardRawAsset(animPrepack);

            if (overrideController != null)
            {
                AnimationClip[] clips = overrideController.animationClips;
                if (clips != null)
                {
                    foreach (var clip in clips)
                        AssetUtility.DiscardRawAsset(clip);
                }

                AssetUtility.DiscardRawAsset(overrideController.runtimeAnimatorController);
                overrideController.runtimeAnimatorController = null;
            }


            animPrepack = null;
            overrideController = null;
            manimator = null;

        }

    }
}
