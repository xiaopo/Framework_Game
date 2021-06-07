using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetManifestProcessor : UnityEditor.AssetModificationProcessor
{

    [InitializeOnLoadMethod]
    static void AssetManifestInitializeOnLoadMethod()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {

        }
    }



    static bool s_LogEnable = true;
    static EAssetManifest s_AssetManifest;
    public static EAssetManifest GetAssetManifest()
    {
        if (s_AssetManifest == null)
            s_AssetManifest = AssetDatabase.LoadAssetAtPath<EAssetManifest>(EAssetManifest.s_AssetManifestPath);

        if (s_AssetManifest == null)
        {
            s_AssetManifest = ScriptableObject.CreateInstance<EAssetManifest>();
            AssetDatabase.CreateAsset(s_AssetManifest, EAssetManifest.s_AssetManifestPath);
        }
        return s_AssetManifest;
    }


    public static void RefreshAll()
    {
        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

        EAssetManifest assetManifest = GetAssetManifest();
        assetManifest.Clear();
        string[] allfile = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);
        int spos = Application.dataPath.Length - 6;
        foreach (var item in allfile)
        {
            string path = item.Substring(spos).Replace("\\", "/");
            AddAsset(path);
        }
        sw.Stop();
        EditorUtility.SetDirty(assetManifest);
        AssetDatabase.SaveAssets();
        Debug.Log("RefreshAll time: " + sw.Elapsed.TotalSeconds);
    }


    static void OnWillCreateAsset(string assetPath)
    {
        //Log("OnWillCreateAsset :" + assetPath);
        AddAsset(assetPath);
    }

    static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
    {
        //Log("OnWillDeleteAsset :" + assetPath + "    " + options);
        DeleteAsset(assetPath);
        return AssetDeleteResult.DidNotDelete;
    }

    static AssetMoveResult OnWillMoveAsset(string sourcePath, string targetPath)
    {
        MoveAsset(sourcePath, targetPath);
        //Log("OnWillMoveAsset :" + sourcePath + "    " + targetPath);
        return AssetMoveResult.DidNotMove;
    }

    static string[] OnWillSaveAssets(string[] paths)
    {

        //foreach (string path in paths)
        //{
        //    AddAsset(path);
        //    Log("OnWillSaveAssets :" + path);
        //}
        return paths;
    }

    public static void AddAsset(string assetPath)
    {
        EAssetManifest manifest = GetAssetManifest();
        if (!IsIgnore(assetPath,manifest))
            manifest.AddAsset(assetPath);
    }

    public static void DeleteAsset(string assetPath)
    {
       GetAssetManifest().DeleteAsset(assetPath);
    }


    public static void MoveAsset(string sourcePath,string targetPath)
    {
        EAssetManifest manifest = GetAssetManifest();
        if (!IsIgnore(targetPath, manifest))
            manifest.AddAsset(targetPath);
        else
            manifest.DeleteAsset(sourcePath);
    }


    static void Log(string str)
    {
        if (s_LogEnable)
             Debug.Log(str);
    }

    static bool IsIgnore(string assetPath, EAssetManifest manifest)
    {
        foreach (var item in manifest.MatchingFolder)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            if (assetPath.StartsWith(item))
            {

                string ext = Path.GetExtension(assetPath);
                if (string.IsNullOrEmpty(ext))
                    return true;

                ext = Path.GetExtension(assetPath).Substring(1);
                foreach (var iext in manifest.IgnoreFileType)
                {
                    if (ext == iext)
                        return true;
                }
                return false;
            }
        }
        return true;
    }
}