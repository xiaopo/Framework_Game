using UnityEngine;
using UnityEditor;
using XGUI;
using System;

[CustomEditor(typeof(XLoader), true)]
public class XLoaderEditor : Editor
{
    
    SerializedProperty m_Template;
    SerializedProperty m_TemplateAsset;

    protected virtual void OnEnable()
    {
        m_Template = serializedObject.FindProperty("m_Template");
        m_TemplateAsset = serializedObject.FindProperty("m_TemplateAsset");
    }


    public override void OnInspectorGUI()
    {
    
        EditorGUI.BeginChangeCheck();
        UnityEngine.Object lastObject = m_Template.objectReferenceValue;
        EditorGUILayout.ObjectField(m_Template);
        if (EditorGUI.EndChangeCheck() && m_Template.objectReferenceValue != null)
        {
            if (EditorUtility.IsPersistent(m_Template.objectReferenceValue))
            {
                m_TemplateAsset.stringValue = m_Template.objectReferenceValue.name + ".prefab";
                m_Template.objectReferenceValue = lastObject;
            }
        }
        EditorGUILayout.PropertyField(m_TemplateAsset);
       
        serializedObject.ApplyModifiedProperties();
       
    }
}