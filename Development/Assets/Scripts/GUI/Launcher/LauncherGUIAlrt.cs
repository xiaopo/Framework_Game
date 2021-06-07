using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LauncherGUIAlrt : LauncherUIBase
{

    private Text m_text_msg;
    private Text m_text_title;
    private Button m_button;
    private UnityAction m_action;

    public LauncherGUIAlrt(LauncherGUIManager trans) : base(trans) 
    {
        resourceName = "Alert_page/res_alert_page";

    }

    protected override void Init()
    {
        m_text_msg = this.GetBindComponet<Text>("Text_msg");
        m_text_msg.color = Color.red;
        m_text_title = this.GetBindComponet<Text>("Text_title");

        m_button = this.GetBindComponet<Button>("Button_ok");
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        m_action?.Invoke();
        m_action = null;
        Close();
    }

    public void Open(string msg = "提示内容", UnityAction okcall = null, string title = "提示")
    {

        m_action = okcall;

        m_text_msg.text = msg;
        m_text_title.text = title;

        this.gameObject.SetActive(true);

        this.manager.OnTop(this);
    }

  
}
