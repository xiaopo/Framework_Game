using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetsMenuDefinition 
{
    [MenuItem("Assets/Refresh AssetsManifest")]
    static void RefreshAssetsManifest()
    {
        AssetManifestProcessor.RefreshAll();
    }


}