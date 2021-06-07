using UnityEngine;
using UnityEditor;
using System.Collections;
using XGUI;
using UnityEditor.UI;

[CustomEditor(typeof(XTiledImage), true)]
[CanEditMultipleObjects]
public class XTiledImageEditor : ImageEditor
{
    SerializedProperty m_adjustOffset;
    SerializedProperty m_overrideSpriteSize;
    SerializedProperty m_column;
    SerializedProperty m_row;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_adjustOffset = serializedObject.FindProperty("m_adjustOffset");
        m_overrideSpriteSize = serializedObject.FindProperty("m_overrideSpriteSize");
        m_column = serializedObject.FindProperty("m_column");
        m_row = serializedObject.FindProperty("m_row");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_adjustOffset);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_overrideSpriteSize);
        if (m_overrideSpriteSize.vector2Value == Vector2.zero)
        {
            if (m_overrideSpriteSize != null)
            {
                XTiledImage tiledImage = (serializedObject.targetObject as XTiledImage);
                if (tiledImage != null && tiledImage.overrideSprite !=null)
                {
                    m_overrideSpriteSize.vector2Value = tiledImage.overrideSprite.rect.size;
                }
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            XTiledImage tiledImage = (serializedObject.targetObject as XTiledImage);
            tiledImage.OverrideSpriteSize = m_overrideSpriteSize.vector2Value;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_column);
        if (EditorGUI.EndChangeCheck())
        {
            XTiledImage tiledImage = (serializedObject.targetObject as XTiledImage);
            tiledImage.Column = m_column.floatValue;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_row);
        if (EditorGUI.EndChangeCheck())
        {
            XTiledImage tiledImage = (serializedObject.targetObject as XTiledImage);
            tiledImage.Row = m_row.floatValue;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
