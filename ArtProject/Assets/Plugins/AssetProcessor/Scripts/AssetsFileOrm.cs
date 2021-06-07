using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class AssetsFileOrm
{
    static string errorpath;
    static void log(string str)
    {
        File.AppendAllText(errorpath, str + "\r\n");
#if UNITY_EDITOR
        UnityEngine.Debug.Log(str);
#else
        System.Console.WriteLine(str);
#endif

    }


    private Dictionary<string, FileOrm> m_AllFileOrm;
    public Dictionary<string, FileOrm> allFileOrm
    {
        get { return m_AllFileOrm; }
    }
    static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    [Serializable]
    public class FileOrm
    {

        [Serializable]
        public class AssetInfo
        {
            public string p_AssetName;
            public string p_AssetPath;
        }

        [Serializable]
        public class AssetBundleInfo
        {
            public string p_AssetBundleName;
            //public string p_AssetHashBundleName;
            public string p_AssetBundleHash;
            public string p_AssetCRC;
            public string p_AssetBundleMd5;
            public List<AssetInfo> p_Assets;
        }

        public List<AssetBundleInfo> p_AssetBundleList;

#pragma warning disable 0414
        private string m_FilePath;
        public FileOrm(string filePath) { m_FilePath = filePath; }


        public static FileOrm Load(string filePath)
        {
            FileOrm fileOrm = new FileOrm(filePath);
            UnityEngine.JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), fileOrm);
            return fileOrm;
        }

        public static FileOrm Load(string path, string data)
        {
            FileOrm fileOrm = new FileOrm(path);
            UnityEngine.JsonUtility.FromJsonOverwrite(data, fileOrm);
            return fileOrm;
        }
    }

    public void Load(string folder)
    {
        errorpath = Path.Combine(folder, "error.log");
        if (File.Exists(errorpath))
            File.Delete(errorpath);

        File.Create(errorpath).Close();

        if (!Directory.Exists(folder))
        {
            log(string.Format("AssetsFileOrm::Load() 目录不存在 folder={0}", folder));
            return;
        }
        
        string[] orms = Directory.GetFiles(folder, "fileorm.txt", SearchOption.AllDirectories);

        m_AllFileOrm = new Dictionary<string, FileOrm>();
        log(string.Format("AssetsFileOrm::Load() fileorm.txt count {0}", orms.Length));
        foreach (var path in orms)
        {
            FileOrm orm = FileOrm.Load(path);
            string name = Path.GetFileName(Path.GetDirectoryName(path));
            m_AllFileOrm.Add(name, orm);
            log(string.Format("AssetsFileOrm::Load() add orm path = {0}", path));
        }
        
        RepeCheck();
    }

    public void RepeCheck()
    {
        Dictionary<string, FileOrm.AssetBundleInfo> dic = new Dictionary<string, FileOrm.AssetBundleInfo>();
        Dictionary<string, FileOrm.AssetInfo> dic2 = new Dictionary<string, FileOrm.AssetInfo>();
        foreach (var item in m_AllFileOrm)
        {
            foreach (var info in item.Value.p_AssetBundleList)
            {
                if (dic.ContainsKey(info.p_AssetBundleName))
                {
                    FileOrm.AssetBundleInfo exist = dic[info.p_AssetBundleName];
                    log(string.Format("重复资源包 AssetBundleName [{0}]:[{1}]  [{2}]:[{3}]", exist.p_AssetBundleName, exist.p_AssetBundleName, info.p_AssetBundleName, info.p_AssetBundleName));
                }
                else
                {
                    dic.Add(info.p_AssetBundleName, info);
                }


                if (info.p_Assets == null)
                    continue;

                foreach (var asset in info.p_Assets)
                {
                    if (dic2.ContainsKey(asset.p_AssetName))
                    {
                        FileOrm.AssetInfo exist = dic2[asset.p_AssetName];
                        log(string.Format("重复资源 AssetName [{0}]:[{1}]   [{2}]:[{3}]", exist.p_AssetName, exist.p_AssetPath, asset.p_AssetName, asset.p_AssetPath));
                    }
                    else
                    {
                        dic2.Add(asset.p_AssetName, asset);
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    public static AssetsFileOrm.FileOrm CreateFileOrm(string path, UnityEditor.AssetBundleBuild[] obuilds, UnityEditor.AssetBundleBuild[] builds, List<string> hashs,List<string> crcs)
    {
        FileOrm fileOrm = new FileOrm(path);
        fileOrm.p_AssetBundleList = new List<FileOrm.AssetBundleInfo>();

        FileOrm.AssetBundleInfo info;

        string parent = Path.GetDirectoryName(path);
        for (int j = 0; j < builds.Length; j++)
        {

            UnityEditor.AssetBundleBuild item = obuilds[j];
            info = new FileOrm.AssetBundleInfo();
            fileOrm.p_AssetBundleList.Add(info);
           
            string bundleName = item.assetBundleName;
            info.p_AssetBundleName = bundleName;
            //info.p_AssetHashBundleName = builds[j].assetBundleName;
            info.p_AssetBundleHash = hashs[j];
            info.p_AssetCRC = crcs[j];
   
            string abPath = Path.Combine(parent, info.p_AssetBundleName);
            if (File.Exists(abPath))
                info.p_AssetBundleMd5 = md5file(abPath);

            if (item.assetNames.Length > 0)
            {
                info.p_Assets = new List<FileOrm.AssetInfo>();
                for (int i = 0; i < item.assetNames.Length; i++)
                {
                    info.p_Assets.Add(new FileOrm.AssetInfo
                    {
                        p_AssetName = item.addressableNames[i],
                        p_AssetPath = item.assetNames[i],
                    });
                }
            }
        }

        if (File.Exists(path))
            File.Delete(path);

        File.WriteAllText(path, UnityEngine.JsonUtility.ToJson(fileOrm, true));

        return fileOrm;
    }
#endif

    public static AssetsFileOrm OpenAll(string folder)
    {
        AssetsFileOrm fileorm = new AssetsFileOrm();
        fileorm.Load(folder);
        return fileorm;
    }
}
