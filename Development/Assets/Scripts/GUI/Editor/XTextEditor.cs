using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using XGUI;

[CustomEditor(typeof(XText), true)]
[CanEditMultipleObjects]

public class XTextEditor : TextEditor {

    SerializedProperty m_IsStatic;
    SerializedProperty m_LanguageId;
    SerializedProperty m_IsCanSortingMask;


    protected override void OnEnable()
    {
        base.OnEnable();
        m_IsStatic = serializedObject.FindProperty("isStatic");
        m_LanguageId = serializedObject.FindProperty("languageId");
        m_IsCanSortingMask = serializedObject.FindProperty("m_IsCanSortingMask");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_IsStatic);
        bool isStatic = m_IsStatic.boolValue;
        if (isStatic)
        {
            EditorGUILayout.PropertyField(m_LanguageId);
        }
        EditorGUILayout.PropertyField(m_IsCanSortingMask);
        serializedObject.ApplyModifiedProperties();
    }
}
