using AssetManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 游戏启动过程的UI系统
/// </summary>
public class LauncherGUIManager : SingleBehaviourTemplate<LauncherGUIManager>
{

    public RectTransform _rectTransform;

    protected int orderNumber = 0;
    protected override void OnInitialize()
    {

        Canvas canvas = this.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = this.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(ResolutionDefine.resolution_width, ResolutionDefine.resolution_hight);
        scaler.matchWidthOrHeight = 0;

        this.gameObject.AddComponent<GraphicRaycaster>();

        _rectTransform = this.gameObject.GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, 0);


    }

    private LauncherGUIAlrt m_guiArlt;
    public void Alert(string msg = "提示内容", UnityAction okcall = null, string title = "提示")
    {
        if (m_guiArlt == null)
        {
            m_guiArlt = new LauncherGUIAlrt(this);
            m_guiArlt.LoadAndInit();
        }
        m_guiArlt.Open(msg, okcall, title);
    }


    private LauncherGUIPage m_guiPage;

    public LauncherGUIPage UpdatePage
    {
        get
        {
            if (m_guiPage == null)
            {
                m_guiPage = new LauncherGUIPage(this);
                m_guiPage.LoadAndInit();
            }

            return m_guiPage;
        }
    }

    public void OnTop(LauncherUIBase view)
    {
        orderNumber++;
        view.SetOrder(orderNumber);
    }

    public void DestroyAll()
    {
        GameObject.Destroy(this.gameObject);

        m_guiPage = null;
        m_guiArlt = null;

    }


    private void Update()
    {
        
    }
}
