using UnityEngine;
using UnityEditor;
using XGUI;
using System;

[CustomEditor(typeof(GradientEffect), true)]
public class GradientEffectEditor : Editor
{
    SerializedProperty _topColor;
    SerializedProperty _bottomColor;
    SerializedProperty m_Direction;

    protected virtual void OnEnable()
    {
        m_Direction = serializedObject.FindProperty("m_Direction");
        _topColor = serializedObject.FindProperty("_topColor");
        _bottomColor = serializedObject.FindProperty("_bottomColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_topColor);
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as GradientEffect).topColor = _topColor.colorValue;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_bottomColor);
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as GradientEffect).bottomColor = _bottomColor.colorValue;
        }
        EditorGUILayout.PropertyField(m_Direction);
        serializedObject.ApplyModifiedProperties();
    }
}
