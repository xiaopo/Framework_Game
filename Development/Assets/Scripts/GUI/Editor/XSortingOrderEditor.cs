using UnityEngine;
using UnityEditor;
using XGUI;
using System;

[CustomEditor(typeof(XSortingOrder), true)]
public class XSortingOrderEditor : Editor
{
    SerializedProperty m_sortingOrder;
    SerializedProperty m_isUI;

    protected virtual void OnEnable()
    {
        m_sortingOrder = serializedObject.FindProperty("m_sortingOrder");
        m_isUI = serializedObject.FindProperty("m_isUI");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_sortingOrder);
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as XSortingOrder).UpdateSortingOrder();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_isUI);
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as XSortingOrder).UpdateSortingOrder();
        }
       
    }   
}