using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XGUI;


public static class UGUIExtension
{
    public static void AddClickEvent(this Button button, UnityAction action) { button.onClick.AddListener(action); }
    public static void RemoveClickEvent(this Button button, UnityAction action) { button.onClick.RemoveListener(action); }
    public static void ClearClickEvent(this Button button) { button.onClick.RemoveAllListeners(); }
    public static void OnSelectEvent(this XButton button, UnityAction action) { button.onSelect.AddListener(action); }


    static Vector3[] s_TempCorners = new Vector3[4];
    /// <summary>
    /// 将RectTransform  转为屏幕坐标
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="idx">
    /// 顺时针
    /// 
    /// [1] [2]
    /// [0] [3]
    /// </param>
    /// <returns></returns>
    public static Vector3 GetScreenPointCorner(this RectTransform transform, int idx)
    {
        idx = Mathf.Clamp(idx, 0, 3);
        transform.GetWorldCorners(s_TempCorners);
        Vector3 result = s_TempCorners[idx];
        if (GameCameraUtiliy.uiCamera != null)
            result = GameCameraUtiliy.uiCamera.WorldToScreenPoint(result);
        return result;
    }


    /// <summary>
    /// 本地空间转屏幕空间
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="local"></param>
    /// <returns></returns>
    public static Vector3 GetLocalToScreenPoint(this RectTransform transform, Vector3 local)
    {
        local = transform.TransformPoint(local);
        local = GameCameraUtiliy.uiCamera.WorldToScreenPoint(local);

        return local;
    }

    /// <summary>
    /// 屏幕空间转本地空间
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="local"></param>
    /// <returns></returns>
    public static Vector3 GetScreenToLocalPoint(this RectTransform transform, Vector3 local)
    {
        Vector2 localPoint = local;
        if (GameCameraUtiliy.uiCamera != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform, local, GameCameraUtiliy.uiCamera, out localPoint);
        }

        //local = transform.InverseTransformPoint(XCamera.guiCamera.ScreenToWorldPoint(local));
        return localPoint;
    }

    public static void SetAnchorCenterSize(this RectTransform transform, float x, float y, float width, float height)
    {
        transform.anchorMin = transform.anchorMax = Vector2.one * 0.5f;
        transform.anchoredPosition = new Vector2(x, y);
        transform.sizeDelta = new Vector2(width, height);
    }

    public static Material s_GrayMaterial = null;
    
    public static void SetGray(this Graphic graphic, bool isGary,float saturation = 0f)
    {
        if (!(graphic is Text))
        {
            if (IsMaskGray(graphic, isGary))
            {
                return;
            }
            if (isGary)
            {
                if (s_GrayMaterial == null)
                {
                    Shader sh = ShaderHandler.GetShader("X_Shader/G_GUI/Gray");
                    if (sh != null)
                    {
                        s_GrayMaterial = new Material(sh);
                    }
                }
                graphic.material = s_GrayMaterial;
                graphic.material.SetFloat("_Saturation", saturation);
            }
            else
            {
                graphic.material = null;
            }
        }else
        {
            graphic.SetTextGray(isGary);
        }
    }

    static void SetTextGray(this Graphic graphic, bool isGary)
    {
        XText text = graphic as XText;
        text.SetGray(isGary);
    }

    public static bool IsMaskGray(Graphic graphic, bool isGary)
    {
        if (graphic.material && graphic.material.shader.name == "X_Shader/G_GUI/MaskGray")
        {
            graphic.material.SetFloat("_Saturation", isGary ? 0 : 1);
            return true;
        }
        return false;
    }

    public static void SetGray(this Selectable button, bool isGary, bool isInCludeText = true)
    {
        Graphic[] GraphicArray = button.transform.GetComponentsInChildren<Graphic>(true);
        for (int i = 0; i < GraphicArray.Length; i++)
        {
            if (isInCludeText || !(GraphicArray[i] is Text))
            {
                GraphicArray[i].SetGray(isGary);
            }
        }
    }

    //制灰全部子对象
    public static void SetChildrenGray(this RectTransform transform, bool isGary,float saturation = 0f)
    {
        Graphic[] GraphicArray = transform.GetComponentsInChildren<Graphic>(false);
        for (int i = 0; i < GraphicArray.Length; i++)
        {
            SetGray(GraphicArray[i], isGary, saturation);
        }
    }

    private static Material s_DimMaterial = null;

    //设置模糊
    public static void SetDim(this Graphic graphic, bool isGary)
    {
        if (isGary)
        {
            if (s_DimMaterial == null)
            {
                Shader sh = ShaderHandler.GetShader("X_Shader/G_GUI/GaussianBlur");
                if (sh != null)
                {
                    s_DimMaterial = new Material(sh);
                }
            }
            graphic.material = s_DimMaterial;

        }
        else
        {
            graphic.material = null;
        }
    }

    public static void SetSize(this RectTransform rect,float width,float height)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}
