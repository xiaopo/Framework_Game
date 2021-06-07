using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGUI
{
    public class XImageNode : MonoBehaviour
    {
        public XImageGroup imageGroup;

        public string imageName = string.Empty;

        [HideInInspector]
        public XImage image;

        void Awake()
        {
            imageGroup.RegisterGroup(this);
            image = gameObject.GetComponent<XImage>();
            if (image != null && imageGroup != null && !string.IsNullOrEmpty(imageGroup.nameSuffix))
            {
                string spriteName = string.Format("{0}_{1}.png", imageName, imageGroup.nameSuffix);
                if (AssetUtility.Contains(spriteName))
                {
                    image.spriteAssetName = spriteName;
                }
                else
                {
                    image.spriteAssetName = imageName + ".png";
                }
            }
        }
    }
}

