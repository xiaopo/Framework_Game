using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 编辑器资源，映射
/// </summary>
public class EAssetManifest : ScriptableObject, ISerializationCallbackReceiver
{
    public const string s_AssetManifestPath = "Assets/Plugins/AssetProcessor/AssetManifest.asset";

    [System.Serializable]
    public class AssetInfo
    {
        public string m_AssetName;
        public string m_AssetPath;
        public AssetInfo(string assetName, string assetPath)
        {
            this.m_AssetName = assetName;
            this.m_AssetPath = assetPath;
        }

    }

    [SerializeField]
    private List<string> m_MatchingFolder = new List<string>() { "Assets" };
    public List<string> MatchingFolder { get { return m_MatchingFolder; } }
    [SerializeField]
    private List<string> m_IgnoreFileType = new List<string> { "meta", "cs" };
    public List<string> IgnoreFileType { get { return m_IgnoreFileType; } }

    [SerializeField]
    private List<AssetInfo> m_AssetList = new List<AssetInfo>();
    public List<AssetInfo> AssetList { get { return m_AssetList; } }
    private Dictionary<string, string> m_AssetMap = new Dictionary<string, string>();


    public void OnBeforeSerialize()
    {
        this.m_AssetList.Clear();
        foreach (var item in this.m_AssetMap)
            m_AssetList.Add(new AssetInfo(item.Key, item.Value));
    }

    public void OnAfterDeserialize()
    {
        foreach (var item in m_AssetList)
            if (!this.m_AssetMap.ContainsKey(item.m_AssetName))
                this.m_AssetMap.Add(item.m_AssetName, item.m_AssetPath);
    }

    public void Clear()
    {
        this.m_AssetList.Clear();
        this.m_AssetMap.Clear();
    }

    public string GetAssetPath(string assetName)
    {
        if (this.m_AssetMap.ContainsKey(assetName))
            return this.m_AssetMap[assetName];
        return string.Empty;
    }

    public bool ContainsAsset(string assetName)
    {
        return this.m_AssetMap.ContainsKey(assetName);
    }

#if UNITY_EDITOR
    public void AddAsset(string assetPath)
    {
        string assetName = Path.GetFileName(assetPath);
        if (!this.m_AssetMap.ContainsKey(assetName))
        {
            this.m_AssetMap.Add(assetName, assetPath);
        }
        else
        {
            this.m_AssetMap[assetName] = assetPath;
        }
    }

    public void DeleteAsset(string assetPath)
    {
        string assetName = Path.GetFileName(assetPath);
        string sPath;
        if (this.m_AssetMap.TryGetValue(assetName, out sPath))
            if (sPath == assetPath)
                this.m_AssetMap.Remove(assetName);
    }

    public static EAssetManifest EditorLoadAssetManifest()
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<EAssetManifest>(EAssetManifest.s_AssetManifestPath);
    }
#endif
}
