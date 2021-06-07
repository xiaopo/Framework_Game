
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildInterior
{
    #region xxxxxx..............û����
    // [MenuItem("Build/Step/Build-Interior", false, 3)]
    public static void Building()
    {
        //һ���ļ�һ����

        List<AssetBundleBuild> list = new List<AssetBundleBuild>();

        BuildArt.CollectPakages(list, "Packages");
        if (list.Count == 0)
        {
            EditorUtility.DisplayDialog("��ʾ", "û�пɴ������Դ", "�˳�");
            return;
        }

        string outPath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString(), "003");
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
        string version = BuildUtil.GetBuildVersion();
        BuildUtil.BuildAssetBundles(outPath, list.ToArray(), BuildDefinition.option, BuildDefinition.target, version);


    }

    private static bool Filter(string str)
    {
        return str.Contains("Editor") || str.Contains("Documentation") || str.Contains("Tests");
    }

   
    private static void CollectInterior(List<AssetBundleBuild> list, string projectPath,int offset)
    {
        string fullPath = BuildUtil.ProjToFullPath(projectPath);
        string[] folders = Directory.GetDirectories(fullPath);
        foreach (var folder in folders)
        {
            
            BuildUtil.CollectBundles(list, BuildUtil.FullToPorjPath(folder), true,true, offset);
        }

    }

    #endregion

    [MenuItem("Build/Step/Build-HotFixFile")]
    public static void BuildHotFixFile()
    {
        BuildTarget target = BuildDefinition.target;
        string outPath = Path.Combine(BuildDefinition.OutBasePath, target.ToString());

        string[] addressableNames = new string[0] { };
        string[] assetNames = new string[0] { };
        BuildUtil.CombineManifest(ref addressableNames, ref assetNames, true);

        BuildUtil.CombineFileList(outPath);

        BuildUtil.CombineVersionFile(outPath);

        List<string> need_clear = new List<string>();
        //���û�õ��ļ�
        string [] files = Directory.GetFiles(outPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            if (file.Contains(".manifest")) need_clear.Add(file);
        }

        string xxx = Path.Combine(outPath, target.ToString());
        need_clear.Add(xxx);

        foreach (var item in need_clear)
        {
            File.Delete(item);
            if (File.Exists(item + ".meta"))
                File.Delete(item + ".meta");
        }

    }

    #region xxxxxx..............û����
    public static void BuildHotFixFileBig()
    {
        //�����ȸ����ļ�

        BuildTarget target = BuildDefinition.target;
        string outPath = Path.Combine(BuildDefinition.OutBasePath, target.ToString());

       
        string[] addressableNames = new string[0] { };
        string[] assetNames = new string[0] { };


        BuildUtil.CombineManifest(ref addressableNames, ref assetNames, false);

        BuildUtil.CombineFileList(outPath);

        BuildUtil.CombineVersionFile(outPath);

        //����ļ����һ��assetBundle
        string[] files = Directory.GetFiles(outPath, "*", SearchOption.TopDirectoryOnly);
        List<string> needPackes = new List<string>(3) { BuildDefinition.c_FileListFileName, BuildDefinition.c_VersionFileName };

        List<string> need_clear = new List<string>();
        foreach(var file in files)
        {
            string item = Path.GetFileName(file);
            if (needPackes.Contains(item))
            {
                string projectFullPath = Path.Combine(Application.dataPath, item);
                string projectPath = BuildUtil.FullToPorjPath(projectFullPath);
      
                FileUtil.CopyFileOrDirectory(file, projectFullPath);
                need_clear.Add(projectFullPath);
    
                ArrayUtility.Add<string>(ref addressableNames, item);
                ArrayUtility.Add<string>(ref assetNames, projectPath);
            }

        }

        AssetDatabase.Refresh();

        //��ȫ���ļ�ѹ����һ���ļ���������
        AssetBundleBuild abbuild = new AssetBundleBuild();
        abbuild.assetBundleName = BuildDefinition.s_assetManifest;
        abbuild.addressableNames = addressableNames;
        abbuild.assetNames = assetNames;
        ////�嵥�ļ�����ѹ��
        BuildAssetBundleOptions options = BuildAssetBundleOptions.None | BuildAssetBundleOptions.ForceRebuildAssetBundle;
        BuildPipeline.BuildAssetBundles(outPath, new AssetBundleBuild[1]{ abbuild }, options, target);

        //���û�õ��ļ�
        files = Directory.GetFiles(outPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            if (file.Contains(".manifest")) need_clear.Add(file);
        }

        string xxx = Path.Combine(outPath, target.ToString());
        need_clear.Add(xxx);

        foreach (var item in need_clear)
        {
            File.Delete(item);
            if (File.Exists(item + ".meta"))
                File.Delete(item + ".meta");
        }

        AssetDatabase.DeleteAsset(BuildDefinition.s_XassetPath);


        AssetDatabase.Refresh();
    }
    #endregion


    [MenuItem("Build/Pipeline/Build-Pipeline")]
    public static void BuildAll()
    {
       
#if _Development
        BuildDevelopment.Building();
        BuildLua.Building();
#elif _ArtPro
       BuildArt.Building();
#endif

      BuildInterior.BuildHotFixFile();

        //ִ��copy
      BuildInterior.CopyDeployCommond();

    }


#if _Development
    [MenuItem("Build/Pipeline/Build-All-Pro")]
    public static void BuildAllPipeline()
    {

        BuildDevelopment.Building();
        BuildLua.Building();
        BuildArt.Building();

        BuildInterior.BuildHotFixFile();

        //ִ��copy
        BuildInterior.CopyDeployCommond();

    }
#endif

    [MenuItem("Build/Copy/Copy to Server")]
    public static void CopyDeployCommond()
    {

        string path = Application.dataPath + "/../build_cmd/";
        string sourcePath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString());
        string httpPath = "D:\\nginx\\html\\assetBundles_tt\\android";
        // string httpPath2 = "D:\\nginx\\html\\assetBundles_tt2\\android";
        //������ http������
        if (File.Exists(sourcePath))
            File.Delete(sourcePath);

        LCommandlineUtil.StartCat("copytohttp.bat", path, sourcePath + " " + httpPath);
        //LCommandlineUtil.StartCat("copytohttp.bat", path, sourcePath + " " + httpPath2);

    }


    [MenuItem("Build/Copy/Copy to StreamingAssets")]
    public static void CopyStreamingAssets()
    {

        string path = Application.dataPath + "/../build_cmd/";
        string sourcePath = Path.Combine(BuildDefinition.OutBasePath, BuildDefinition.target.ToString());
        string streamingPath = Application.dataPath + "/StreamingAssets/A_AssetBundles";
        streamingPath = streamingPath.Replace("/", "\\");


        if (File.Exists(streamingPath))
            File.Delete(streamingPath);

        //������ StreamingAssets
        LCommandlineUtil.StartCat("copytoStreamingAssets.bat", path, sourcePath + " " + streamingPath);
    }


}
