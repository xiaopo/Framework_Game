using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Linq;
using XGUI;

[CustomEditor(typeof(XProgressImage))]
public class ChildXProgressImageInspector : ImageEditor
{
    SerializedProperty m_FillMethod;
    SerializedProperty m_FillOrigin;
    SerializedProperty m_FillAmount;
    SerializedProperty m_FillClockwise;
    SerializedProperty m_Type;
    SerializedProperty m_FillCenter;
    SerializedProperty m_Sprite;
    SerializedProperty m_PreserveAspect;
    GUIContent m_SpriteContent;
    GUIContent m_SpriteTypeContent;
    GUIContent m_ClockwiseContent;
    AnimBool m_ShowSlicedOrTiled;
    AnimBool m_ShowSliced;
    AnimBool m_ShowFilled;
    AnimBool m_ShowType;

    void SetShowNativeSize(bool instant)
    {
        XProgressImage.Type type = (XProgressImage.Type)m_Type.enumValueIndex;
        bool showNativeSize = (type == XProgressImage.Type.Simple || type == XProgressImage.Type.Filled);
        base.SetShowNativeSize(showNativeSize, instant);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_SpriteContent = new GUIContent("Source XProgressImage");
        m_SpriteTypeContent = new GUIContent("XProgressImage Type");
        m_ClockwiseContent = new GUIContent("Clockwise");

        m_Sprite = serializedObject.FindProperty("m_Sprite");
        m_Type = serializedObject.FindProperty("m_Type");
        m_FillCenter = serializedObject.FindProperty("m_FillCenter");
        m_FillMethod = serializedObject.FindProperty("m_FillMethod");
        m_FillOrigin = serializedObject.FindProperty("m_FillOrigin");
        m_FillClockwise = serializedObject.FindProperty("m_FillClockwise");
        m_FillAmount = serializedObject.FindProperty("m_FillAmount");
        m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");

        m_ShowType = new AnimBool(m_Sprite.objectReferenceValue != null);
        m_ShowType.valueChanged.AddListener(Repaint);

        var typeEnum = (XProgressImage.Type)m_Type.enumValueIndex;
        m_ShowSlicedOrTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == XProgressImage.Type.Sliced);
        m_ShowSliced = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == XProgressImage.Type.Sliced);
        m_ShowFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == XProgressImage.Type.Filled);
        m_ShowSlicedOrTiled.valueChanged.AddListener(Repaint);
        m_ShowSliced.valueChanged.AddListener(Repaint);
        m_ShowFilled.valueChanged.AddListener(Repaint);

        SetShowNativeSize(true);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SpriteGUI();
        AppearanceControlsGUI();
        RaycastControlsGUI();

        m_ShowType.target = m_Sprite.objectReferenceValue != null;
        if (EditorGUILayout.BeginFadeGroup(m_ShowType.faded))
        {
            EditorGUILayout.PropertyField(m_Type, m_SpriteTypeContent);

            ++EditorGUI.indentLevel;
            {
                XProgressImage.Type typeEnum = (XProgressImage.Type)m_Type.enumValueIndex;
                bool showSlicedOrTiled = (!m_Type.hasMultipleDifferentValues && (typeEnum == XProgressImage.Type.Sliced || typeEnum == XProgressImage.Type.Tiled));
                if (showSlicedOrTiled && targets.Length > 1)
                    showSlicedOrTiled = targets.Select(obj => obj as XProgressImage).All(img => img.hasBorder);

                m_ShowSlicedOrTiled.target = showSlicedOrTiled;
                m_ShowSliced.target = (showSlicedOrTiled && !m_Type.hasMultipleDifferentValues && typeEnum == XProgressImage.Type.Sliced);
                m_ShowFilled.target = (!m_Type.hasMultipleDifferentValues && typeEnum == XProgressImage.Type.Filled);

                XProgressImage cImage = target as XProgressImage;

                if (EditorGUILayout.BeginFadeGroup(m_ShowSlicedOrTiled.faded))
                {
                    if (cImage.hasBorder)
                    {
                        EditorGUILayout.PropertyField(m_FillCenter);
                        EditorGUILayout.PropertyField(m_FillAmount);
                    }

                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSliced.faded))
                {
                    if (cImage.sprite != null && !cImage.hasBorder)
                        EditorGUILayout.HelpBox("This XProgressImage doesn't have a border.", MessageType.Warning);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowFilled.faded))
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(m_FillMethod);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_FillOrigin.intValue = 0;
                    }
                    switch ((XProgressImage.FillMethod)m_FillMethod.enumValueIndex)
                    {
                        case XProgressImage.FillMethod.Horizontal:
                            m_FillOrigin.intValue = (int)(XProgressImage.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin", (XProgressImage.OriginHorizontal)m_FillOrigin.intValue);
                            break;
                        case XProgressImage.FillMethod.Vertical:
                            m_FillOrigin.intValue = (int)(XProgressImage.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin", (XProgressImage.OriginVertical)m_FillOrigin.intValue);
                            break;
                        case XProgressImage.FillMethod.Radial90:
                            m_FillOrigin.intValue = (int)(XProgressImage.Origin90)EditorGUILayout.EnumPopup("Fill Origin", (XProgressImage.Origin90)m_FillOrigin.intValue);
                            break;
                        case XProgressImage.FillMethod.Radial180:
                            m_FillOrigin.intValue = (int)(XProgressImage.Origin180)EditorGUILayout.EnumPopup("Fill Origin", (XProgressImage.Origin180)m_FillOrigin.intValue);
                            break;
                        case XProgressImage.FillMethod.Radial360:
                            m_FillOrigin.intValue = (int)(XProgressImage.Origin360)EditorGUILayout.EnumPopup("Fill Origin", (XProgressImage.Origin360)m_FillOrigin.intValue);
                            break;
                    }
                    EditorGUILayout.PropertyField(m_FillAmount);
                    if ((XProgressImage.FillMethod)m_FillMethod.enumValueIndex > XProgressImage.FillMethod.Vertical)
                    {
                        EditorGUILayout.PropertyField(m_FillClockwise, m_ClockwiseContent);
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
        }

        EditorGUILayout.EndFadeGroup();

        SetShowNativeSize(false);
        if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_PreserveAspect);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        NativeSizeButtonGUI();

        serializedObject.ApplyModifiedProperties();
    }
}