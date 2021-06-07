using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

using System.Collections;

public class DeployProjectSettings
{
    static string[] tags = { "MapCamera", "GUICamera", "SkipGate", "SceneTrigger" };
    static string[] layers = { "UIHide", "UIModel","Wall", "Floor", "Entity","FightEffect", "Eff_Hig", "Eff_Mid", "Eff_Low", "Eff_SLow", "EtTrigger", "ShadowsCast", "ShadowsReceive" ,"TreeModel", "SkyBox", "EntityHide"};

    static ArrayList navigation_areas = new ArrayList()
    {
        "Walkable",1f,
        "Not Walkable",1f,
        "Jump",2f,
        "Shoal",1f,
        "Water",1f,
        "Roof",1f,
        "Mining",1f,
        "Grass",1f,
        "Desert",2f,
        "Swamp",2f,
    };

    static string[] sortingLayers;

    [InitializeOnLoadMethod]
    public static void CheckProjectSettings()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        CheckLayer();
        CheckTag();
        CheckSortingLayers();
        //UpdateUnitEditor();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Assets/Refresh ProjectSettings")]
    public static void RefreshProjectSettings()
    {
        CheckProjectSettings();
    }


    static void CheckLayer()
    {
        if (layers == null)
            return;

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty iter = tagManager.GetIterator();
        while (iter.NextVisible(true))
        {
            if (iter.name == "layers")
            {
                SerializedProperty layer;
                for (int i = 8; i < iter.arraySize; i++)
                {
                    layer = iter.GetArrayElementAtIndex(i);
                    layer.stringValue = string.Empty;
                }


                for (int i = 0; i < layers.Length; i++)
                {
                    layer = iter.GetArrayElementAtIndex(8 + i);
                    layer.stringValue = layers[i];
                }

                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }

    static void CheckTag()
    {
        if (tags == null)
            return;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty iter = tagManager.GetIterator();
        while (iter.NextVisible(true))
        {
            if (iter.name == "tags")
            {
                iter.ClearArray();

                for (int i = 0; i < tags.Length; i++)
                {
                    iter.InsertArrayElementAtIndex(i);
                    SerializedProperty tag = iter.GetArrayElementAtIndex(i);
                    tag.stringValue = tags[i];
                }
                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }


    static void CheckSortingLayers()
    {
        if (sortingLayers == null)
            return;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty iter = tagManager.GetIterator();
        while (iter.NextVisible(true))
        {
            if (iter.name == "m_SortingLayers")
            {

                for (int i = iter.arraySize - 1; i >= 1; i--)
                    iter.DeleteArrayElementAtIndex(i);



                for (int i = 0; i < sortingLayers.Length; i++)
                {
                    iter.InsertArrayElementAtIndex(i + 1);
                    SerializedProperty layer = iter.GetArrayElementAtIndex(i + 1);
                    layer.FindPropertyRelative("name").stringValue = sortingLayers[i];
                }

                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }


    static void UpdateUnitEditor()
    {
        //if (UnityEditor.EditorSettings.spritePackerMode != SpritePackerMode.BuildTimeOnlyAtlas)
        //    UnityEditor.EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnlyAtlas;


        //GraphicsSettings
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
        serializedObject.Update();
        SerializedProperty m_LightmapStripping = serializedObject.FindProperty("m_LightmapStripping");
        m_LightmapStripping.intValue = 1;
        SerializedProperty m_LightmapKeepPlain = serializedObject.FindProperty("m_LightmapKeepPlain");
        m_LightmapKeepPlain.boolValue = true;
        SerializedProperty m_LightmapKeepDirCombined = serializedObject.FindProperty("m_LightmapKeepDirCombined");
        m_LightmapKeepDirCombined.boolValue = true;
        SerializedProperty m_LightmapKeepDynamicPlain = serializedObject.FindProperty("m_LightmapKeepDynamicPlain");
        m_LightmapKeepDynamicPlain.boolValue = false;
        SerializedProperty m_LightmapKeepDynamicDirCombined = serializedObject.FindProperty("m_LightmapKeepDynamicDirCombined");
        m_LightmapKeepDynamicDirCombined.boolValue = false;
        SerializedProperty m_LightmapKeepShadowMask = serializedObject.FindProperty("m_LightmapKeepShadowMask");
        m_LightmapKeepShadowMask.boolValue = true;
        SerializedProperty m_LightmapKeepSubtractive = serializedObject.FindProperty("m_LightmapKeepSubtractive");
        m_LightmapKeepSubtractive.boolValue = false;
        SerializedProperty m_FogStripping = serializedObject.FindProperty("m_FogStripping");
        m_FogStripping.intValue = 1;
        SerializedProperty m_FogKeepLinear = serializedObject.FindProperty("m_FogKeepLinear");
        m_FogKeepLinear.boolValue = true;
        SerializedProperty m_FogKeepExp = serializedObject.FindProperty("m_FogKeepExp");
        m_FogKeepExp.boolValue = false;
        SerializedProperty m_FogKeepExp2 = serializedObject.FindProperty("m_FogKeepExp2");
        m_FogKeepExp2.boolValue = false;
        serializedObject.ApplyModifiedProperties();


        QualitySettings.skinWeights = SkinWeights.TwoBones;

        //NavMeshAreas
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/NavMeshAreas.asset")[0]);
        serializedObject.Update();
        SerializedProperty m_Areas = serializedObject.FindProperty("areas");
        for (int i = 0; i < m_Areas.arraySize; i++)
        {
            int sidx = i * 2;

            if (sidx >= navigation_areas.Count || sidx + 1 >= navigation_areas.Count)
                break;
            SerializedProperty areasData = m_Areas.GetArrayElementAtIndex(i);
            SerializedProperty name = areasData.FindPropertyRelative("name");
            SerializedProperty cost = areasData.FindPropertyRelative("cost");
            if (name.stringValue == "NotWalkable")
                continue;

            if (i > 2)
                name.stringValue = (string)navigation_areas[sidx];

            cost.floatValue = (float)navigation_areas[sidx + 1];
        }
        serializedObject.ApplyModifiedProperties();

        //UnityEditor.PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        //UnityEditor.PlayerSettings.defaultScreenWidth = 720;
        //UnityEditor.PlayerSettings.defaultScreenHeight = 1280;
        //UnityEditor.PlayerSettings.resizableWindow = true;

        //UnityEditor.PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows, new GraphicsDeviceType[] { GraphicsDeviceType.OpenGLES3 });

        //UnityEditor.PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new GraphicsDeviceType[] { GraphicsDeviceType.OpenGLES3 });
        //UnityEditor.PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android,true);
        //UnityEditor.PlayerSettings.bakeCollisionMeshes = true;
    }
}