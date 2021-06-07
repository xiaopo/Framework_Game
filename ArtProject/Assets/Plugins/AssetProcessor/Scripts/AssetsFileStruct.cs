
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XAssetsFiles : ISerializationCallbackReceiver
{
    public static XAssetsFiles s_BuildingtAssets;
    public static XAssetsFiles s_CurrentAssets;
    public static XVersionFile s_CurrentVersion;
    public enum FileOptions : int
    {
        NONE = 0,
        //首包/内置文件
        BUILDING = 1,
        //DLL 文件
        DLL = 2,
        //lua 文件    
        LUA = 4,
        //安装包文件  
        INSTALL = 8,
        //启动更新
        LAUNCHDOWNLOAD = 16,
        //所有
        All = ~0,
    }



    [System.Serializable]
    public class FileStruct
    {
        //文件路径
        public string path;
        //md5
        public string md5;
        //文件大小
        public int size;
        //包标记
        public sbyte tag = -1;
        //下载优先级
        public sbyte priority = -1;
        //文件选项
        public FileOptions options = FileOptions.NONE;
    }

    public int p_FileCount;
    public List<FileStruct> p_AllFiles;
    private Dictionary<string, FileStruct> m_AllFilesMap = new Dictionary<string, FileStruct>();
    public Dictionary<string, FileStruct> allFilesMap { get { return m_AllFilesMap; } }
    public void OnBeforeSerialize()
    {
        p_FileCount = p_AllFiles != null ? p_AllFiles.Count : 0;
    }

    public void OnAfterDeserialize()
    {
        m_AllFilesMap.Clear();
        foreach (var item in p_AllFiles)
            m_AllFilesMap.Add(item.path, item);
    }

#if UNITY_EDITOR
    public void Clear()
    {
        if (p_AllFiles == null)
            p_AllFiles = new List<FileStruct>();
        p_AllFiles.Clear();
        if (m_AllFilesMap == null)
            m_AllFilesMap = new Dictionary<string, FileStruct>();
        m_AllFilesMap.Clear();
    }

    public void Add(FileStruct fs)
    {
        if (!m_AllFilesMap.ContainsKey(fs.path))
        {
            p_AllFiles.Add(fs);
            m_AllFilesMap.Add(fs.path, fs);
        }
    }
#endif
}

[System.Serializable]
public class XVersionFile
{
    [System.Serializable]
    public struct VersionStruct
    {
        public string svnVer;
        public string buildDate;
    }

    public VersionStruct p_LuaVersion;
    public VersionStruct p_DevVersion;
    public VersionStruct p_ArtVersion;

}
