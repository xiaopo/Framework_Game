using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace AssetManagement
{
    public interface IAssetLoaderOptions
    {

        /// <summary>
        /// 获得AssetBundle资源的依赖
        /// </summary>
        /// <param name="assetBundlename"></param>
        /// <returns></returns>
        List<string> GetAllDepenceies(string assetBundlename);

        /// <summary>
        /// 返回AssetBundle的Crc校验值
        /// </summary>
        /// <param name="assetBundlename"></param>
        /// <returns></returns>
        uint GetABCRCByABName(string assetBundlename);

        /// <summary>
        /// 根据传入的资源名 返回AssetBundle名
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        string GetABPathByAssetName(string assetName);


        /// <summary>
        /// 编辑器模式加载
        /// </summary>
        /// <returns></returns>
        bool IsEditorLoad(string assetName);

        /// <summary>
        /// 是否存在此资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        bool ContainsAsset(string assetName,int checkTag = -1);

        /// <summary>
        /// 根据资源名返回资源路径如   a.png -> Assets/a.png
        /// </summary>
        /// <param name="asssetName"></param>
        /// <returns></returns>
        string GetAssetPathAtName(string asssetName);

    }

    public class DefaultAssetLoaderOptions : IAssetLoaderOptions
    {
        protected AssetBundleManifest m_AssetBundleManifest;

        public DefaultAssetLoaderOptions()
        {

        }

        public DefaultAssetLoaderOptions(AssetBundleManifest manifest)
        {
            this.m_AssetBundleManifest = manifest;
        }


        private readonly List<string> tempList = new List<string>();
        public virtual List<string> GetAllDepenceies(string assetBundlename)
        {
            tempList.Clear();
            string[] depends = m_AssetBundleManifest.GetAllDependencies(assetBundlename);
            foreach (var dname in depends)
            {
                tempList.Add(dname);
            }

            return tempList;
        }

        public virtual string GetABPathByAssetName(string assetName)
        {
            return string.Empty;
        }

        public virtual uint GetABCRCByABName(string assetBundlename)
        {
            return 0;
        }


        public virtual string GetAssetPathAtName(string asssetName)
        {
            return asssetName;
        }

        public virtual bool IsEditorLoad(string assetName)
        {
            return true;
        }


        public virtual bool ContainsAsset(string assetName, int checkTag = -1)
        {
            return false;
        }

    }
}
