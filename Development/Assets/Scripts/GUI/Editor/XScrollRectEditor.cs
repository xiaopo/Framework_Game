using UnityEngine;
using UnityEditor;
using XGUI;
using UnityEditor.UI;

[CustomEditor(typeof(XScrollRect),true)]
public class XScrollRectEditor : ScrollRectEditor
{
    SerializedProperty m_UpArrow;
    SerializedProperty m_DownArrow;
    SerializedProperty m_Horizontal;
    SerializedProperty m_Vertical;
    SerializedProperty m_LeftArrow;
    SerializedProperty m_RightArrow;
    SerializedProperty m_VerticalSlider;
    SerializedProperty UpLimit;
    SerializedProperty DownLimit;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Horizontal = serializedObject.FindProperty("m_Horizontal");
        m_Vertical = serializedObject.FindProperty("m_Vertical");

        m_UpArrow = serializedObject.FindProperty("m_UpArrow");
        m_DownArrow = serializedObject.FindProperty("m_DownArrow");

        m_LeftArrow = serializedObject.FindProperty("m_LeftArrow");
        m_RightArrow = serializedObject.FindProperty("m_RightArrow");

        m_VerticalSlider = serializedObject.FindProperty("m_VerticalSlider");

        UpLimit = serializedObject.FindProperty("UpLimit");
        DownLimit = serializedObject.FindProperty("DownLimit");

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (m_Horizontal.boolValue)
        {
            EditorGUILayout.PropertyField(m_LeftArrow);
            EditorGUILayout.PropertyField(m_RightArrow);
        }

        if (m_Vertical.boolValue)
        {
            EditorGUILayout.PropertyField(m_UpArrow);
            EditorGUILayout.PropertyField(m_DownArrow);
        }

        EditorGUILayout.PropertyField(UpLimit);
        EditorGUILayout.PropertyField(DownLimit);
        EditorGUILayout.PropertyField(m_VerticalSlider);

        serializedObject.ApplyModifiedProperties();
    }
}