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
            GUILayout.Label("���������IP��", GUILayout.Width(nameWidth));
            ipStr = EditorGUILayout.TextField(ipStr, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("ƽ̨���ã�", GUILayout.Width(nameWidth));
            BuildDefinition.target = (BuildTarget)EditorGUILayout.EnumPopup(BuildDefinition.target, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("������ã�", GUILayout.Width(nameWidth));
            BuildDefinition.option = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(BuildDefinition.option, GUILayout.Width(uiWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("BuildLua", "���lua����")))
            {
                BuildLua.Building();
            }
            if (GUILayout.Button(new GUIContent("BuildDevelopment", "�����������")))
            {
                BuildDevelopment.Building();
            }
            if (GUILayout.Button(new GUIContent("BuildArt", "�����������")))
            {
                BuildArt.Building();
            }
            if (GUILayout.Button(new GUIContent("�ϴ���������")))
            {
                BuildInterior.CopyDeployCommond();
            }
            GUILayout.EndHorizontal();
        }
    }

}
