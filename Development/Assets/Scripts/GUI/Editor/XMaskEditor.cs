using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(XMask), true)]
public class XMaskEditor : Editor
{
    SerializedProperty sprite;
    SerializedProperty maskRotateSpeed;
    SerializedProperty uvAdd;
    // Use this for initialization

    protected virtual void OnEnable()
    {
        sprite = serializedObject.FindProperty("sprite");
        maskRotateSpeed = serializedObject.FindProperty("maskRotateSpeed");
        uvAdd = serializedObject.FindProperty("uvAdd");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(maskRotateSpeed);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(uvAdd);
        if (EditorGUI.EndChangeCheck())
        {
            (serializedObject.targetObject as XMask).p_UvAdd = uvAdd.floatValue;
        }
        serializedObject.ApplyModifiedProperties();

    }
}
