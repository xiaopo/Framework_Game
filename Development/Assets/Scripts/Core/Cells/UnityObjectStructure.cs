using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using XGUI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[System.Serializable]
public class UnityObjectStructure
{
    [System.Serializable]
    public class UnityObject
    {
        [SerializeField]
        private string m_Name;
        public string name { get { return m_Name; } }
        [SerializeField]
        private Object m_Target;
        public UnityEngine.Object target { get { return m_Target; } }
        [SerializeField]
        private Object m_Component;
        public UnityEngine.Object component { get { return m_Component; } }
    }

    [SerializeField]
    private List<UnityObject> m_UnityObjects;
    public List<UnityObject> unityObjects { get { return m_UnityObjects; } }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(UnityObjectStructure), true)]
class ComponentInfoDraw : PropertyDrawer
{
    private ReorderableList m_ReorderableList;
    private SerializedProperty m_SerializedProperty;

    ReorderableList GetList()
    {
        if (m_ReorderableList == null)
        {
            SerializedProperty elements = m_SerializedProperty.FindPropertyRelative("m_UnityObjects");
            m_ReorderableList = new ReorderableList(m_SerializedProperty.serializedObject, elements, false, true, true, true);
            m_ReorderableList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawItemHeader);
            m_ReorderableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawItemRenderer);
            m_ReorderableList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddItemRenderer);
            m_ReorderableList.elementHeight = 22f;
            m_ReorderableList.draggable = true;
        }
        return m_ReorderableList;
    }

    private void AddItemRenderer(ReorderableList list)
    {
        list.serializedProperty.arraySize++;
        list.index = list.serializedProperty.arraySize - 1;
        SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(list.index);
        item.FindPropertyRelative("m_Target").objectReferenceValue = null;
        item.FindPropertyRelative("m_Name").stringValue = string.Empty;
        item.FindPropertyRelative("m_Component").objectReferenceValue = null;
    }

    private void DrawItemRenderer(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty itempro = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

        Rect objectRect = rect;
        objectRect.y += 2;
        objectRect.height = 16;
        objectRect.width *= 0.3f;
        SerializedProperty target = itempro.FindPropertyRelative("m_Target");
        EditorGUI.BeginChangeCheck();
        target.objectReferenceValue = EditorGUI.ObjectField(objectRect, target.objectReferenceValue, typeof(Object), true);
        Component comp = itempro.FindPropertyRelative("m_Component").objectReferenceValue as Component;
        if (comp != null && target.objectReferenceValue == null)
            target.objectReferenceValue = comp.gameObject;
        bool change = EditorGUI.EndChangeCheck();

        if (change)
        {
            if (target.objectReferenceValue is MonoScript)
            {
                target.objectReferenceValue = null;
            }


            if (target.objectReferenceValue != null && itempro.FindPropertyRelative("m_Component").objectReferenceValue == null)
            {
                itempro.FindPropertyRelative("m_Component").objectReferenceValue = target.objectReferenceValue;
            }
            else if (target.objectReferenceValue == null)
            {
                itempro.FindPropertyRelative("m_Component").objectReferenceValue = null;
                itempro.FindPropertyRelative("m_Name").stringValue = string.Empty;
            }
        }


        Rect popupRect = objectRect;
        popupRect.x += rect.width * 0.3f + 5;

        EditorGUI.BeginDisabledGroup(target.objectReferenceValue == null);
        BuildPopupList(popupRect, target.objectReferenceValue, itempro);

        Rect inputRect = popupRect;
        inputRect.x += rect.width * 0.3f + 5;
        inputRect.width = (rect.width - inputRect.x) + 10;
        SerializedProperty name = itempro.FindPropertyRelative("m_Name");

        if (change && string.IsNullOrEmpty(name.stringValue) && target.objectReferenceValue != null)
        {
            name.stringValue = target.objectReferenceValue.name;
        }

        EditorGUI.BeginChangeCheck();
        name.stringValue = EditorGUI.TextField(inputRect, name.stringValue);
        change = EditorGUI.EndChangeCheck();

        if (change && string.IsNullOrEmpty(name.stringValue) && target.objectReferenceValue != null)
        {
            name.stringValue = target.objectReferenceValue.name;
        }

        EditorGUI.EndDisabledGroup();
    }


    void BuildPopupList(Rect rect, Object obj, SerializedProperty itempro)
    {
        SerializedProperty component = itempro.FindPropertyRelative("m_Component");
        int index = 0;
        string[] contents = new string[0] { };
        Object[] comps = null;
        if (obj != null)
        {
            GameObject go = obj as GameObject;
            if (go != null)
            {
                comps = go.GetComponents<Component>();

                ArrayUtility.Insert<Object>(ref comps, 0, itempro.FindPropertyRelative("m_Target").objectReferenceValue);

                for (int i = 0; i < comps.Length; i++)
                    if (comps[i] == component.objectReferenceValue)
                    {
                        index = i;
                        break;
                    }

            }
            else
            {
                comps = new Object[] { obj };
            }

            List<string> list = (from c in comps
                                 where c != null
                                 select c.GetType().Name).ToList<string>();

            contents = list.ToArray();
        }

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(rect, index, contents);
        if (EditorGUI.EndChangeCheck())
        {
            component.objectReferenceValue = comps[index];
        }
    }

    private void DrawItemHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Objects");
        float btnWidth = 100;
        rect.x = rect.width - btnWidth + 20;
        rect.width = btnWidth;

    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        this.m_SerializedProperty = property;
        ReorderableList list = GetList();
        list.DoList(position);
        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        this.m_SerializedProperty = property;
        ReorderableList list = GetList();
        return list != null ? list.GetHeight() : 0f;
    }



    private static void GetAllChild(GameObject go, List<GameObject> childs)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform temp = go.transform.GetChild(i);
            XView v = temp.GetComponent<XView>();
            if (v && v.GetType().Name.Contains("XView"))
            {
                continue;
            }

            if (temp.name.StartsWith("b_"))
            {
                childs.Add(temp.gameObject);
            }

            if (temp.childCount > 0)
            {
                GetAllChild(temp.gameObject, childs);
            }
        }
    }


    private static List<string> componentLevel = new List<string>() {
        "XTabView","XListView","InputField","Toggle","XButton","XImage","Button","RawImage","Slider                                                                                                                                                                                                                    ","Image","Text","GameObject"
    };

    private static string GetComponts(GameObject go)
    {
        Component[] components = go.GetComponents<Component>();
        string componentName = "";
        int level = 999;
        string returnName = "";
        for (int i = 0; i < components.Length; i++)
        {
            componentName = components[i].GetType().Name;
            for (int j = 0; j < componentLevel.Count; j++)
            {

                if (componentLevel[j] == componentName && j < level)
                {
                    returnName = componentName;
                    level = j;
                }
            }
        }
        return returnName;
    }
}
#endif