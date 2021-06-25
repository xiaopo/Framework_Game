using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class QuickMenuKey : ScriptableObject
{
    static string m_LaunchGameTag = "QuickMenuKey_LaunchGameTag";
    static string m_LaunchGameAssetBundle = "QuickMenuKey_LaunchGameAssetBundle";
    static string m_LaunchGameLuaEditor = "QuickMenuKey_LaunchLuaEditor";
    static QuickMenuKey()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (EditorPrefs.GetBool(m_LaunchGameTag))
            {
                EditorApplication.update += Update;
            }
        }


        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorPrefs.SetBool(m_LaunchGameAssetBundle, false);

            }
        };

    }

    static void Update()
    {
        if (EditorApplication.isPlaying)
        {
            EditorPrefs.DeleteKey(m_LaunchGameTag);
            EditorApplication.update -= Update;
            CreateLaunchScene();
        }
    }

    static void CreateLaunchScene()
    {
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (scene.name == "lanucher")
        {
            return;
        }

        UnityEngine.SceneManagement.SceneManager.CreateScene("LaunchGame");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
        System.Reflection.Assembly Assembly = System.Reflection.Assembly.Load("Assembly-CSharp");
        System.Type type = Assembly.GetType("Launcher");
        GameObject xgame = new GameObject("Launcher", type);


        Object.DontDestroyOnLoad(xgame);
    }

#if _Development
    [MenuItem("launcher/launcher-normal(Editor Lua)")]
    public static void LauncerNormal()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        RefreshAllScene(true);

        EditorApplication.ExecuteMenuItem("Assets/Refresh AssetsManifest");
        EditorPrefs.SetBool(m_LaunchGameAssetBundle, false);
        EditorPrefs.SetBool(m_LaunchGameTag, true);
        EditorPrefs.SetBool(m_LaunchGameLuaEditor, true);
        
        EditorApplication.isPlaying = true;
    }

    [MenuItem("launcher/launcher-assetBundle(Editor Lua)")]
    public static void LauncerAssetBunlde()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        RefreshAllScene(false);

        UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;

        EditorApplication.ExecuteMenuItem("Assets/Refresh AssetsManifest");

        EditorPrefs.SetBool(m_LaunchGameAssetBundle, true);
        EditorPrefs.SetBool(m_LaunchGameTag, true);
        EditorPrefs.SetBool(m_LaunchGameLuaEditor, true);
        EditorApplication.isPlaying = true;
    }
#endif
    private static readonly string scenePath = "/Art/Scene/";
    public static void RefreshAllScene(bool isEditor = true)
    {
        // 设置场景 *.unity 路径
        string path = Application.dataPath + scenePath;

        if (!Directory.Exists(path)) return;


        // 遍历获取目录下所有 .unity 文件
        string[] files = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);

        // 定义 场景数组
        EditorBuildSettingsScene[] scenes = null;
        if (isEditor)
        {
            scenes = new EditorBuildSettingsScene[files.Length + 1];
            scenes[0] = new EditorBuildSettingsScene("Assets/lanucher.unity", true);

            for (int i = 0; i < files.Length; ++i)
            {
                string scenePath = files[i].Replace("\\", "/").Substring(Application.dataPath.Length - 6);
                scenes[i + 1] = new EditorBuildSettingsScene(scenePath, true);

            }
        }
        else
        {
            scenes = new EditorBuildSettingsScene[1]
            {
                new EditorBuildSettingsScene("Assets/lanucher.unity", true),
            };
        }



        // 设置 scene 数组
        EditorBuildSettings.scenes = scenes;
    }
}
