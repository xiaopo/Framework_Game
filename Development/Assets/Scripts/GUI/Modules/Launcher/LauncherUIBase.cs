using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XGUI;
using Object = UnityEngine.Object;
public class LauncherUIBase
{
    protected LauncherGUIManager manager;
    protected GameObject gameObject;
    protected Canvas canvase;
    protected XView x_view;
    private bool m_init;

    protected string resourceName;//在Resource下

    public LauncherUIBase(LauncherGUIManager pp)
    {
        manager = pp;
    }

    public virtual void LoadAndInit()
    {
        if (m_init) return;
        //string error;
        //string path = Path.Combine(AssetDefine.streamingAssetsPath, assetBundleName);
        //AssetBundle bundle = SFileUtility.ReadStreamingAssetBundle(path, out error);

        //GameObject RawObj = bundle.LoadAsset<GameObject>(prefabName);
        GameObject RawObj = Resources.Load<GameObject>(resourceName);
        gameObject = GameObject.Instantiate(RawObj, manager._rectTransform);

        x_view = gameObject.GetComponent<XView>();
        canvase = gameObject.TryGetComponent<Canvas>();
        Init();
        m_init = true;
    }

    public T GetBindComponet<T>(string bName)where T:Object
    {
        if (this.x_view == null) return default(T);

        return x_view.GetBindComponet<T>(bName);
    }

    protected virtual void Init()
    {

    }

    public void SetOrder(int order)
    {
        canvase.overrideSorting = true;
        canvase.sortingOrder = order;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}