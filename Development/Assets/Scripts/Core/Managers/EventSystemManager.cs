using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 游戏事件系统
/// </summary>
public class EventSystemManager : SingleBehaviourTemplate<EventSystemManager>
{
    protected EventSystem m_eventSystem;
    public EventSystem eventSystem { get { return m_eventSystem; } }
    protected StandaloneInputModule m_standaloneInputModule;
    public StandaloneInputModule standaloneInputModule { get { return m_standaloneInputModule; } }
    protected bool bInit = false;
    public void Init()
    {
        if (bInit) return;

        GameObject eventSys = GameObject.Find("EventSystem");
        if (eventSys != null)
            GameObject.Destroy(eventSys);

        m_eventSystem = this.gameObject.AddComponent<EventSystem>();
        m_standaloneInputModule = this.gameObject.AddComponent<StandaloneInputModule>();

        bInit = true;
    }

}