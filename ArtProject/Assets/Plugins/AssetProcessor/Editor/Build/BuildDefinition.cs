
using System.Collections.Generic;
using UnityEditor;


public class BuildDefinition 
{
    ////�����ļ������Բ��ڴ�����
    public static Dictionary<string, XAssetsFiles.FileOptions> DefaultOptions = new Dictionary<string, XAssetsFiles.FileOptions>()
    {
        {"000/00000000000000000000000000000000.asset",XAssetsFiles.FileOptions.LUA | XAssetsFiles.FileOptions.LAUNCHDOWNLOAD},
        {"001/fonts.asset", XAssetsFiles.FileOptions.LAUNCHDOWNLOAD}
    };

    public const string c_VersionFileName = "version.txt";
    public const string c_FileListFileName = "files.txt";
    public const string c_SettingFileName = "files.setting";
    public const string s_XassetPath = "Assets/XAssetManifest.asset";
    public const string s_assetManifest = "assetManifest";


    //public const string outBasePath = "D:\\ESSamp\\wwwroot\\assetbuild_tt";
    //public const string outBasePath = "D:\\assetBundles";
    public static BuildAssetBundleOptions option = BuildAssetBundleOptions.ChunkBasedCompression;

    //ע�ⲻͬƽ̨����Ҫ����
    public static BuildTarget target = BuildTarget.Android;

    //�������·��
    public static string OutBasePath = "D:\\assetBundles";
}
