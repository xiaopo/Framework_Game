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

    [MenuItem("GameObject/XScene", priority = -1)]
    static public void AddXEnvironment3D(MenuCommand menuCommand)
    {
        new GameObject("--------Lights");
        new GameObject("--------Floors");
        new GameObject("--------Walls");
        new GameObject("--------SkillBoxs");
        new GameObject("--------Objects");
        new GameObject("--------Effects");
    }

    [MenuItem("Assets/Packet Animations")]
    static void CreateAnimPack()
    {
        string[] objs = Selection.assetGUIDs;
        if (objs.Length > 0)
        {
            string floadPath = "";
            string fileName = "";
            AnimationPack pack = AnimationPack.CreateInstance<AnimationPack>();
            for (int i = 0; i < objs.Length; i++)
            {
                floadPath = AssetDatabase.GUIDToAssetPath(objs[i]);
                if (fileName == "")
                {
                    string[] tts = floadPath.Split('/');
                    fileName = tts[tts.Length - 2];
                }

                ExExecuteBuildAnimRes(floadPath, pack);

            }

            AssetDatabase.CreateAsset(pack, floadPath + "/" + fileName + "_Anims.asset");
        }

    }

    public static void ExExecuteBuildAnimRes(string floadPath, AnimationPack pack)
    {

        if (!floadPath.Contains("Anim")) return;
        string[] files = System.IO.Directory.GetFiles(floadPath);
        for (int j = 0; j < files.Length; j++)
        {
            if (!files[j].Contains(".anim")) continue;
            AnimationClip clip = AssetDatabase.LoadAssetAtPath(files[j], typeof(AnimationClip)) as AnimationClip;
            if (clip != null)
            {

                pack.AddClip(clip.name, clip);
            }
        }
    }
}