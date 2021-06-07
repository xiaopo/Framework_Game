using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildLua 
{
    //注意：LuaLoader 中对应的名字

    static string s_tempFolder = "Assets/X_Building_Lua";
    static string s_outputName = "00000000000000000000000000000000.asset";
 
    private static string s_LuaPath = Application.dataPath + "/../Lua_Scripts/";

#if _Development
    [MenuItem("Build/Step/Build-Lua", false, 0)]
#endif
    public static void Building()
    {
        UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.Disabled;

        string version = BuildUtil.GetBuildVersion(s_LuaPath);

        AssetBundleBuild lua = CollectionLuaScripts();

        List<AssetBundleBuild> list = new List<AssetBundleBuild>() { lua };

        string outPath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString(), "000");
        if (Directory.Exists(outPath)) Directory.Delete(outPath, true);

        Directory.CreateDirectory(outPath);

        BuildUtil.BuildAssetBundles(outPath, list.ToArray(), BuildDefinition.option, BuildDefinition.target, version);


        if (AssetDatabase.IsValidFolder(s_tempFolder))
            AssetDatabase.DeleteAsset(s_tempFolder);

        AssetDatabase.Refresh();


    }

    static AssetBundleBuild CollectionLuaScripts()
    {
        if (AssetDatabase.IsValidFolder(s_tempFolder))
            AssetDatabase.DeleteAsset(s_tempFolder);

        string dataPath = Application.dataPath;
        string projectFolder = dataPath;
        projectFolder = projectFolder.Substring(0, projectFolder.Length - 6);
        projectFolder = Path.Combine(projectFolder, s_tempFolder);

        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetBundleName = s_outputName;
        abb.assetNames = new string[0];
        abb.addressableNames = new string[0];

        string[] allLuas = Directory.GetFiles(s_LuaPath, "*.lua", SearchOption.AllDirectories);
        int sidx = s_LuaPath.Length;
        int psidx = dataPath.Length - 6;
        foreach (var file in allLuas)
        {
            //将文件从lua工程目录拷贝到Unity工程中进行打包
            string relative = file.Substring(sidx);
            string projectPath = Path.Combine(projectFolder, relative);
            projectPath = projectPath.Replace(Path.GetExtension(projectPath), ".txt");
            string dirPath = Path.GetDirectoryName(projectPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            FileUtil.CopyFileOrDirectory(file, projectPath);

            string assetPath = projectPath.Substring(psidx).Replace("\\", "/");
            string addressPath = assetPath.Substring(s_tempFolder.Length + 1).Replace("/", ".");

            ArrayUtility.Add<string>(ref abb.assetNames, assetPath);
            ArrayUtility.Add<string>(ref abb.addressableNames, addressPath);
        }


        AssetDatabase.Refresh();

        return abb;
    }
}
