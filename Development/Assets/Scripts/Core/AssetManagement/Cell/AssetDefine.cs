using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

 public class AssetDefine
{

    public static string[] RemoteDownloadUrls = new string[1] { "http://192.168.0.100/assetbuild_tt/Android/" };
    public static string AssetBundleFolder = "/A_AssetBundles/";
    public static string AssetManifestName = "xassetmanifest";
    public readonly static string dataPath = Application.dataPath;
    public readonly static string persistentDataPath = Application.persistentDataPath;
    public readonly static string streamingAssetsPath = Application.streamingAssetsPath;
    public readonly static string temporaryCachePath = Application.temporaryCachePath;

        //扩展卡资源
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public static string ExternalSDCardsPath = Path.Combine(AssetDefine.dataPath, "../" + AssetDefine.AssetBundleFolder);
#elif UNITY_IOS
        public static string ExternalSDCardsPath = AssetDefine.temporaryCachePath + AssetDefine.AssetBundleFolder;
#else
        public static string ExternalSDCardsPath = AssetDefine.persistentDataPath + AssetDefine.AssetBundleFolder;
#endif



        //首包内资源
#if UNITY_EDITOR
        public static string BuildinAssetPath = AssetDefine.streamingAssetsPath + AssetDefine.AssetBundleFolder;
#elif UNITY_ANDROID
        public static string BuildinAssetPath = AssetDefine.streamingAssetsPath + AssetDefine.AssetBundleFolder;
#else
        public static string BuildinAssetPath = AssetDefine.streamingAssetsPath + AssetDefine.AssetBundleFolder;
#endif

#if UNITY_EDITOR
        public static string DataDataPath = Path.Combine(AssetDefine.dataPath, "../");
#elif UNITY_STANDALONE_WIN
         public static string DataDataPath = AssetDefine.dataPath;
#else
         public static string DataDataPath = "/data/data/" + Application.identifier + "/files/";
#endif

        public static string DllPath = DataDataPath + "Assembly-CSharp.dll";
    }

