using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using XGUI;

public class XPassEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler , IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject m_FirstHitGObj;

    [SerializeField]
    private int m_blockInstanceID = 0;

    [SerializeField]
    private bool m_debug = false;

    bool m_HasPassEvent = false;

    [Tooltip("是否拖动")]
    [SerializeField]
    bool isAll = false;

    bool isDraging = false;

    [SerializeField]
    bool isAdpat = true;

    public class CilckEvent : UnityEvent { }
    public CilckEvent onClick = new CilckEvent();

    [SerializeField]
    bool isPreventClick = false;

    //是否穿透
    [Tooltip("是否穿透")]
    [SerializeField]
    bool isPenetrate = true;

    public bool penetrate
    {
        get { return isPenetrate; }
        set { isPenetrate = value; }
    }

    private void Start()
    {
        if (isAdpat)
            this.transform.localScale = Vector3.one / UIAdpat.AdpTargetScale;

    }

    public void OnEnable()
    {
        isDraging = false;
    }

    private GameObject GetHitObjet(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        GameObject current = eventData.pointerCurrentRaycast.gameObject;

        GameObject hitObjt = null;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                if(!isPenetrate && m_blockInstanceID == results[i].gameObject.GetInstanceID())
                {
                    return null;
                }
                if (m_blockInstanceID == 0 || m_blockInstanceID != results[i].gameObject.GetInstanceID())
                {
                    hitObjt = results[i].gameObject;
                    break;
                }
            }
        }
        results.Clear();

        return hitObjt;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject hitObj = GetHitObjet(eventData);
        if(hitObj != null)
        {
            m_FirstHitGObj = hitObj;
            XGUI.XButtonScaleTween scaleTween = hitObj.GetComponent<XGUI.XButtonScaleTween>();
            if (scaleTween != null)
            {
                scaleTween.OnPointerDown(eventData);
            }
        }
        //PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //恢复首次点击按钮的大小
        GameObject hitObj = m_FirstHitGObj;

        if (hitObj != null)
        {
            XGUI.XButtonScaleTween scaleTween = hitObj.GetComponent<XGUI.XButtonScaleTween>();
            if (scaleTween != null)
            {
                scaleTween.OnPointerUp(eventData);

            }
        }
        //PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        if (m_HasPassEvent)
            return;
        m_HasPassEvent = true;

        GameObject hitObj = GetHitObjet(data);
        
        
        if (m_FirstHitGObj != null && m_FirstHitGObj != hitObj)
        {
            m_FirstHitGObj = null;
            m_HasPassEvent = false;
            if (m_debug)
            {
                Debug.Log("按下与松开不是同一个对象，不传递事件");
            }
            return;
        }
       
        if(hitObj != null && !isDraging && hitObj.GetComponent<XTouchClick>() == null)
        {
            if (m_debug)
            {
                Debug.Log("你的组件的名字：" + hitObj.name);
            }
            TimerManager.AddCoroutine(ExecuteEvent(hitObj, data, function));
            //StartCoroutine(ExecuteEvent(hitObj, data, function));
        }

        if (!isPreventClick || (hitObj != null && hitObj.GetComponent<XPreventClick>() == null))
            onClick.Invoke();
        m_FirstHitGObj = null;
        m_HasPassEvent = false;

    }

    private IEnumerator ExecuteEvent<T>(GameObject obj,PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        yield return new WaitForEndOfFrame();
        if (m_debug)
        {
            Debug.Log("穿透组件的名字：" + obj.name);
        }
        ExecuteEvents.Execute(obj, data, function);
    }

    public void SetBlockInstanceID(int InstanceID)
    {
        m_blockInstanceID = InstanceID;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAll)
        {
            isDraging = true;
            PassHitObjet(eventData, ExecuteEvents.beginDragHandler);
        }
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAll)
        {
            isDraging = false;
            PassHitObjet(eventData, ExecuteEvents.endDragHandler);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAll)
            PassHitObjet(eventData, ExecuteEvents.dragHandler);
    }
    XScrollRect scroll;
    private void PassHitObjet<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        GameObject current = eventData.pointerCurrentRaycast.gameObject;

        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                if (m_blockInstanceID == 0 || m_blockInstanceID != results[i].gameObject.GetInstanceID())
                {
                    scroll = results[i].gameObject.GetComponent<XScrollRect>();
                    if (scroll != null)
                    {
                        ExecuteEvents.Execute(results[i].gameObject, eventData, function);
                    }
                }
            }
        }
        results.Clear();

    }

    private void OnDestroy()
    {
        onClick.RemoveAllListeners();
        onClick = null;
    }
}
