using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class  TextureImportSettings : AssetPostprocessor
{
    private static Dictionary<string, string> m_WaitUpdate;

    public static Dictionary<string, string> WaitUpdate
    {
        get
        {
            if (m_WaitUpdate == null) m_WaitUpdate = new Dictionary<string, string>();

            return m_WaitUpdate;
        }
    }

    public static bool m_addCall = false;
    /// <summary>
    /// 在纹理刚完成导入之前获取通知,此时，选择压缩格式已为时已晚
    /// </summary>
    /// <param name="texture"></param>
    void OnPostprocessTexture(Texture2D texture)
    {

        TextureImporter importer = assetImporter as TextureImporter;
        if (assetPath.Contains("Assets/GUI"))
            TextureImportSettingsAtlas.AtlasImportHandler(importer, assetPath);//UI图片 打图集

    }

    //纹理导入器运行之前获取通知
    ////修改图片的压缩格式必须放在 OnPreprocessTexture 方法中执行才能有效
    void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;

        if (assetPath.Contains("Assets/GUI"))
            TextureImportSettingsAtlas.AtlasImageCompress(importer, assetPath);//压缩图片
        else if (assetPath.Contains("Assets/Art"))
        {
            //美术资源图片
            SetTexurePlatformSetting(importer, assetPath);
        }
    }


    

    private static void OnDelayCall()
    {
        EditorApplication.delayCall -= OnDelayCall;
        m_addCall = false;

        TextureImportSettingsAtlas.DelaySetPlatformSetting();


        if (m_WaitUpdate != null)
        {
            foreach (var item in m_WaitUpdate)
                TextureImportSettingsAtlas.RefreshSpriteAtlas(item.Key, item.Value);
        }

        m_WaitUpdate.Clear();
        m_WaitUpdate = null;

    }



    public static void AddDelayCall()
    {
        if (!m_addCall)
        {
            EditorApplication.delayCall += OnDelayCall;
            m_addCall = true;
        }
    }

   

    public static void SetTexurePlatformSetting(TextureImporter importer, string assetPath, bool isReImport = true)
    {
        TextureImporterPlatformSettings tips = new TextureImporterPlatformSettings();
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        bool isAlpha = tex != null ? tex.alphaIsTransparency : true;
        tips.maxTextureSize = 1024;
        tips.overridden = true;
        tips.format = isAlpha ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.ASTC_4x4;
        tips.name = "Android";
        importer.SetPlatformTextureSettings(tips);

        tips.overridden = true;
        tips.format = isAlpha ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.ASTC_4x4;
        tips.name = "iPhone";
        importer.SetPlatformTextureSettings(tips);
        if (isReImport)
            importer.SaveAndReimport();
    }

}
