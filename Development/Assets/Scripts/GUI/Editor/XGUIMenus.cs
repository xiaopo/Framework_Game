using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
using XGUI;

public class XGUIMenus : ScriptableObject
{
    private static string font_path = "Assets/GUI/Fonts/fangzhengls.TTF";

    static GameObject CallMenuOptions(string name, MenuCommand menuCommand)
    {
        
        System.Reflection.Assembly Assembly = System.Reflection.Assembly.Load("UnityEditor.UI");
        System.Type type = Assembly.GetType("UnityEditor.UI.MenuOptions");

        System.Reflection.MethodInfo method = type.GetMethod(name);

        method.Invoke(type, new object[] { menuCommand });

        return Selection.activeGameObject;
    }


    static public void AddXEnvironment(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddCanvas", menuCommand);
        GameObject camGO = new GameObject("UICamera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;

        CanvasScaler scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.matchWidthOrHeight = 0.5f;
    }

    static public GameObject AddXEnvironment3D(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddCanvas", menuCommand);
        GameObject camGO = new GameObject("UICamera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.fieldOfView = 20;
        cam.farClipPlane = 20;
        cam.nearClipPlane = 0.3f;
        //cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.farClipPlane = 3;

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 1;

        CanvasScaler scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.matchWidthOrHeight = 0.5f;
        return go;
    }

    [MenuItem("GameObject/XGUI/XButton", priority = 1)]
    static public GameObject AddGreenButton(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddButton", menuCommand);
        Button btn = go.GetComponent<Button>();
        Object.DestroyImmediate(btn);
        XButton xbutton = go.AddComponent<XButton>();
        Text text = (Text)xbutton.transform.FindComponent(typeof(Text), "Text");
        GameObject obj = text.gameObject;
        DestroyImmediate(text);
        XText xText = obj.AddComponent<XText>();
        xbutton.labelText = xText;
        xbutton.labelText.raycastTarget = false;
        xbutton.transition = Selectable.Transition.None;

        Image img = xbutton.GetComponent<Image>();
        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI/Modules/Common/Images/Buttons/common_btn_03.png");
        img.type = Image.Type.Simple;
        img.SetNativeSize();
        Color color = Color.white;
        xText.fontSize = 28;
        xText.alignment = TextAnchor.MiddleCenter;

        Outline outLine = xText.gameObject.AddComponent<Outline>();

        ColorUtility.TryParseHtmlString("#113f30", out color);
        outLine.effectColor = color;
        outLine.effectDistance = new Vector2(1, 0);
        ColorUtility.TryParseHtmlString("#ffffff", out color);
        xText.color = color;

        Font font = AssetDatabase.LoadAssetAtPath<Font>(font_path);
        if (font != null)
            xbutton.labelText.font = font;

        go.AddComponent<XButtonScaleTween>();
        return go;
    }

    [MenuItem("GameObject/XGUI/XImage", priority = 2)]
    static public void AddXImage(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddImage", menuCommand);
        Image img = go.GetComponent<Image>();
        Object.DestroyImmediate(img);
        img = go.AddComponent<XImage>();
        img.raycastTarget = false;
    }


    [MenuItem("GameObject/XGUI/XText", priority = 3)]
    static public GameObject AddXText(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddText", menuCommand);
        DestroyImmediate(go.GetComponent<Text>());

        XText txt = go.AddComponent<XText>();
        Font font = AssetDatabase.LoadAssetAtPath<Font>(font_path);
        if (font != null)
            txt.font = font;
        txt.fontSize = 24;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.white;
        txt.raycastTarget = false;

        return go;
    }


    [MenuItem("GameObject/XGUI/XListView", priority = 4)]
    static public GameObject AddListView(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddScrollView", menuCommand);
        Transform transform = go.transform;
        go.name = "ListView";
        Object.DestroyImmediate(go.GetComponent<Image>());
        Object.DestroyImmediate(transform.Find("Scrollbar Horizontal").gameObject);
        Object.DestroyImmediate(transform.Find("Scrollbar Vertical").gameObject);

        go.AddComponent<XNoDrawingView>();
        ScrollRect scrollRect = go.GetComponent<ScrollRect>();
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;
        viewport.anchorMin = Vector2.zero;
        viewport.anchorMax = Vector2.one;
        viewport.offsetMin = viewport.offsetMax = Vector2.zero;

        content.anchoredPosition = Vector2.zero;
        content.sizeDelta = new Vector2(0, viewport.rect.height);


        Object.DestroyImmediate(scrollRect);

        scrollRect = go.AddComponent<XScrollRect>();
        scrollRect.content = content;
        scrollRect.viewport = viewport;

        go.AddComponent<XListView>().xScrollRect = scrollRect as XScrollRect;

        Mask mask = transform.Find("Viewport").GetComponent<Mask>();
        Image image = transform.Find("Viewport").GetComponent<Image>();
        mask.gameObject.AddComponent<RectMask2D>();
        Object.DestroyImmediate(mask);
        Object.DestroyImmediate(image);
        return go;
    }

    [MenuItem("GameObject/XGUI/XListView Small", priority = 5)]
    static public void AddListViewSmall(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddScrollView", menuCommand);
        Transform transform = go.transform;
        go.name = "ListView Small";
        Object.DestroyImmediate(transform.Find("Scrollbar Horizontal").gameObject);
        Object.DestroyImmediate(transform.Find("Scrollbar Vertical").gameObject);

        Object.DestroyImmediate(go.GetComponent<Image>());
        go.AddComponent<XNoDrawingView>();

        ScrollRect scrollRect = go.GetComponent<ScrollRect>();
        RectTransform content = scrollRect.content;

        Object.DestroyImmediate(content.gameObject);

        RectTransform viewport = scrollRect.viewport;
        viewport.anchorMin = Vector2.zero;
        viewport.anchorMax = Vector2.one;
        viewport.offsetMin = viewport.offsetMax = Vector2.zero;



        Object.DestroyImmediate(scrollRect);

        go.AddComponent<XListView>().xScrollRect = scrollRect as XScrollRect;

        viewport.gameObject.name = "Content";
        Mask mask = viewport.gameObject.GetComponent<Mask>();
        Image image = viewport.gameObject.GetComponent<Image>();
        Object.DestroyImmediate(mask);
        Object.DestroyImmediate(image);
    }

    [MenuItem("GameObject/XGUI/XListTreeView", priority = 6)]
    static public void AddXListTreeView(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddScrollView", menuCommand);
        Transform transform = go.transform;
        go.name = "ListTreeView";
        Object.DestroyImmediate(transform.Find("Scrollbar Horizontal").gameObject);
        Object.DestroyImmediate(transform.Find("Scrollbar Vertical").gameObject);

        RectTransform m_transform = go.GetComponent<RectTransform>();
        Object.DestroyImmediate(go.GetComponent<Image>());

        ScrollRect scroll = go.GetComponent<ScrollRect>();
        RectTransform viewPort = scroll.viewport;
        RectTransform content = scroll.content;

        Object.DestroyImmediate(scroll);

        XScrollRect xrect = go.AddComponent<XScrollRect>();
        xrect.viewport = viewPort;
        xrect.content = content;

        viewPort.anchorMin = new Vector2(0, 0);
        viewPort.anchorMax = new Vector2(1, 1);
        viewPort.pivot = new Vector2(0, 1.0f);
        viewPort.sizeDelta = Vector2.zero;

        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(0, 1);
        content.pivot = new Vector2(0, 1);
        content.anchoredPosition = Vector2.zero;

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(200, 20);
        rect.anchoredPosition = Vector2.zero;

        m_transform.sizeDelta = new Vector2(200, 400);

        XListTreeView treeView = go.AddComponent<XListTreeView>();
        xrect.horizontal = false;
        treeView.m_scrollRect = xrect;
        treeView.m_viewPort = viewPort;
        treeView.m_content = content;

        for(int i = 0;i< 3;i++)
        {
            ListTreeObj treeObj = GetTreeItemTemplates(transform,i);
            treeView.templates.Add(treeObj);
        }
       
    }
    private static ListTreeObj GetTreeItemTemplates(Transform transform,int index)
    {
        ListTreeObj treeObj = new ListTreeObj();
        GameObject obj = new GameObject("template_" + index, typeof(XImage), typeof(XButton));
        GameObject textO = new GameObject("text", typeof(XText));
        textO.transform.SetParentOEx(obj.transform);
        ((RectTransform)textO.transform).sizeDelta = new Vector2(200, 20);
        XText text = textO.GetComponent<XText>();
        text.text = "渲染条__" + index;
        text.color = Color.red;
        obj.transform.SetParentOEx(transform);
        treeObj.template = obj;
        treeObj.transform.anchorMin = new Vector2(0f, 1f);
        treeObj.transform.anchorMax = new Vector2(0f, 1f);
        treeObj.transform.pivot = new Vector2(0f, 1f);
        treeObj.transform.anchoredPosition = new Vector2(0, -30 * index);
        treeObj.transform.sizeDelta = new Vector2(200, 20);

        return treeObj;
    }



    [MenuItem("GameObject/XGUI/XSpriteMask", priority = 10)]
    static public void AddSpriteMaskOrder(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddImage", menuCommand);
        GameObject obj = new GameObject();
        obj.transform.parent = go.transform.parent;
        Object.DestroyImmediate(go);
        obj.name = "SpriteMask";
        SpriteMask sm = obj.AddComponent<SpriteMask>();
        sm.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI/Modules/MainUI/Images/zjm_bg_liaotiandi.png");
        sm.alphaCutoff = 0;
        XSpriteMaskOrder xo = obj.AddComponent<XSpriteMaskOrder>();
        Transform tr = obj.GetComponent<Transform>();
        tr.localScale = new Vector3(5000, 5000,1);
        tr.localPosition = Vector3.zero;
        tr.SetAsFirstSibling();
        obj.layer = LayerMask.NameToLayer("UI");
    }

    

    [MenuItem("GameObject/XGUI/XToggle", priority = 9)]
    static public GameObject AddXToggle(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddToggle", menuCommand);
        go.name = "XToggle";
        Toggle tog = go.GetComponent<Toggle>();
        Graphic graphic = tog.graphic;
        Object.DestroyImmediate(go.GetComponent<Toggle>());
        XToggle xToggle = go.AddComponent<XToggle>();
        xToggle.transition = Selectable.Transition.None;
        xToggle.graphic = graphic;
        Text txt = go.transform.Find("Label").GetComponent<Text>();
        GameObject obj = txt.gameObject;
        DestroyImmediate(txt);
        XText xText = obj.transform.AddComponent<XText>();
        Font font = AssetDatabase.LoadAssetAtPath<Font>(font_path);
        if (font != null)
            xText.font = font;

        xText.alignment = TextAnchor.MiddleLeft;
        xText.fontSize = 24;

        Image img = go.transform.Find("Background").GetComponent<Image>();
        img.raycastTarget = false;
        img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI/Modules/Common/Images/Buttons/common_btn_13.png");
        img.SetNativeSize();

        Image img2 = go.transform.Find("Background/Checkmark").GetComponent<Image>();
        img2.raycastTarget = false;
        img2.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI/Modules/Common/Images/Buttons/common_btn_12.png");
        img2.SetNativeSize();

		XText text = go.transform.Find("Label").GetComponent<XText>();
		text.raycastTarget = false;

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 25);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);

        RectTransform rect2 = go.transform.Find("Background").GetComponent<RectTransform>();
        rect2.anchoredPosition = new Vector2(-67.5f, 0);
        rect2.sizeDelta = new Vector2(25, 25);
        rect2.anchorMax = new Vector2(0.5f, 0.5f);
        rect2.anchorMin = new Vector2(0.5f, 0.5f);

        RectTransform rect3 = go.transform.Find("Background/Checkmark").GetComponent<RectTransform>();
        rect3.sizeDelta = new Vector2(30, 26);
        rect3.anchorMax = new Vector2(0.5f, 0.5f);
        rect3.anchorMin = new Vector2(0.5f, 0.5f);

        RectTransform rect4 = go.transform.Find("Label").GetComponent<RectTransform>();
        rect4.anchoredPosition = new Vector2(14, 0);
        rect4.sizeDelta = new Vector2(132, 30.75f);
        rect4.anchorMax = new Vector2(0.5f, 0.5f);
        rect4.anchorMin = new Vector2(0.5f, 0.5f);

		go.AddComponent<XButtonScaleTween>();
		go.AddComponent<XNoDrawingView>();

        return go;
    }

    [MenuItem("GameObject/XGUI/XSlider", priority = 10)]
    static public void AddXSlider(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddSlider", menuCommand);
        Slider slider = go.GetComponent<Slider>();
        Graphic graphic = slider.targetGraphic;
        RectTransform fillRect = slider.fillRect;
        RectTransform handleRect = slider.handleRect;

        DestroyImmediate(slider);
        XSlider xSlider = go.AddComponent<XSlider>();
        xSlider.targetGraphic = graphic;
        xSlider.fillRect = fillRect;
        xSlider.handleRect = handleRect;
    }

    [MenuItem("GameObject/XGUI/XInputField", priority = 11)]
    static public void AddXInputField(MenuCommand menuCommand)
    {
        GameObject go = CallMenuOptions("AddInputField", menuCommand);
        InputField inputField = go.GetComponent<InputField>();
        Text text = inputField.textComponent;
        GameObject placeholder = inputField.placeholder.gameObject;

        GameObject obj = text.gameObject;
        DestroyImmediate(text);
        XText xText = obj.transform.AddComponent<XText>();
        Font font = AssetDatabase.LoadAssetAtPath<Font>(font_path);
        if (font != null)
            xText.font = font;

        xText.alignment = TextAnchor.MiddleCenter;
        xText.supportRichText = false;
        xText.color = new Color(0,0,0,1);
        xText.fontSize = 24;

        Text placeText = placeholder.GetComponent<Text>();
        DestroyImmediate(placeText);
        XText xPlaceText = placeholder.transform.AddComponent<XText>();
        if (font != null)
            xPlaceText.font = font;

        xPlaceText.alignment = TextAnchor.MiddleCenter;
        xPlaceText.fontStyle = FontStyle.Italic;
        xPlaceText.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        xPlaceText.fontSize = 24;

        DestroyImmediate(inputField);
        XInputField xInputField = go.AddComponent<XInputField>();
        xInputField.placeholder = xPlaceText;
        xInputField.textComponent = xText;
    }
}