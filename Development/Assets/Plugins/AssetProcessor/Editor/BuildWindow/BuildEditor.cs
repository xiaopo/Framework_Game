using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BuildTool
{
    public class BuildEditor : IEditor
    {
        // Start is called before the first frame update

        const float nameWidth = 200;
        const float uiWidth = 200;
        string ipStr;
        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("输入服务器IP：", GUILayout.Width(nameWidth));
            ipStr = EditorGUILayout.TextField(ipStr, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("平台设置：", GUILayout.Width(nameWidth));
            BuildDefinition.target = (BuildTarget)EditorGUILayout.EnumPopup(BuildDefinition.target, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("打包设置：", GUILayout.Width(nameWidth));
            BuildDefinition.option = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(BuildDefinition.option, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("BuildLua", "打包lua代码")))
            {
                BuildLua.Building();
            }
            if (GUILayout.Button(new GUIContent("BuildDevelopment", "打包开发工程")))
            {
                BuildDevelopment.Building();
            }
            if (GUILayout.Button(new GUIContent("BuildArt", "打包美术工程")))
            {
                BuildArt.Building();
            }
            if (GUILayout.Button(new GUIContent("上传到服务器")))
            {
                BuildInterior.CopyDeployCommond();
            }
            GUILayout.EndHorizontal();
        }
    }

}
