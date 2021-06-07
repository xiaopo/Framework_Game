using UnityEngine;
using UnityEditor;
using System.Collections;
using XGUI;
using UnityEditor.UI;

[CustomEditor(typeof(XButton), true)]
[CanEditMultipleObjects]
public class XButtonEditor : ButtonEditor
{
    SerializedProperty m_LabelText;
    SerializedProperty m_SelectedGraphic;
    SerializedProperty m_SelectedGameObject;
    SerializedProperty m_NormalGameObject;
    SerializedProperty m_IsSelected;
    SerializedProperty m_Group;
    SerializedProperty onSelect;
    SerializedProperty m_IsHasCD;
    SerializedProperty m_CD;                   


    protected override void OnEnable()
    {
        base.OnEnable();

        m_LabelText = serializedObject.FindProperty("m_LabelText");
        m_SelectedGraphic = serializedObject.FindProperty("m_SelectedGraphic");
        m_SelectedGameObject = serializedObject.FindProperty("m_SelectedGameObject");
        m_NormalGameObject = serializedObject.FindProperty("m_NormalGameObject");

        m_IsSelected = serializedObject.FindProperty("m_IsSelected");
        m_Group = serializedObject.FindProperty("m_Group");
        onSelect = serializedObject.FindProperty("onSelect");
        m_IsHasCD = serializedObject.FindProperty("m_IsHasCD");
        m_CD = serializedObject.FindProperty("m_CDSecond");                      

    }

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_LabelText);
        EditorGUILayout.PropertyField(m_IsSelected);
        EditorGUILayout.PropertyField(m_SelectedGraphic);
        EditorGUILayout.PropertyField(m_SelectedGameObject);
        EditorGUILayout.PropertyField(m_NormalGameObject);
        EditorGUILayout.PropertyField(m_Group);
        EditorGUILayout.PropertyField(m_IsHasCD);          

        bool isHasCD = m_IsHasCD.boolValue;
        if (isHasCD)
        {
            EditorGUILayout.PropertyField(m_CD);
        }
        EditorGUILayout.PropertyField(onSelect);
        serializedObject.ApplyModifiedProperties();
    }
}
