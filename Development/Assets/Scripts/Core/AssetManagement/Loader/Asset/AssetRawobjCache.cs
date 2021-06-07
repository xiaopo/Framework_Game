//龙跃
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace AssetManagement
{
    public class RawAsset
    {
        public string abPath;//001/SDF/ss.png
        public string name;//assetName
        public Object asset;

        /// <summary>
        /// 被引用次数，多次实例化只产生一次引用
        /// </summary>
        public uint referenceCount;

        /// <summary>
        /// 实例化次数
        /// </summary>
        public uint instanceCount;


        public uint assetBundleReferenceCount
        {
            get
            {
                return AssetbundleCache.GetRefrenceCount(abPath);
            }
        }
    }

    public class AssetRawobjCache
    {
        /// <summary>
        /// 不在被应用的源对象
        /// </summary>
        protected static List<RawAsset> garbage = new List<RawAsset>(64);

        /// <summary>
        /// 源对象数据<rawObj,RawAsset>
        /// </summary>
        protected static Dictionary<Object, RawAsset> rawAssets = new Dictionary<Object, RawAsset>();
        /// <summary>
        /// 实例化数据和源数据的关系<instanceId,RawAsset>
        /// </summary>
        protected static Dictionary<int, RawAsset> instance_raw_relation = new Dictionary<int, RawAsset>();

        public static List<RawAsset> GetRawAssets(List<RawAsset> outlist) 
        {
            outlist.Clear();

            foreach(var item in rawAssets)
                outlist.Add(item.Value);

            return outlist;
        }
      


        protected static RawAsset AddInfo(string assetName, string abPath, Object asset)
        {
            if (asset == null) return null;

            RawAsset rawInfo = null;
            if (rawAssets.TryGetValue(asset, out rawInfo))
            {
                return rawInfo;
            }

            if (rawInfo == null)
            {
                //从垃圾桶找
                for (int i = 0; i < garbage.Count; i++)
                {
                    if (garbage[i].asset == asset)
                    {
                        rawInfo = garbage[i];
                        garbage.RemoveAt(i);
                        break;
                    }
                }
            }

            if (rawInfo == null) rawInfo = new RawAsset();

            rawInfo.abPath = abPath;
            rawInfo.name = assetName;
            rawInfo.asset = asset;
            rawInfo.referenceCount = 0;
            rawInfo.instanceCount = 0;

            rawAssets.Add(asset, rawInfo);

            return rawInfo;
        }

        protected static void RemoveInfo(RawAsset rawInfo)
        {
            garbage.Add(rawInfo);

            //如果报错说明逻辑有问题
            rawAssets.Remove(rawInfo.asset);
        }

        protected static void AddRefrence(RawAsset rawInfo)
        {
            rawInfo.referenceCount++;

            //同步AssetBundle引用 计数
            AssetbundleCache.AddRefrence(rawInfo.abPath);
        }

        protected static void UnRefrence(RawAsset rawInfo)
        {

            if (rawInfo.referenceCount > 0)
            {
                rawInfo.referenceCount--;
                //同步AssetBundle引用 计数
                AssetbundleCache.UnRefrence(rawInfo.abPath);
            }

            if (rawInfo.referenceCount <= 0)
            {
                //进垃圾桶
                RemoveInfo(rawInfo);
         
            }
        }
        /// <summary>
        /// 获得一个源对象。必须使用 DiscardRawobj 释放
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Object GetRawAsset(string abPath, string assetName, AssetBundleRequest request)
        {
            Object asset = request.asset;
            if (asset == null) return null;

            RawAsset rawInfo = null;
            rawAssets.TryGetValue(asset, out rawInfo);

            if (rawInfo != null)
                AddRefrence(rawInfo);//
            else
            {
                
                rawInfo = AddInfo(assetName, abPath, asset);
                AddRefrence(rawInfo);
            }

            return asset;
        }

        /// <summary>
        /// 取消一次对源对象的引用
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <param name="rawAsset"></param>
        public static void DiscardRawAsset(Object rawAsset)
        {
            if (rawAsset == null) return;

            RawAsset rawInfo = null;
            rawAssets.TryGetValue(rawAsset, out rawInfo);
            if (rawInfo != null)
            {
                UnRefrence(rawInfo);
            }

            //源对象的销毁，AssetBundle.Unload(true)
        }


        /// <summary>
        /// 游戏中资源实例化的唯一接口，必须使用 DestroyInstance 释放
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Object Instantiate(string abPath, string assetName, Transform parent, AssetBundleRequest request)
        {
            Object asset = request.asset;
            if (asset == null) return null;

            RawAsset rawInfo = null;
            rawAssets.TryGetValue(asset, out rawInfo);

            if (rawInfo == null )
                rawInfo = AddInfo(assetName, abPath, asset);


            //实例化对象，只对应一次引用计数的变化
            if (rawInfo.instanceCount == 0) AddRefrence(rawInfo);

            rawInfo.instanceCount++;

            Object obj = Object.Instantiate(asset, parent);


            instance_raw_relation.Add(obj.GetInstanceID(), rawInfo);

            return obj;


        }

        /// <summary>
        /// 销毁一个 Instance 对象
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="abPath"></param>
        /// <param name="obj"></param>
        public static void DestroyInstance(Object obj)
        {
            if (obj.Equals(null)) return;

 
            RawAsset rawInfo = null;

            if (instance_raw_relation.TryGetValue(obj.GetInstanceID(), out rawInfo))
            {
                if (rawInfo.instanceCount > 0) rawInfo.instanceCount--;

                if (rawInfo.instanceCount == 0) UnRefrence(rawInfo);
            }


            if (obj is GameObject)
                Object.Destroy(obj, 0);
            else if (obj is Material)
                Object.DestroyImmediate(obj);

        }

        /// <summary>
        /// 清理垃圾桶
        /// </summary>

        public static void DumpTrash()
        {
            foreach (var item in garbage)
            {
                item.asset = null;
            }

            garbage.Clear();

            //释放AssetBundle
            AssetbundleCache.GC_Scaning();
        }

    }
}