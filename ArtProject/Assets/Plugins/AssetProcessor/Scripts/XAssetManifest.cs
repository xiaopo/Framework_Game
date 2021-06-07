using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AB 包资源关系映射
/// </summary>

public class XAssetManifest : ScriptableObject, ISerializationCallbackReceiver
{
    //需要构建一个全局封装
    // 1 通过资源可查询 AB包 的名字
    // 2 能获得依赖
    [System.Serializable]
    public class AssetBundleInfo
    {
        public int index;
        public uint crc;
        public int[] depence;
        public string md5;
    }
    [SerializeField]
    private List<string> m_AssetBundleNames = new List<string>();

    //需要转换
    private Dictionary<int, AssetBundleInfo> m_abInfos = new Dictionary<int, AssetBundleInfo>();
    private Dictionary<string, int> m_assetToAB = new Dictionary<string, int>();

    [SerializeField]
    private List<int> m_abInfos_key = new List<int>();
    [SerializeField]
    private List<AssetBundleInfo> m_abInfos_Val = new List<AssetBundleInfo>();
    [SerializeField]
    private List<string> m_assetToAB_key = new List<string>();
    [SerializeField]
    private List<int> m_assetToAB_Val = new List<int>();

    public void OnAfterDeserialize()
    {
       //运行时自动执行反序列化
       //把list转换成Dictionary 方便使用
       for(int i = 0;i< m_abInfos_key.Count;i++)
        {
            if(!m_abInfos.ContainsKey(m_abInfos_key[i]))
            {
                m_abInfos.Add(m_abInfos_key[i],m_abInfos_Val[i]);
            }
        }

        for (int i = 0; i < m_assetToAB_key.Count; i++)
        {
            if (!m_assetToAB.ContainsKey(m_assetToAB_key[i]))
            {
                m_assetToAB.Add(m_assetToAB_key[i], m_assetToAB_Val[i]);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        //序列化之前接收
        //字典是无法序列化的，需要转换成List
        m_abInfos_key.Clear();
        m_abInfos_Val.Clear();
        m_assetToAB_key.Clear();
        m_assetToAB_Val.Clear();

        foreach(KeyValuePair<int,AssetBundleInfo> map in m_abInfos)
        {
            m_abInfos_key.Add(map.Key);
            m_abInfos_Val.Add(map.Value);
        }

        foreach (KeyValuePair<string, int> map in m_assetToAB)
        {
            m_assetToAB_key.Add(map.Key);
            m_assetToAB_Val.Add(map.Value);
        }


    }

    /// <summary>
    /// 获得依赖的AB包
    /// </summary>
    /// <param name="abPath"> 01或02 开始的路径</param>
    /// <returns></returns>
    public int[] GetAllDepenceies(string abPath)
    {
        AssetBundleInfo bdInfo = null;
        int index = m_AssetBundleNames.IndexOf(abPath);
        if(m_abInfos.TryGetValue(index, out bdInfo))
        {
            return bdInfo.depence;
        }

        return new int[0];
    }

    private readonly List<string> tempList = new List<string>();
    public  List<string> GetAllDepenceies_string(string abPath)
    {
        tempList.Clear();
        AssetBundleInfo bdInfo = null;
        int index = m_AssetBundleNames.IndexOf(abPath);
        if (m_abInfos.TryGetValue(index, out bdInfo))
        {
            foreach(var aName in bdInfo.depence)
            {
                tempList.Add(GetABNameByIndex(aName));
            }
        }

        return tempList;
    }


    /// <summary>
    /// 获得资源对于的AB包名字
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public string GetABPathByAssetName(string assetName)
    {
        int index = -1;
        if (!m_assetToAB.TryGetValue(assetName, out index)) return string.Empty;

        return (index == -1 || index  > m_AssetBundleNames .Count - 1) ? string.Empty : m_AssetBundleNames[index];

    }

    public string GetABNameByIndex(int index)
    {
        if(index < 0 || index >= m_AssetBundleNames.Count)return "";

        return m_AssetBundleNames[index];

    }
    public uint GetABCRCByABName(string abPath)
    {
        int index = m_AssetBundleNames.IndexOf(abPath);

        AssetBundleInfo abInfo = null;
        if(m_abInfos.TryGetValue(index,out abInfo))
        {
            return abInfo.crc;
        }

        return 0;

    }

    public bool ContainsAsset(string assetName)
    {
        return !string.IsNullOrEmpty(GetABPathByAssetName(assetName));
    }

#if UNITY_EDITOR
    public void ReBuild(string outPath)
    {
        m_AssetBundleNames.Clear();
        m_abInfos.Clear();
        //同时检测全部的
        string[] children = System.IO.Directory.GetDirectories(outPath);

        Dictionary<string, AssetBundleManifest> manifestes = new Dictionary<string, AssetBundleManifest>();
        foreach(var item in children)
        {
            string manifestPath = System.IO.Path.Combine(item, System.IO.Path.GetFileName(item)).Replace("\\", "/");
            if (!System.IO.File.Exists(manifestPath)) continue;

            //哪部分的
            string part = System.IO.Path.GetFileName(item);

            //下面从 manifest 获得路径全部要加上 part
            AssetBundle mab = AssetBundle.LoadFromFile(manifestPath);
            if (mab == null) continue;

            AssetBundleManifest manifest = mab.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
            mab.Unload(false);
            manifestes.Add(part,manifest);

            string[] assets = manifest.GetAllAssetBundles();
            for (int i = 0; i < assets.Length; i++)
            {
                string fullAbpath = System.IO.Path.Combine(part, assets[i]).Replace("\\", "/");
                m_AssetBundleNames.Add(fullAbpath);
            }
        }

        foreach(var item in manifestes)
        {
            AssetBundleManifest manifest = item.Value;
            string part = item.Key;
            string[] assets = manifest.GetAllAssetBundles();

            for (int i = 0; i < assets.Length; i++)
            {
                string fullAbpath = System.IO.Path.Combine(part, assets[i]).Replace("\\", "/");

                int index = m_AssetBundleNames.IndexOf(fullAbpath);//保存在 m_AssetBundleNames 的 index

                AssetBundleInfo abInfo = new AssetBundleInfo();
                abInfo.index = index;
                string[] depence = manifest.GetAllDependencies(assets[i]);
                abInfo.depence = new int[depence.Length];
                for (int j = 0; j < depence.Length; j++)
                {
                    string depenAb = System.IO.Path.Combine(part, depence[j]).Replace("\\", "/");
                    abInfo.depence[j] = m_AssetBundleNames.IndexOf(depenAb);
                }

                //ab包路径
                string abpath = System.IO.Path.Combine(outPath, fullAbpath).Replace("\\", "/");
                //CRC 码
                uint crc = 0;
                if (UnityEditor.BuildPipeline.GetCRCForAssetBundle(abpath, out crc))
                    abInfo.crc = crc;
                //MD5 码
                abInfo.md5 = LFileUtil.fileMD5(abpath);

                //映射，通过资源名 获得 AB包名
                AssetBundle ab = AssetBundle.LoadFromFile(abpath);
                if (ab != null)
                {
                    string[] assetss = null;
                    if (ab.isStreamedSceneAssetBundle)
                    {
                        assetss = ab.GetAllScenePaths();

                        for(int t = 0;t< assetss.Length;t++)
                        {
                            assetss[t] = System.IO.Path.GetFileName(assetss[t]).ToLower();
                        }
                    }
                    else
                        assetss = ab.GetAllAssetNames();

                    foreach (var asset in assetss)
                    {
                        if (m_assetToAB.ContainsKey(asset))
                        {
#if _Development
                            //重名对AB包自身资源没有影响，不会丢失。但是不会进入记录文件
                            Debug.LogWarning(string.Format("!!!!!资源名称重复，此资源不能被资源管理系统(AssetProgram)加载 {0}", asset));
#endif
                        }
                        else
                        {
                            m_assetToAB.Add(asset, index);
                        }

                    }

                }
                ab.Unload(true);

                //保存AB包数据
                m_abInfos.Add(index, abInfo);
            }
        }

    }
#endif
                    }
