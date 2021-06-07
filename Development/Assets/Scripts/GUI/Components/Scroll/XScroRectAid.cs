using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XGUI;

public class XScroRectAid : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private XListView m_XListView;
    private XScrollRect m_XScrollRect;
    private int dataCount;
    public RectTransform Template;  //放大倍数
    public string Target;    //放大目标
    public bool IsOverlay;   //是否层叠
    public bool IsBuffer;  //是否缓冲变大
    public float mulriple;
    private float rectPoint;
    private float TemplaterectPoint;
    public UnityAction onBeginDragHandler;
    public UnityAction onEndDragHandler;
    private void Awake()
    {
        m_XListView = GetComponent<XListView>();
        m_XScrollRect = GetComponent<XScrollRect>();
        rectPoint = ((RectTransform)m_XListView.transform).rect.width / 2;
        TemplaterectPoint = Template.rect.width / 2;
        m_XScrollRect.checkScrollValid = true;
    }


    public void Update()
    {
        if (m_XListView.listItems != null)
        {
            Vector2 halfSize = m_XListView.viewRect.rect.size * 0.5f;
            foreach (var item in m_XListView.listItems)
            {
                Vector3 pos = m_XListView.viewRect.InverseTransformPoint(item.Value.transform.TransformPoint(item.Value.transform.rect.size * 0.5f));
                Transform Vg = item.Value.transform.Find(Target);
                Vector3 itemScale = Vg.localScale;
                float distance = Mathf.Abs(pos.x - halfSize.x);
                if (IsBuffer) 
                {
                    if (distance < 10)
                    {
                        Vector3 temp1 = new Vector3(mulriple, mulriple, mulriple);
                        Vg.localScale = Vector3.Lerp(itemScale, temp1, 0.2f);
                    }
                    else
                    {
                        float _mulriple = (mulriple - (mulriple - 1f) / halfSize.x * distance)-0.1f;
                        Vector3 temp2 = new Vector3(_mulriple, _mulriple, _mulriple);
                        Vg.localScale = Vector3.Lerp(itemScale, temp2, 0.2f);
                    }
                }
                else
                {
                    if (distance < 10)
                    {
                        Vector3 temp1 = new Vector3(mulriple, mulriple, mulriple);
                        Vg.localScale = Vector3.Lerp(itemScale, temp1, 0.2f); 
                    }
                    else
                    {
                        Vector3 temp2 = new Vector3(1, 1, 1);
                        Vg.localScale = Vector3.Lerp(itemScale, temp2, 0.2f); 
                    }
                }
                if (IsOverlay)
                {
                    if (distance < 10)
                    {
                        item.Value.transform.SetAsLastSibling();
                        int idx = item.Value.transform.GetSiblingIndex();
                        SetSiblingRight(idx, item.Key);
                        SetSiblingLeft(idx, item.Key);
                    }
                }
            }
        }
    }

    public void SetSiblingRight(int idx, int key)
    {
        XListView.ListItemRenderer item = null;
        bool isHas =  m_XListView.listItems.TryGetValue(key - 1,out item);
        if (isHas == false) { return; }
        else
        {
            item.transform.SetSiblingIndex(idx - 1);
            int idx1 = item.transform.GetSiblingIndex();
            SetSiblingRight(idx1, key - 1);
        }
    }

    public void SetSiblingLeft(int idx, int key)
    {
        XListView.ListItemRenderer item = null;
        bool isHas = m_XListView.listItems.TryGetValue(key + 1, out item);
        if (isHas == false) { return; }
        else
        {
            item.transform.SetSiblingIndex(idx - 1);
            int idx1 = item.transform.GetSiblingIndex();
            SetSiblingLeft(idx1, key + 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDragHandler != null)
        {
            onEndDragHandler.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler = null;
        }
        if (onEndDragHandler != null)
        {
            onEndDragHandler = null;
        }
    }
}
