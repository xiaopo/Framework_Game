
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildDevelopment
{
#if _Development
    [MenuItem("Build/Step/Build-Development", false,1)]
#endif
    public static void Building()
    {
        UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnlyAtlas;

        //prefab 单独打
        //Image 打成一个

        List<AssetBundleBuild> list = new List<AssetBundleBuild>();

        CollectConfigs(list, "Assets/Configs");
        string basePath = "Assets/GUI/{0}";
        CollectGUI(list, string.Format(basePath, "Modules"));
        CollectGUI(list, string.Format(basePath, "Fonts"));
        if (list.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "没有可打包的资源", "退出");
            return;
        }

        string outPath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString(), "001");
        if (Directory.Exists(outPath)) Directory.Delete(outPath, true);
        Directory.CreateDirectory(outPath);

        string version = BuildUtil.GetBuildVersion();
        BuildUtil.BuildAssetBundles(outPath, list.ToArray(), BuildDefinition.option, BuildDefinition.target, version);

    }

    private static void CollectConfigs(List<AssetBundleBuild> list, string projectPath)
    {
        string[] folders = Directory.GetDirectories(projectPath);
        foreach (var folder in folders)
        {
            if(folder.Contains("Common"))
            {
                BuildUtil.CollectBundles(list, folder, false, true,7);

            }
          
        }
    }

    public static List<string> gui_filters = new List<string>() { ".spriteatlas" };
    private static void CollectGUI(List<AssetBundleBuild> list, string projectPath)
    {
        if (projectPath.Contains("/Modules"))
        {
            string[] folders = Directory.GetDirectories(projectPath);

            foreach (var module in folders)
            {
                string[] contents = Directory.GetDirectories(module);

                foreach (var item in contents)
                {
                    if (item.Contains("Images"))
                        //只打小图，图集Unity自动搞定
                        BuildUtil.RecursiveCollectBundles(list, item, "_atlas", gui_filters);
                    else if(item.Contains("Single"))
                        BuildUtil.CollectBundles(list, item, false, true);//大图分开打
                    else if (item.Contains("Prefabs"))
                        BuildUtil.CollectBundles(list, item, false, true);//预制体分开打
                  
                }

            }
        }
        else if (projectPath.Contains("/Fonts"))
        {
            BuildUtil.CollectBundles(list, projectPath, true, true);
        }
    }
}
