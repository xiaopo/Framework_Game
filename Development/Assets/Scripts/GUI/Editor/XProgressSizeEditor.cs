using System;
using UnityEditor;
using UnityEngine;

namespace XGUI
{
    [CustomEditor(typeof(XProgressSize), true)]
    public class XProgressSizeEditor : Editor
    {
        SerializedProperty m_value;
        SerializedProperty m_maxValue;
        SerializedProperty m_playType;
        SerializedProperty m_easeType;
        protected void OnEnable()
        {
            m_value = serializedObject.FindProperty("m_value");
            m_maxValue = serializedObject.FindProperty("m_maxValue");
            m_playType = serializedObject.FindProperty("m_playType");
            m_easeType = serializedObject.FindProperty("m_easeType");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            XProgressSize.PlayType playType = (XProgressSize.PlayType)Enum.ToObject(typeof(XProgressSize.PlayType), m_playType.enumValueIndex);

            if (playType != XProgressSize.PlayType.NONE)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(m_easeType);
                serializedObject.ApplyModifiedProperties();
            }


            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_maxValue);
            EditorGUILayout.PropertyField(m_value);
            if (EditorGUI.EndChangeCheck())
            {
                float max = Mathf.Max(0, m_maxValue.floatValue);
                float cur = Mathf.Max(0, m_value.floatValue);
                XProgressSize progress = (serializedObject.targetObject as XProgressSize);
                float duration = progress.m_duration;
                progress.m_value = cur;
                progress.m_maxValue = max;
                progress.InitUI();
                // progress.CleanWait();

                progress.SetValue(cur, max, duration);
            }
        }
    }
}

