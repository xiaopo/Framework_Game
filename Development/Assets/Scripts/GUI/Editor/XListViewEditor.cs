using UnityEngine;
using UnityEditor;
using XGUI;
using System;

[CustomEditor(typeof(XListView), true)]
public class XListViewEditor : Editor
{
    SerializedProperty m_ScrollRect;
    SerializedProperty m_Spacing;
    SerializedProperty m_DataCount;
    SerializedProperty m_ListLayout;
    SerializedProperty m_customLayout;
    SerializedProperty m_ControllChildSize;
    SerializedProperty m_CellSize;
    SerializedProperty m_GridConstraint;
    SerializedProperty m_GridConstraintCount;
    SerializedProperty m_RecycleDeActive;
    SerializedProperty m_WaitCreateCount;
    SerializedProperty m_Template;
    SerializedProperty m_TemplateAsset;
    SerializedProperty m_Objects;

    int m_ScrollToIndex = 0;
    float m_ScrollSmoothTime = 0;

    protected virtual void OnEnable()
    {
        m_ScrollRect = serializedObject.FindProperty("m_ScrollRect");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_DataCount = serializedObject.FindProperty("m_DataCount");
        m_ListLayout = serializedObject.FindProperty("m_ListLayout");
        m_customLayout = serializedObject.FindProperty("m_customLayout");
        m_CellSize = serializedObject.FindProperty("m_CellSize");
        m_ControllChildSize = serializedObject.FindProperty("m_ControllChildSize");
        m_GridConstraint = serializedObject.FindProperty("m_GridConstraint");
        m_GridConstraintCount = serializedObject.FindProperty("m_GridConstraintCount");
        m_RecycleDeActive = serializedObject.FindProperty("m_RecycleDeActive");
        m_WaitCreateCount = serializedObject.FindProperty("m_WaitCreateCount");
        m_Template = serializedObject.FindProperty("m_Template");
        m_TemplateAsset = serializedObject.FindProperty("m_TemplateAsset");
        m_Objects = serializedObject.FindProperty("m_Objects");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_ScrollRect);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_ListLayout);
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as XListView).layout = (XListView.ListLayout)Enum.ToObject(typeof(XListView.ListLayout), m_ListLayout.enumValueIndex);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_DataCount);
        if (EditorGUI.EndChangeCheck())
        {
            int value = Mathf.Max(0, m_DataCount.intValue);
            (serializedObject.targetObject as XListView).dataCount = value;
            m_DataCount.intValue = value;
        }


        XListView.ListLayout layout = (XListView.ListLayout)Enum.ToObject(typeof(XListView.ListLayout), m_ListLayout.enumValueIndex);
        if (layout == XListView.ListLayout.Horizontal)
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUILayout.FloatField("HSpacing", m_Spacing.vector2Value.x);//Mathf.Max(0, EditorGUILayout.FloatField("HSpacing", m_Spacing.vector2Value.x));
            if (EditorGUI.EndChangeCheck())
            {
                m_Spacing.vector2Value.Set(value, m_Spacing.vector2Value.y);
                (serializedObject.targetObject as XListView).horizontalSpacing = value;
            }
        }
        else if (layout == XListView.ListLayout.Vertical)
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUILayout.FloatField("VSpacing", m_Spacing.vector2Value.y);//Mathf.Max(0, EditorGUILayout.FloatField("VSpacing", m_Spacing.vector2Value.y));
            if (EditorGUI.EndChangeCheck())
            {
                m_Spacing.vector2Value.Set(m_Spacing.vector2Value.x, value);
                (serializedObject.targetObject as XListView).verticalSpacing = value;
            }
        }
        else if (layout == XListView.ListLayout.Grid)
        {
            if (m_ControllChildSize.boolValue)
                EditorGUILayout.PropertyField(m_CellSize);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Spacing);
            EditorGUILayout.PropertyField(m_GridConstraint);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_GridConstraintCount);
            m_GridConstraintCount.intValue = Mathf.Max(1, m_GridConstraintCount.intValue);
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
            {
                (serializedObject.targetObject as XListView).ForceRefresh();
            }
        }
        else if (layout == XListView.ListLayout.Custom)
        {
            EditorGUILayout.PropertyField(m_customLayout);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_ControllChildSize);
        if (EditorGUI.EndChangeCheck())
            (serializedObject.targetObject as XListView).ForceRefresh();

        EditorGUILayout.PropertyField(m_RecycleDeActive);

        EditorGUILayout.PropertyField(m_WaitCreateCount);
        

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
        EditorGUILayout.PropertyField(m_Objects);
        serializedObject.ApplyModifiedProperties();


        if (Application.isPlaying)
        {
            EditorGUILayout.Space();
            //EditorGUILayout.BeginHorizontal();
            m_ScrollToIndex = EditorGUILayout.IntField("位置", m_ScrollToIndex);
            m_ScrollSmoothTime = EditorGUILayout.FloatField("平滑时间", m_ScrollSmoothTime);
            if (GUILayout.Button("ScrollToIndex"))
            {
                (serializedObject.targetObject as XListView).ScrollToIndex(m_ScrollToIndex, m_ScrollSmoothTime);
            }

            if (GUILayout.Button("ForceRefresh"))
            {
                (serializedObject.targetObject as XListView).ForceRefresh();
            }
            
            //EditorGUILayout.EndHorizontal();
        }
    }
}