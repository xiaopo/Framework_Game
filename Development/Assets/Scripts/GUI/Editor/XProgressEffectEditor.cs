using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using XGUI;

[CustomEditor(typeof(XProgressEffect), true)]
[CanEditMultipleObjects]

public class XProgressEffectEditor : XProgressEditor
{

    SerializedProperty m_content;
    SerializedProperty m_effect;

    protected override void OnEnable()
    {
        m_content = serializedObject.FindProperty("m_content");
        m_effect = serializedObject.FindProperty("m_effect");

        base.OnEnable();

    }
    public override void OnInspectorGUI()
    {

        EditorGUILayout.ObjectField(m_content);
        if (EditorGUI.EndChangeCheck() && m_content.objectReferenceValue != null)
        {
            EditorUtility.IsPersistent(m_content.objectReferenceValue);
        }


        EditorGUILayout.ObjectField(m_effect);
        if (EditorGUI.EndChangeCheck() && m_effect.objectReferenceValue != null)
        {
            EditorUtility.IsPersistent(m_effect.objectReferenceValue);
        }

 
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
