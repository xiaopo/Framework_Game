using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using XGUI;

[CustomEditor(typeof(XProgress), true)]
[CanEditMultipleObjects]

public class XProgressEditor : TextEditor
{

    SerializedProperty m_maxValue;
    SerializedProperty m_minValue;
    SerializedProperty m_label;
    SerializedProperty m_EaseType;
    SerializedProperty m_style;
    SerializedProperty m_direction;
    SerializedProperty m_value;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_maxValue = serializedObject.FindProperty("maxValue");
        m_minValue = serializedObject.FindProperty("minValue");
        m_label = serializedObject.FindProperty("label");
        m_EaseType = serializedObject.FindProperty("processImg");
        m_style = serializedObject.FindProperty("style");
        m_direction = serializedObject.FindProperty("direction");
        m_value = serializedObject.FindProperty("viewValue");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_maxValue);
        EditorGUILayout.PropertyField(m_minValue);
        EditorGUILayout.PropertyField(m_label);
        EditorGUILayout.PropertyField(m_EaseType);
        EditorGUILayout.PropertyField(m_style);
        EditorGUILayout.PropertyField(m_direction);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_value);
        if (EditorGUI.EndChangeCheck())
            (serializedObject.targetObject as XProgress).SetValue(m_value.floatValue * (m_maxValue.floatValue - m_minValue.floatValue));

        serializedObject.ApplyModifiedProperties();
    }
}
