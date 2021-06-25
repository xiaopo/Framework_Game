//龙跃
using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

//加载资源使用接口
namespace AssetManagement
{
    public class AssetUtility
    {
        public static AssetLoaderParcel GetLoadinger(string assetName)
        {
            return AssetsGetManger.Instance.GetLoadinger(assetName);
        }

        public static AssetLoaderParcel LoadAsset<T>(string assetName)
        {

            return AssetsGetManger.Instance.LoadAsset(assetName, typeof(T));
        }

        public static AssetLoaderParcel LoadAsset(string assetName, Type assetType)
        {

            return AssetsGetManger.Instance.LoadAsset(assetName, assetType);
        }


        public static AssetLoaderParcel PreLoad(string assetName)
        {
            return AssetsGetManger.Instance.PreloadAsset(assetName, typeof(GameObject));
        }



        /// <summary>
        /// 解散一个RawObj的引用
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="t"></param>
        public static void DiscardRawAsset(Object asset, float t = 0.0f)
        {
            AssetRawobjCache.DiscardRawAsset(asset);
        }

        /// <summary>
        /// 销毁一个Instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="t"></param>
        
        public static void DestroyInstance(Object instance,float t = 0.0f)
        {
            AssetRawobjCache.DestroyInstance(instance);
        }

        /// <summary>
        /// 是否有这个资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static bool Contains(string assetName)
        {
            return AssetProgram.Instance.loadOptions.ContainsAsset(assetName);
        }

        public static bool CheckExists(string abpath, out string downPath, out string savePath, out string loadPath)
        {
            string down_path = AssetDefine.RemoteDownloadUrls[0] + abpath;//下载路径
            string sd_path = Path.Combine(AssetDefine.ExternalSDCardsPath, abpath);//扩展卡
            string st_path = Path.Combine(AssetDefine.BuildinAssetPath, abpath);//APk 包内
            string load_path = sd_path;//加载路径

            bool nothing = false;
            //扩展卡不存在
            if (!File.Exists(sd_path)) nothing = true;

            if (nothing)
            {
                if (File.Exists(st_path))
                {
                    // APk 存在
                    nothing = false;
                    load_path = st_path;
                }
            }

            downPath = down_path;
            savePath = sd_path;
            loadPath = load_path;

            return nothing;
        }


    }
}
