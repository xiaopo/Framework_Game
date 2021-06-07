using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.UI;
using XGUI;
using Object = UnityEngine.Object;

public static class UnityExtension
{
    public static void SetLayer(this Transform t, int layer, bool allChild = false)
    {
        t.gameObject.layer = layer;
        if (allChild)
        {
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                SetLayer(child, layer, allChild);
            }
        }
    }

    public static void SetLayer(this Transform t, string layerName, bool allChild = false)
    {
        t.SetLayer(LayerMask.NameToLayer(layerName), allChild);
    }


    /// <summary>
    /// 查找组件
    /// </summary>
    /// <param name="t"></param>
    /// <param name="type">类型</param>
    /// <param name="relative">相对路径</param>
    public static Object FindComponent(this Transform t, System.Type type = null, string relative = null)
    {
        Transform target = !string.IsNullOrEmpty(relative) ? t.Find(relative) : t;
        if (target)
        {
            if (type == typeof(GameObject))
                return target.gameObject;
            else
                return type != null ? target.GetComponent(type) : target;
        }

        return null;
    }

    public static Object FindComponent(this Transform t, string type = null, string relative = null)
    {
        Transform target = !string.IsNullOrEmpty(relative) ? t.Find(relative) : t;

        if (target)
            return !string.IsNullOrEmpty(type) ? target.GetComponent(type) : target;
        return null;
    }

    public static Transform[] FindAllChilds(this Transform t, bool isFindEvery = false)
    {
        Transform[] childs = null;
        if (isFindEvery)
        {
            childs = t.GetComponents<Transform>();
        }
        else
        {
            int count = t.childCount;
            childs = new Transform[count];
            for (int i = 0; i < count; i++)
                childs[i] = t.GetChild(i);
        }
        return childs;
    }

    public static void SetParentEx(this Transform t, Transform parent)
    {
        Vector3 opos = t.localPosition;
        Vector3 oscale = t.localScale;
        Vector3 oangles = t.localEulerAngles;
        t.SetParent(parent);
        t.localPosition = opos;
        t.localScale = oscale;
        t.localRotation = Quaternion.Euler(oangles);
    }

    public static void ResetTRS(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
    }

    public static void SetParentOEx(this Transform t, Transform parent)
    {
        t.SetParent(parent);
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
    }

    public static void SetParentEx(this Transform t, Transform parent, Vector3 pos, Vector3 angles, Vector3 scale)
    {
        t.SetParent(parent);
        t.localPosition = pos;
        t.localScale = scale;
        t.localRotation = Quaternion.Euler(angles);
    }

	public static void SetParentZero(this RectTransform t,RectTransform parent)
	{
		t.SetParent(parent);
		t.offsetMin = Vector2.zero;
		t.offsetMax = Vector2.zero;

	}
    public static void LocalScaleEx(this Transform t, float x, float y, float z)
    {
        t.localScale = new Vector3(x, y, z);
    }

    public static void LocalPositionEx(this Transform t, float x, float y, float z)
    {
        t.localPosition = new Vector3(x, y, z);
    }

    public static void PositionEx(this Transform t, float x, float y, float z)
    {
        t.position = new Vector3(x, y, z);
    }

    public static float GetPosition(this Transform t, out float y, out float z)
    {
        Vector3 position = t.position;
        y = position.y;
        z = position.z;

        return t.position.x;
    }


    public static void SetLocalRotation(this Transform t, float x, float y, float z)
    {
        t.localRotation = Quaternion.Euler(x, y, z);
    }

    public static void OpenMask(this Camera camera, params int[] layers)
    {
        camera.cullingMask = SUtility.AddMask(camera.cullingMask, layers);
    }

    public static void OpenMask(this Camera camera, params string[] layersNames)
    {
        for (int i = 0; i < layersNames.Length; i++)
            camera.cullingMask |= 1 << LayerMask.NameToLayer(layersNames[i]);
    }

    public static void CloseMask(this Camera camera, params int[] layers)
    {
        camera.cullingMask = SUtility.SubMask(camera.cullingMask, layers);
    }

    public static void CloseMask(this Camera camera, params string[] layersNames)
    {
        for (int i = 0; i < layersNames.Length; i++)
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer(layersNames[i]));
    }



    public static void SetMask(this Camera camera, params string[] layersNames)
    {
        camera.cullingMask = LayerMask.GetMask(layersNames);
    }


    public static void AllMask(this Camera camera, bool enable)
    {
        camera.cullingMask = enable ? -1 : ~-1;
    }


    public static bool IsNull(this UnityEngine.Object obj)
    {
        return obj == null || obj.Equals(null);
    }

    public static int ToInt(this Enum obj)
    {
        return System.Convert.ToInt32(obj);
    }

    public static void SetActive(this Component c, bool value)
    {
        c.gameObject.SetActive(value);
    }

    public static T AddComponent<T>(this Component c) where T : Component
    {
        return c.gameObject.AddComponent<T>();
    }

    public static Component AddComponent(this Component c, System.Type type)
    {
        return c.gameObject.AddComponent(type);
    }


    public static T TryGetComponent<T>(this GameObject c) where T : Component
    {
        T component = c.GetComponent<T>();
        if (component == null)
            component = c.AddComponent<T>();
        return component;
    }

    public static T TryGetComponent<T>(this Component c) where T : Component
    {
        T component = c.GetComponent<T>();
        if (component == null)
            component = c.AddComponent<T>();
        return component;
    }

    public static Component TryGetComponent(this Component c, System.Type type)
    {
        Component component = c.GetComponent(type);
        if (component == null)
            component = c.AddComponent(type);
        return component;
    }

    public static Component TryGetComponent(this Component c, string type)
    {
        Component component = c.GetComponent(type);
        if (component == null)
            component = c.AddComponent(System.Type.GetType(type));
        return component;
    }

    public static Component TryGetComponent(this GameObject c, System.Type type)
    {
        Component component = c.GetComponent(type);
        if (component == null)
            component = c.AddComponent(type);
        return component;
    }

    public static Component TryGetComponent(this GameObject c, string type)
    {
        Component component = c.GetComponent(type);
        if (component == null)
            component = c.AddComponent(System.Type.GetType(type));
        return component;
    }

    public static T GetComponentInChildrenDepth<T>(this GameObject go, bool depth = false, bool includeInactive = false) where T : Component
    {
        if (depth)
            return go.GetComponentInChildren<T>(includeInactive);

        Transform ts = go.transform;
        for (int i = 0; i < ts.childCount; i++)
        {
            T t = ts.GetChild(i).GetComponent<T>();
            if (!t.IsNull()) return t;
        }

        return null;
    }


    public static void IteratorChild(this Transform ts, UnityAction<Transform> func, bool depth = false)
    {
        int count = ts.childCount;
        if (count < 1) return;
        for (int i = 0; i < count; i++)
        {
            Transform child = ts.GetChild(i);
            func.Invoke(child);
            if (depth && child.childCount > 0) child.IteratorChild(func, depth);
        }
    }

    public static void AddCanvas(this GameObject go, int sortingOrder)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        if (canvas == null || canvas.IsNull())
        {
            canvas = go.AddComponent<Canvas>();
            canvas.overrideSorting = true;
        }

        CanvasScaler canvasScaler = go.GetComponent<CanvasScaler>();
        if (canvasScaler == null || canvasScaler.IsNull())
        {
            go.AddComponent<CanvasScaler>();

        }
        GraphicRaycaster graphicRaycaster = go.GetComponent<GraphicRaycaster>();
        if (graphicRaycaster == null || graphicRaycaster.IsNull())
        {
            go.AddComponent<GraphicRaycaster>();
        }
        canvas.sortingOrder = sortingOrder;
    }


     
    //更所有
    public static void UpdateAllChildSortingOrder(this GameObject go)
    {
        Profiler.BeginSample("UnityExtension.UpdateAllChildSortingOrder");
        List<XSortingOrder> result = ListPool<XSortingOrder>.Get();
        List<XSpriteMaskOrder> maskOrders = ListPool<XSpriteMaskOrder>.Get();
        go.GetComponentsInChildren<XSortingOrder>(true, result);
        go.GetComponentsInChildren<XSpriteMaskOrder>(true, maskOrders);
        foreach (XSortingOrder item in result)
            item.UpdateSortingOrder();
        foreach (XSpriteMaskOrder maskOrder in maskOrders)
            maskOrder.UpdateSortingOrder(); 
        ListPool<XSortingOrder>.Release(result);
        ListPool<XSpriteMaskOrder>.Release(maskOrders); 
        Profiler.EndSample();
    }
}
