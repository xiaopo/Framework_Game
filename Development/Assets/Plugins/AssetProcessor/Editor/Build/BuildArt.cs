using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class BuildArt
{
    static string s_tempFolder = "Assets/X_Building_Packages";
    private static string s_PackagePath = Application.dataPath + "/../Packages/";

    private static string copyFrom = Application.dataPath + "/Plugins/AssetProcessor";
    public static string unityPath = @"C:\Program Files\Unity\Hub\Editor\2020.3.7f1c1\Editor\Unity.exe";
   
    static string logpath = Application.dataPath + "/BuildArt.log";

    /// <summary>
    /// 在开发工程中调用，或者通过命令行调用
    /// </summary>
    public static void BuildingArt()
    {
        string copyTo = copyFrom.Replace("Development", "ArtProject");
        FileUtil.DeleteFileOrDirectory(copyTo);
        FileUtil.CopyFileOrDirectory(copyFrom, copyTo);

        string projectPath = Application.dataPath.Replace("Development", "ArtProject");

        LCommandlineUtil.StartCat("BuildArt.bat", projectPath + "/../build_cmd/", "");

    }

#if _ArtPro
    [MenuItem("Build/Step/Build-Art", false, 3)]
#endif
    public static void Building()
    {
        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        string basePath = "Assets/Art/{0}";
        CollectShader(list, string.Format(basePath, "Shader"));
        CollectPakages(list, "Packages");
        CollectCharacter(list, string.Format(basePath, "Character"));
        CollectScene(list, string.Format(basePath, "Scene"));

        if (list.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "没有可打包的资源","退出");
            return;
        }

        string outPath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString(),"002");
        if (Directory.Exists(outPath)) Directory.Delete(outPath, true);

        Directory.CreateDirectory(outPath);


        string version = BuildUtil.GetBuildVersion();
        BuildUtil.BuildAssetBundles(outPath,list.ToArray(), BuildDefinition.option, BuildDefinition.target, version);


        if (AssetDatabase.IsValidFolder(s_tempFolder))
            AssetDatabase.DeleteAsset(s_tempFolder);

        AssetDatabase.Refresh();
    }

    private static void CollectShader(List<AssetBundleBuild> list, string projectPath)
    {
        BuildUtil.CollectBundles(list, projectPath, true,true);
    }
    private static void CollectScene(List<AssetBundleBuild> list, string projectPath)
    {
    
        string fullPath = BuildUtil.ProjToFullPath(projectPath);
        string[] folders = Directory.GetDirectories(fullPath);
        foreach (var folder in folders)
        {
            // BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(folder), true);

            //第一层 各个文件一个包
            BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(folder), false,false);

            //子文件夹各自 单独一个包
            string[] children = Directory.GetDirectories(folder);

            foreach(var child in children)
            {
                BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(child), true, true);

            }

        }
    }

    /// <summary>
    /// 收集角色模型信息
    /// </summary>
    /// <param name="list"></param>
    /// <param name="projectPath"></param>
    private static void CollectCharacter(List<AssetBundleBuild> list,string projectPath)
    {
        string fullPath = BuildUtil.ProjToFullPath(projectPath);
        string[] folders = Directory.GetDirectories(fullPath);

        foreach(var folder in folders)
        {
            string folderName = Path.GetFileName(folder);

            switch(folderName)
            {
                case "Controller":
                    BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(folder),false);
                break;
                default: 
                    //子文件夹一个包
                    string[] children = Directory.GetDirectories(folder);
                    foreach(var item in children)
                    {
                        BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(item),true);
                    }
                    break;
            }
        }
    }

    private static bool Filter(string str)
    {
        return str.Contains("Editor") || str.Contains("Documentation") || str.Contains("Tests");
    }

    public static void CollectPakages(List<AssetBundleBuild> list, string buildPath)
    {
        string basePath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        string fullPath = Path.Combine(basePath, buildPath);
        if (!Directory.Exists(fullPath)) return;
        string[] folders = Directory.GetDirectories(fullPath);
        foreach (var folder in folders)
        {
            if (Filter(folder)) continue;

            string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);//获得全部子文件
            if (files.Length < 1) return;


            AssetBundleBuild ab = new AssetBundleBuild();
            ab.assetBundleName = BuildUtil.TripPackageDirectory(folder) + ".asset";
            string[] addressableNames = new string[0];
            string[] assetNames = new string[0];
            foreach (var file in files)
            {
                if (Filter(file)) continue;

                string clearlyFile = BuildUtil.TripPackageFile(file);
                if (BuildUtil.Filter(clearlyFile)) continue;

                ArrayUtility.Add<string>(ref addressableNames, Path.GetFileName(clearlyFile));//加载资源时，使用的名字
                ArrayUtility.Add<string>(ref assetNames, BuildUtil.FullToPorjPath(clearlyFile));//项目中的路径

            }

            ab.addressableNames = addressableNames;
            ab.assetNames = assetNames;

            list.Add(ab);
        }

    }




}
