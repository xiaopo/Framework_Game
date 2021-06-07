using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingAssetCreater
{

    [MenuItem("Setting/Create-Download-Setting")]
    public static void CreateDownloadSetting()
    {
        DownloadSetting msg = ScriptableObject.CreateInstance<DownloadSetting>();
        AssetDatabase.CreateAsset(msg, "Assets/Resources/Settings/DownloadSetting2.asset");
        AssetDatabase.Refresh();
    }

}
