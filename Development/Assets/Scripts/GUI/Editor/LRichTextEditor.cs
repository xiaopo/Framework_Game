using UnityEngine;
using UnityEditor;
using XGUI;
using System;

[CustomEditor(typeof(LRichText), true)]
public class LRichTextEditor : Editor
{

    SerializedProperty m_Text;

    protected virtual void OnEnable()
    {
        m_Text = serializedObject.FindProperty("m_Text");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //EditorGUI.BeginChangeCheck();
        //EditorGUILayout.PropertyField(m_Text);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    LRichText richText = (serializedObject.targetObject as LRichText);
        //    richText.text = m_Text.stringValue;
        //}
        //serializedObject.ApplyModifiedProperties();

        //if (Application.isPlaying)
        //{
            EditorGUILayout.Space();
            if (GUILayout.Button("Roloaded"))
            {
                (serializedObject.targetObject as LRichText).text = m_Text.stringValue;
            }
        //}
       
    }
}