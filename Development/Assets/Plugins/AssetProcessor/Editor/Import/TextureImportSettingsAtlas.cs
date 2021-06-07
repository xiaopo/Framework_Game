using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class TextureImportSettingsAtlas
{
    public struct PlatformSettingCacheData
    {
        public string assetPath;
        public TextureImporter importer;
    }

    private static List<PlatformSettingCacheData> m_WaitSetplatformSetting;

    public static void AtlasImageCompress(TextureImporter import, string assetPath)
    {
        if (!import) return;
        bool reimport = false;
        if (import.textureType != TextureImporterType.Sprite)
        {
            import.textureType = TextureImporterType.Sprite;
            import.crunchedCompression = true;
            reimport = true;
        }

        TextureImporterSettings ts = new TextureImporterSettings();
        import.ReadTextureSettings(ts);
        if (ts.spriteMeshType != SpriteMeshType.FullRect)
        {
            ts.spriteMeshType = SpriteMeshType.FullRect;
            import.SetTextureSettings(ts);
            reimport = true;
        }

        if (reimport) import.SaveAndReimport();

    }


    public static void AtlasImportHandler(TextureImporter import, string assetPath)
    {
        if (!import) return;

        bool isNeedPlatformSetting = true;
        //解析获得 图集的名字
        if (assetPath.Contains("/Images"))// 必须要放在Image文件夹下才可以
        {
            //一帧只自动编辑一次
            string atlasName = GetAtlasAssetName(assetPath, out isNeedPlatformSetting);

            if (!string.IsNullOrEmpty(atlasName))
            {
                if (import.spritePackingTag != atlasName)
                {
                    //修改PackingTag
                    import.spritePackingTag = atlasName;
                    import.SaveAndReimport();
                }

                string atlasPathS = atlasName + ".spriteatlas";

                if (!TextureImportSettings.WaitUpdate.ContainsKey(atlasPathS))
                {
                    TextureImportSettings.WaitUpdate.Add(assetPath, atlasPathS);
                }

                TextureImportSettings.AddDelayCall();
            }

           
        }

        if (isNeedPlatformSetting)
        {
            if (m_WaitSetplatformSetting == null) m_WaitSetplatformSetting = new List<PlatformSettingCacheData>();
            PlatformSettingCacheData cacheData = new PlatformSettingCacheData();
            cacheData.importer = import;
            cacheData.assetPath = assetPath;
            m_WaitSetplatformSetting.Add(cacheData);
        }

    }

    public static string GetAtlasAssetName(string assetPath, out bool isNeedPlatformSetting)
    {
        isNeedPlatformSetting = false;
        string moduleName = assetPath.Substring(19).Split('/')[0];
        string folderName = Path.GetFileName(Path.GetDirectoryName(assetPath));
        string atlasAssetName = "Atlas_" + moduleName;
        if (folderName.Contains("Single"))
            atlasAssetName += "_Single_" + Path.GetFileNameWithoutExtension(assetPath);
        else if (folderName.Contains("Sequence") || folderName.Contains("Split"))
        {
            isNeedPlatformSetting = true;
            atlasAssetName = string.Empty;
        }
        else if (folderName != "Images")
            atlasAssetName += "_" + folderName;

        return atlasAssetName;
    }


    public static SpriteAtlas CreateSpriteAtlas(string assetpath, string atlasName)
    {
        string atlasFolder = Path.GetDirectoryName(assetpath).Replace("\\", "/");

        string atlasAssetPath = Path.Combine(atlasFolder, atlasName);

        AssetDatabase.DeleteAsset(atlasAssetPath);

        //图集属性
        SpriteAtlas atlas = new SpriteAtlas();
        SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
        };
        atlas.SetPackingSettings(packSetting);


        SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        atlas.SetTextureSettings(textureSetting);

        TextureImporterPlatformSettings platformSetting_android = new TextureImporterPlatformSettings()
        {
            maxTextureSize = 1024,
            format = TextureImporterFormat.ASTC_4x4,
            textureCompression = TextureImporterCompression.Compressed,
            name = "Android",
            overridden = true,

        };

        atlas.SetPlatformSettings(platformSetting_android);
        TextureImporterPlatformSettings platformSetting_ios = new TextureImporterPlatformSettings()
        {
            maxTextureSize = 1024,
            format = TextureImporterFormat.ASTC_4x4,
            textureCompression = TextureImporterCompression.Compressed,
            name = "iPhone",
            overridden = true,

        };

        atlas.SetPlatformSettings(platformSetting_ios);

        AssetDatabase.CreateAsset(atlas, atlasAssetPath);

        return atlas;

    }

    public static void RefreshSpriteAtlas(string assetpath, string atlasName)
    {
        string atlasFolder = Path.GetDirectoryName(assetpath).Replace("\\", "/");

        SpriteAtlas atlas = CreateSpriteAtlas(assetpath, atlasName);

        // 1、添加文件
        DirectoryInfo dir = new DirectoryInfo(atlasFolder);
        // 这里我使用的是png图片，已经生成Sprite精灵了
        FileInfo[] files = dir.GetFiles("*.png");
        List<Sprite> s_list = new List<Sprite>();
        foreach (FileInfo file in files)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{atlasFolder}/{file.Name}");
            if (sprite != null)
                s_list.Add(sprite);

        }

        atlas.Add(s_list.ToArray());

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public static void DelaySetPlatformSetting()
    {
        if (m_WaitSetplatformSetting == null) return;

        for (int i = 0; i < m_WaitSetplatformSetting.Count; i++)
        {
            var item = m_WaitSetplatformSetting[i];
            TextureImportSettings.SetTexurePlatformSetting(item.importer, item.assetPath);
        }
    }
}