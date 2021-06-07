using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using XGUI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using AssetManagement;
#endif

[System.Serializable]
public class XImageGroupStructure
{
    [System.Serializable]
    public class UnityObject
    {
        [SerializeField]
        private string m_Name;
        public string name { get { return m_Name; } }
        [SerializeField]
        private Object m_Target;
        public UnityEngine.Object target { get { return m_Target; } set { m_Target = value; } }
        [SerializeField]
        private XImageNode m_Node;
        public XImageNode node
        {
            get
            {
                if (m_Node == null && target != null)
                {
                    m_Node = (target as XImage).GetComponent<XImageNode>();
                }
                return m_Node;
            }
            set
            {
                m_Node = value;
            }
        }
    }
    [SerializeField]
    private List<UnityObject> m_UnityObjects;
    public List<UnityObject> unityObjects { get { return m_UnityObjects; } }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(XImageGroupStructure), true)]
class ComponentInfoDrawXImageGroup : PropertyDrawer
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
        item.FindPropertyRelative("m_Node").objectReferenceValue = null;
        item.FindPropertyRelative("m_Target").objectReferenceValue = null;
        //item.FindPropertyRelative("m_Component").objectReferenceValue = null;
    }

    private void DrawItemRenderer(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty itempro = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

        Rect objectRect = rect;
        objectRect.y += 2;
        objectRect.height = 16;
        SerializedProperty target = itempro.FindPropertyRelative("m_Target");
        EditorGUI.BeginChangeCheck();

        if (target.objectReferenceValue != null)
        {
            if (!Application.isPlaying)
            {
                XGUI.XImageGroup obj = (XGUI.XImageGroup)m_SerializedProperty.serializedObject.targetObject;
                XImageNode node = null;
                (target.objectReferenceValue as XImage).TryGetComponent<XImageNode>(out node);

            }
            target.objectReferenceValue = EditorGUI.ObjectField(objectRect, target.objectReferenceValue, typeof(XImage), true);
        }
        else
        {
            target.objectReferenceValue = EditorGUI.ObjectField(objectRect, null, typeof(XImage), true);
        }

        //Component comp = itempro.FindPropertyRelative("m_Component").objectReferenceValue as Component;
        //if (comp != null && target.objectReferenceValue == null)
        //    target.objectReferenceValue = comp.gameObject;
        bool change = EditorGUI.EndChangeCheck();

        if (change)
        {
            if (!(target.objectReferenceValue is XImage))
            {
                return;
            }

            //if (target.objectReferenceValue != null && itempro.FindPropertyRelative("m_Component").objectReferenceValue == null)
            //{
            //    itempro.FindPropertyRelative("m_Component").objectReferenceValue = target.objectReferenceValue;
            //}
            if (target.objectReferenceValue == null)
            {
                XImageNode node = (itempro.FindPropertyRelative("m_Node").objectReferenceValue as XImageNode);
                if (node != null)
                {
                    Object.DestroyImmediate(node);
                }
                itempro.FindPropertyRelative("m_Node").objectReferenceValue = null;
                itempro.FindPropertyRelative("m_Target").objectReferenceValue = null;
            }
        }


        //Rect popupRect = objectRect;
        //popupRect.x += rect.width * 0.3f + 10;

        //EditorGUI.BeginDisabledGroup(target.objectReferenceValue == null);
        //BuildPopupList(popupRect, target.objectReferenceValue, itempro);

        //Rect inputRect = popupRect;
        //inputRect.x += rect.width * 0.3f + 5;
        //inputRect.width = (rect.width - inputRect.x) + 10;
        //SerializedProperty name = itempro.FindPropertyRelative("m_Name");

        //if (change && string.IsNullOrEmpty(name.stringValue) && target.objectReferenceValue != null)
        //{
        //    name.stringValue = (target.objectReferenceValue as XImage).sprite.name;
        //}

        //EditorGUI.BeginChangeCheck();
        //name.stringValue = EditorGUI.TextField(inputRect, name.stringValue);
        //change = EditorGUI.EndChangeCheck();

        //if (change && string.IsNullOrEmpty(name.stringValue) && target.objectReferenceValue != null)
        //{
        //    name.stringValue = target.objectReferenceValue.name;
        //}

        //EditorGUI.EndDisabledGroup();
    }

    private void DrawItemHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Objects");
        float offset = 100;
        rect.x = rect.width - 100;

        Rect rect1 = new Rect();
        rect1.y = rect.y;
        rect1.x = rect.width - 2 * offset;
        rect1.height = rect.height;
        rect1.width = 100;

        Rect rect2 = new Rect();
        rect2.y = rect.y;
        rect2.x = rect.width - 3 * offset;
        rect2.height = rect.height;
        rect2.width = 100;

        Rect rect3 = new Rect();
        rect3.y = rect.y;
        rect3.x = rect.width - 4 * offset;
        rect3.height = rect2.height;
        rect3.width = 100;


        rect.width = 100;
        if (GUI.Button(rect, "Auto Find XImage", EditorStyles.miniButton))
            AutoFindXImage();
        else if (GUI.Button(rect1, "Auto Find Name", EditorStyles.miniButton))
            AutoFindName();
        else if (GUI.Button(rect2, "Clear Image", EditorStyles.miniButton))
            ClearImage();
        else if (GUI.Button(rect3, "PreView", EditorStyles.miniButton))
            Preview(); 

    }


    private static Dictionary<string, string> GetResource()
    {
        Dictionary<string, string> fileDateBase = new Dictionary<string, string>();
        string[] paths = {
            @"Assets/GUI/Modules/ActivityBlend/Images" ,
            @"Assets/GUI/Modules/Common/Images",
            @"Assets/GUI/Modules/ChargeActivity/Images/Single"
        };
        foreach (var path in paths)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles("*.png", SearchOption.AllDirectories);

                foreach (var fileInfo in files)
                {
                    fileDateBase[fileInfo.Name] = fileInfo.FullName;
                }
            }
        }
        return fileDateBase;
    }
    //预览
    void Preview()
    {
        Dictionary<string,string> fileDateBase = GetResource();

        XGUI.XImageGroup obj = (XGUI.XImageGroup)m_SerializedProperty.serializedObject.targetObject;
        SerializedProperty elements = m_SerializedProperty.FindPropertyRelative("m_UnityObjects");

        XImageNode[] xImageNodeArray = obj.GetComponentsInChildren<XImageNode>(true);

        foreach (XImageNode imageNode in xImageNodeArray)
        {
            XImage image = imageNode.GetComponent<XImage>();
            if (image == null) {
                continue;
            }
            string spriteName_origin = string.Format("{0}.{1}", imageNode.imageName, "png");
            string spriteName_suffix = string.Format("{0}_{1}.{2}", imageNode.imageName, obj.nameSuffix, "png");

            string spriteName = fileDateBase.ContainsKey(spriteName_suffix)? spriteName_suffix: spriteName_origin;

            if (!string.IsNullOrEmpty(spriteName) && fileDateBase.ContainsKey(spriteName)) {
                image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fileDateBase[spriteName].Substring(fileDateBase[spriteName].LastIndexOf("Assets\\")));
                if (image.autoSetNativeSize) 
                { 
                    image.SetNativeSize();
                }
            }

            bool anyEmpty = true;
            while (anyEmpty)
            {
                anyEmpty = false;
                for (int i = 0; i < elements.arraySize; i++)
                {
                    SerializedProperty item = elements.GetArrayElementAtIndex(i);
                    var img = (XImage)item.FindPropertyRelative("m_Target").objectReferenceValue;
                    if (img == null)
                    {
                        anyEmpty = true;
                        elements.DeleteArrayElementAtIndex(i);
                        break;
                    }
                }
            }

            bool isfind = false;
            for (int i = 0; i < elements.arraySize; i++) {
                var item = elements.GetArrayElementAtIndex(i);
                var tImage = (XImage)item.FindPropertyRelative("m_Target").objectReferenceValue;

                if (tImage != null && image.GetInstanceID().Equals(tImage.GetInstanceID())) {
                    isfind = true;
                    break;
                }
            }
            if (!isfind) {
                elements.InsertArrayElementAtIndex(elements.arraySize);
                SerializedProperty item = elements.GetArrayElementAtIndex(elements.arraySize - 1);
                item.FindPropertyRelative("m_Target").objectReferenceValue = image;
            }
        }


            EditorUtility.SetDirty(m_SerializedProperty.serializedObject.targetObject);
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

    //自动添加
    void AutoFindXImage()
    {
        XGUI.XImageGroup obj = (XGUI.XImageGroup)m_SerializedProperty.serializedObject.targetObject;

        //Component[] components = obj.GetComponents<Component>();
        //List<XImage> childs = new List<XImage>();
        XImage[] xImagesArray = obj.GetComponentsInChildren<XImage>(true);
        XImageNode[] xImageNodeArray = obj.GetComponentsInChildren<XImageNode>(true);
        SerializedProperty elements = m_SerializedProperty.FindPropertyRelative("m_UnityObjects");
        //elements();
        //elements.array
        elements.ClearArray();

        foreach (XImageNode imageNode in xImageNodeArray)
        {
            GameObject.DestroyImmediate(imageNode);
        }
    }

    void AutoFindName()
    {
        XGUI.XImageGroup obj = (XGUI.XImageGroup)m_SerializedProperty.serializedObject.targetObject;

        XImageNode[] xImageNodeArray = obj.GetComponentsInChildren<XImageNode>(true);

        foreach (XImageNode imageNode in xImageNodeArray)
        {
            imageNode.imageName = imageNode.GetComponent<XImage>().sprite.name;
            imageNode.imageGroup = obj;
        }
    }

    void ClearImage()
    {
        XGUI.XImageGroup obj = (XGUI.XImageGroup)m_SerializedProperty.serializedObject.targetObject;

        XImageNode[] xImageNodeArray = obj.GetComponentsInChildren<XImageNode>(true);

        foreach (XImageNode imageNode in xImageNodeArray)
        {
            XImage image = imageNode.GetComponent<XImage>();
            if (image != null)
            {
                image.sprite = null;
            }
        }

        EditorUtility.SetDirty(m_SerializedProperty.serializedObject.targetObject);
    }
}
#endif