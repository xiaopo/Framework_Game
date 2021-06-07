using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XGUI
{
    public class XScrollRollView_Item : MonoBehaviour
    {
        private XScrollRollView parent;
        [HideInInspector]
        public RectTransform rect;   
        private CanvasGroup m_CanvasGroup;

        private float m_TimeValue = 0;

        private RectTransform parentRect;
        /// <summary>
        /// 缩放值
        /// </summary>           
        private float m_ScaleValue = 0;

        private float panrentLeftPoint = 0, panrentRightPoint = 0;

        [SerializeField]
        private int m_DataIndex = 0;

        private UnityEvent m_UpdataRender = new UnityEvent(), m_OnClick = new UnityEvent(), m_OnDrag = new UnityEvent(), m_onDestroy = new UnityEvent();

        public float TimeValue
        {
            get
            {
                return m_TimeValue;
            }

            set
            {
                m_TimeValue = value;
            }
        }

        public int Index
        {
            get
            {
                return m_DataIndex;
            }

            set
            {
                m_DataIndex = value;
            }
        }

        public float ScaleValue
        {
            get
            {
                return m_ScaleValue;
            }
        }   

        public UnityEvent UpdataRender
        {
            get
            {
                return m_UpdataRender;
            }
        }

        public UnityEvent OnClick
        {
            get
            {
                return m_OnClick;
            }   
        }

        public UnityEvent onDestroy
        {
            get
            {
                return m_onDestroy;
            }          
        }

        public UnityEvent OnDrag
        {
            get
            {
                return m_OnDrag;
            }  
        }

        public void Init(XScrollRollView _parent)
        {
            rect = this.GetComponent<RectTransform>(); 
            parentRect = _parent.GetComponent<RectTransform>();
            m_CanvasGroup = GetComponent<CanvasGroup>() ? GetComponent<CanvasGroup>() : gameObject.AddComponent<CanvasGroup>();
            parent = _parent;

            panrentRightPoint = parentRect.anchoredPosition.x + parentRect.rect.width;
            panrentLeftPoint = parentRect.anchoredPosition.x;
        }

        public void Drag(float value)
        {
            m_TimeValue += value;
            Vector3 p = rect.localPosition;
            p.x = parent.GetPosition(TimeValue);
            rect.localPosition = p;

            m_CanvasGroup.alpha = parent.GetApa(TimeValue);
            m_ScaleValue = parent.GetScale(TimeValue);
            rect.localScale = new Vector3(m_ScaleValue, m_ScaleValue, 1);
            OnDrag.Invoke();
        }

        public void ClearEvent()
        {                      
            if (m_UpdataRender != null)                     
            {
                m_UpdataRender.RemoveAllListeners();
                m_UpdataRender = null;
            }

            if (m_OnClick != null)
            {
                m_OnClick.RemoveAllListeners();
                m_OnClick = null;
            }
        }

        private void OnDestroy()
        {
            ClearEvent();
            onDestroy.Invoke();
        }
    }
}

