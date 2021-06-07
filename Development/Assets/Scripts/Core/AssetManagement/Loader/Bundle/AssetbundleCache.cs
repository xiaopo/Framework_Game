//龙跃
using System.Collections.Generic;
using UnityEngine;

namespace AssetManagement
{
    public class BundleInfo
    {
        public string path;
        public AssetBundle ab;
  
        private uint _referenceCount;
        /// <summary>
        /// 永不卸载,Custom Shader,Lua ,URP Shader 等等
        /// </summary>
        public bool persistent;
        //正在从AB中加载资源
        //AB引用记数
        //引用计数必须得加上 dependence
        public uint referenceCount
        {
            get { return _referenceCount; }
            set 
            { 
                _referenceCount = value;
                if (value == 0)
                    dissolveTime = Time.time;
                else
                    dissolveTime = 0;
            }
        }

        /// <summary>
        /// 引用计数为0的时刻
        /// </summary>
        public float dissolveTime;
    }


    public class AssetbundleCache
    {

        private static Dictionary<string, BundleInfo> asseBundles = new Dictionary<string, BundleInfo>();

        public static List<BundleInfo> GetAllBundles(List<BundleInfo> outList)
        {
            if (outList == null) outList = new List<BundleInfo>(asseBundles.Count);

            outList.Clear();

            foreach(var item in asseBundles)
            {
                outList.Add(item.Value);
            }


            return outList;
        }

        public static void Add(string abPath, AssetBundle ab,bool persistent = false)
        {
            if(ab == null) return;


            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                if (abInfo.referenceCount > 0)
                    //旧的资源正在被使用，无法替换
                    return;
                else
                {
                    //立即销毁旧资源
                    asseBundles.Remove(abPath);
                    abInfo.ab.Unload(true);
                }
            }

            abInfo = new BundleInfo();
            abInfo.path = abPath;
            abInfo.ab = ab;
            abInfo.referenceCount = 0;
            abInfo.persistent = persistent;

            asseBundles.Add(abPath, abInfo);
        }

        /// <summary>
        /// 1.依赖已经载入内存或加载失败
        /// 2.自己已载入内存或失败
        /// </summary>
        /// <param name="abpath"></param>
        /// <returns></returns>
        public static void Achieve(string abPath,out BundleInfo bundleInfo,out string errorMsg)
        {
 
            errorMsg = string.Empty;
            AssetBundleLoader loser = null;
            if (asseBundles.TryGetValue(abPath, out bundleInfo))
            {
                //检查依赖
                List<string> dependence = AssetProgram.Instance.loadOptions.GetAllDepenceies(abPath);
         
                foreach (var dpPath in dependence)
                {
                    if(!asseBundles.ContainsKey(dpPath))
                    {
                    
                        loser = AssetBundleManager.Instance.GetLoser(dpPath);
                        if (loser != null)
                        {   
                            //依赖加载失败,跳过
                        }
                        else
                        {
                            bundleInfo = null;
                            //依赖还未加载完成
                            return;
                        }

                    }
                }
                
            }
            else
            {
                //自己加载失败
                loser = AssetBundleManager.Instance.GetLoser(abPath);
                if (loser != null)
                {
                    errorMsg = loser.Message;
                    return;
                }
            }
   

        }

        /// <summary>
        /// 已经载入内存
        /// </summary>
        /// <param name="abPath"></param>
        /// <returns></returns>
        public static bool Has(string abPath)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 依赖不一定载入内存
        /// </summary>
        /// <param name="abPath"></param>
        /// <returns></returns>
        public static BundleInfo TryGet(string abPath)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                return abInfo;
            }

            return null;
        }

        public static uint GetRefrenceCount(string abPath)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                return abInfo.referenceCount;
            }

            return 0;
        }

        private static void Operation_depenceies(string abPath, int opr)
        {
            //对引用AB包的计数处理
            List<string> dependence = AssetProgram.Instance.loadOptions.GetAllDepenceies(abPath);
            foreach (var dpPath in dependence)
            {
                BundleInfo dpInfo;
                if (asseBundles.TryGetValue(dpPath, out dpInfo))
                {
                    if (opr == 1)
                        dpInfo.referenceCount++;
                    else if (opr == 2)
                    {
                        if (dpInfo.referenceCount > 0) dpInfo.referenceCount--;
                    }


                }
            }
        }

        public static void AddRefrence(string abPath)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                abInfo.referenceCount++;

                //对引用AB包的计数处理
                Operation_depenceies(abPath, 1);

            }
        }



        public static void UnRefrence(string abPath)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                if (abInfo.referenceCount > 0) abInfo.referenceCount--;

                //对依赖计数处理
                Operation_depenceies(abPath, 2);
            }
        }

        /// <summary>
        /// 单个卸载
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="force"></param>
        public static void TryUnLoad(string abPath,bool force = false)
        {
            BundleInfo abInfo;
            if (asseBundles.TryGetValue(abPath, out abInfo))
            {
                if(abInfo.referenceCount <= 0)
                {
                    if(!abInfo.persistent || force)
                    {
                        if (Time.time - abInfo.dissolveTime > 2.0)
                        {
                            abInfo.ab.Unload(true);
                            asseBundles.Remove(abPath);
                        }
                        
                    }
                   
                }
            }
        }

        /// <summary>
        /// 全面卸载失去引用的AB资源
        /// </summary>
        public static void GC_Scaning()
        {
            List<string> info = new List<string>();

            foreach (var item in asseBundles)
            {
                BundleInfo bdinfo = item.Value;
                if (bdinfo.referenceCount <= 0 && !bdinfo.persistent)
                {
                    if (Time.time - bdinfo.dissolveTime > 2.0)
                    {
                        //释放AB包
                        bdinfo.ab.Unload(true);
                        info.Add(item.Key);
                    }

                }
            }

            foreach (var item in info)
            {
                asseBundles.Remove(item);
            }

            info.Clear();
        }

        public static void Dispose()
        {
            foreach (var item in asseBundles)
            {
                if (!item.Value.persistent)
                    item.Value.ab.Unload(true);
            }

            asseBundles.Clear();
        }
    }

}