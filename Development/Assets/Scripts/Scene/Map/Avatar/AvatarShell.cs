//龙跃
using System.Collections.Generic;
using UnityEngine;

namespace Game.MScene
{


   //单个模型显示
   //身体全部部件都在一个模型上
    public class AvatarShell: AvatarBase
    {
        protected AvatarCommonBone commonBone;
        protected List<AvatarPartAnim> avatarParts = new List<AvatarPartAnim>();
        protected ActionX completeFun;

        protected Dictionary<string, ActionX<AvatarPartAnim>> partcompleteFuns = new Dictionary<string, ActionX<AvatarPartAnim>>();

        protected Transform root;

        protected bool invalidLoading = false;
        public AvatarShell(Transform rot)
        {
            this.root = rot;
        }

        public void BindListener(ActionX comFun)
        {
            completeFun = comFun;
        }

        public void BindListener(string assetName, ActionX<AvatarPartAnim> comFun)
        {
            partcompleteFuns.Remove(assetName);
            partcompleteFuns.Add(assetName, comFun);
        }

        /// <summary>
        /// 公用骨骼预制体
        /// </summary>
        /// <param name="assetName"></param>
        public void RequestBone(string assetName, string controllerName, string animsPackName)
        {
            if (commonBone == null) commonBone = new AvatarCommonBone();
            commonBone.assetName = assetName;
            commonBone.controllerName = controllerName;
            commonBone.animsPackName = animsPackName;
            commonBone.parent = this.root;

            commonBone.SendRequest();
        }

        /// <summary>
        /// 模型加载，可加载控制器和动画
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="parent"></param>
        /// <param name="controllerName"></param>
        /// <param name="animsPackName"></param>
        public void AttachModel(string assetName,string parent,string controllerName,string animsPackName)
        {
            invalidLoading = false;

            AvatarPartAnim part = null;
            for(int i = 0;i< avatarParts.Count;i++)
            {
                if(avatarParts[i].assetName == assetName)
                {
                    part = avatarParts[i];
                    break;
                }
            }

            if (part == null)
            {
                part = new AvatarPartAnim();
                avatarParts.Add(part);
            }

            part.assetName = assetName;
            part.controllerName = controllerName;
            part.animsPackName = animsPackName;
            //if (string.IsNullOrEmpty(parent))
            part.parent = this.root;

        }

        public void SendRequestModel()
        {
            invalidLoading = true;
           
        }

        public override void Update(float time)
        {
          
            if(invalidLoading)
            {
                if(commonBone == null || commonBone.IsDown)
                {
                    foreach (var item in avatarParts)
                    {
                        //开始加载模型
                        item.SendRequest();
                    }
                    invalidLoading = true;
                }

            }

        }


        protected void OnLoadComplete()
        {

        }


        public override void Clean()
        {
            base.Clean();

            partcompleteFuns.Clear();
        }

    }
}
