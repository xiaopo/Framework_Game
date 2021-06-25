using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ModelImportSettings : AssetPostprocessor
{

    void OnPreprocessModel()
    {
        //ModelImporter modelImporter = (ModelImporter)assetImporter;
        //if (modelImporter.importMaterials)

  
        //modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
        //modelImporter.importAnimation = false;
        //modelImporter.SaveAndReimport();
    }

    static Dictionary<string, string> m_WaitUpdate;
    void OnPostprocessModel(GameObject gameObject)
    {

        if (!assetPath.Contains("Anim"))
            return;

        AnimationClip clip = new AnimationClip();
        AnimationClip nClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
        if (nClip != null)
        {
            EditorUtility.CopySerialized(nClip, clip);
            string path = Path.GetDirectoryName(assetPath);
            path = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath) + ".anim");
            string tempPath = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath) + "_temp.anim");
            string fullPath = Path.Combine(Application.dataPath, path.Substring(7));
            string tempFullPath = Path.Combine(Application.dataPath, tempPath.Substring(7));

            if (File.Exists(fullPath))
            {
                AssetDatabase.CreateAsset(clip, tempPath);
                File.Delete(fullPath);
                FileUtil.MoveFileOrDirectory(tempFullPath, fullPath);
            }
            else
            {
                AssetDatabase.CreateAsset(clip, path);
            }

            if (m_WaitUpdate == null)
            {
                m_WaitUpdate = new Dictionary<string, string>();
                EditorApplication.delayCall += OnDelayCall;
            }

            if (!m_WaitUpdate.ContainsKey(assetPath))
                m_WaitUpdate.Add(assetPath, assetPath);
        }
    }

    private void OnDelayCall()
    {
        string fileName = "";
        string[] tts = assetPath.Split('/');
        fileName = tts[tts.Length - 3];
        string floatName = "";
        for (int i = 0; i < tts.Length - 1; i++)
        {
            floatName += tts[i] + '/';
        }

        if (!floatName.Contains("Players"))
        {
            AnimationPack pack = AnimationPack.CreateInstance<AnimationPack>();
            AssetsMenuDefinition.ExExecuteBuildAnimRes(floatName, pack);
            AssetDatabase.CreateAsset(pack, floatName + "/" + fileName + "_Anims.asset");
        }

        foreach (var item in m_WaitUpdate)
        {
            AssetDatabase.DeleteAsset(item.Key);
        }

        m_WaitUpdate.Clear();
        m_WaitUpdate = null;

        AssetDatabase.SaveAssets();
        EditorApplication.delayCall -= OnDelayCall;
    }


}
