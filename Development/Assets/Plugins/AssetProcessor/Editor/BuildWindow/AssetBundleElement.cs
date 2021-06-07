using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AssetsFileOrm.FileOrm;

namespace BuildTool
{
    public class AssetBundleElement : TreeElement
    {
        public bool enabled;
        public AssetBundleInfo assetBundleInfo;
        public AssetBundleElement(string _name)
        {
            assetBundleInfo = new AssetBundleInfo();
            name = _name;
        }
        public AssetBundleElement(AssetInfo assetInfo)
        {
            name = assetInfo.p_AssetName;
            assetBundleInfo = new AssetBundleInfo();
            assetBundleInfo.p_AssetBundleName = assetInfo.p_AssetName;
        }
        public AssetBundleElement(AssetBundleInfo _assetBundleInfo)
        {
            assetBundleInfo = _assetBundleInfo;
            name = _assetBundleInfo.p_AssetBundleName;
        }
    }
}
