using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace XGUI
{
    public class XScrollRect : ScrollRect
    {
        private bool m_ScrollToing = false;
        private float m_SmoothTime = 0.1f;
        private Vector2 m_ScrollTo = Vector2.zero;
        private Vector2 m_CVelocity = Vector2.zero;
        public UnityAction onEndDrag;
        public UnityAction onBeginDrag;
        public bool checkScrollValid = true;

        [SerializeField]
        private GameObject m_UpArrow;
        [SerializeField]
        private GameObject m_DownArrow;

        [SerializeField]
        private GameObject m_LeftArrow;

        [SerializeField]
        private GameObject m_RightArrow;

        [SerializeField]
        private XSlider m_VerticalSlider;
        [HideInInspector]
        public float PostionOffes = 0;    //滑动时候当前位置与前一帧位置的差
        private Vector2 OldTimePostion = Vector2.zero;

        private RectTransform m_rectTransform;

        public float UpLimit = 0.98f;

        public float DownLimit = 0.015f;

        private RectTransform rectTransform
        {
            get
            {
                if(m_rectTransform == null)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener((Vector2 offset) => {
                VisibleArrow();
                UpdateSilder();
            });
            VisibleArrow();
            InitSilder();
        }

        void Update()
        {
            if (!content)
                return;

            if (!m_ScrollToing)
                return;

            float deltaTime = Time.unscaledDeltaTime;
            content.anchoredPosition = Vector2.SmoothDamp(content.anchoredPosition, m_ScrollTo, ref m_CVelocity, m_SmoothTime, Mathf.Infinity, deltaTime);
            if ((content.anchoredPosition - m_ScrollTo).magnitude < 0.1f)
                this.m_ScrollToing = false;
        }


        public override void OnBeginDrag(PointerEventData eventData)
        {
            this.m_ScrollToing = false;
            base.OnBeginDrag(eventData);
            if(onBeginDrag != null)onBeginDrag.Invoke();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);                               
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);                   
            if (onEndDrag != null) onEndDrag.Invoke();  
        }

        public void ScrollToPosition(Vector2 value, float smoothTime = 0.1f)
        {
            this.m_SmoothTime = smoothTime;
            this.m_ScrollToing = true;
            this.m_ScrollTo = value;
            ScrollValid(ref this.m_ScrollTo);
        }


        public void ScrollToNextPage()
        {
            if (!content || !viewport)
                return;
            Vector2 target = new Vector2();
            target.x = content.anchoredPosition.x - viewRect.rect.width;
            target.y = content.anchoredPosition.y + viewRect.rect.height;

            ScrollValid(ref target);
            ScrollToPosition(target);
        }

        public void ScrollToPrevPage()
        {
            if (!content || !viewport)
                return;
            Vector2 target = new Vector2();
            target.x = content.anchoredPosition.x + viewRect.rect.width;
            target.y = content.anchoredPosition.y - viewRect.rect.height;
            ScrollValid(ref target);
            ScrollToPosition(target);
        }

        public void ScrollToTop(float smoothTime = 0.1f)
        {
            if (smoothTime == 0f)
            {
                this.verticalNormalizedPosition = 1;
            }
            else
            {
                Vector2 target = new Vector2();
                target.x = 0;
                target.y = 0;
                ScrollValid(ref target);
                ScrollToPosition(target, smoothTime);
            }
        }

        public void ScrollToBottom(float smoothTime = 0.1f)
        {
            if (smoothTime == 0f)
            {
                this.verticalNormalizedPosition = 0;
            }
            else
            {
                Vector2 target = new Vector2();
                target.x = -(content.sizeDelta.x - viewRect.rect.width);
                target.y = content.sizeDelta.y - viewRect.rect.height;
                ScrollValid(ref target);
                ScrollToPosition(target, smoothTime);
            }
        }

        private void ScrollValid(ref Vector2 value)
        {
            if (checkScrollValid)
            {
                value.y = Mathf.Clamp(value.y, 0, content.sizeDelta.y < viewRect.rect.height ? 0 : content.sizeDelta.y - viewRect.rect.height);
                value.x = Mathf.Clamp(value.x, content.sizeDelta.x < viewRect.rect.width ? 0 : -(Mathf.Abs(content.sizeDelta.x - viewRect.rect.width)), 0);
            }
        }

        private void VisibleArrow()
        {
            if (rectTransform.rect.height < content.rect.height)
            {
                SetDownArrow(!(this.verticalNormalizedPosition <= DownLimit));
                SetUpArrow(!(this.verticalNormalizedPosition >= UpLimit));
            }
            else
            {
                SetDownArrow(false);
                SetUpArrow(false);
            }

            if(rectTransform.rect.width < content.rect.width)
            {
                SetRightArrow(!(this.horizontalNormalizedPosition >= UpLimit));
                SetLeftArrow(!(this.horizontalNormalizedPosition <= DownLimit));
            }
            else
            {
                SetRightArrow(false);
                SetLeftArrow(false);
            }
        }

        private void SetUpArrow(bool res)
        {
            if (m_UpArrow)
            {
                m_UpArrow.SetActive(res);
            }
        }

        private void SetDownArrow(bool res)
        {
            if (m_DownArrow)
            {
                m_DownArrow.SetActive(res);
            }
        }

        private void SetLeftArrow(bool res)
        {
            if (m_LeftArrow)
            {
                m_LeftArrow.SetActive(res);
            }
        }

        private void SetRightArrow(bool res)
        {
            if (m_RightArrow)
            {
                m_RightArrow.SetActive(res);
            }
        }

        private void InitSilder()
        {
            if(m_VerticalSlider != null)
            {
                m_VerticalSlider.onValueChanged.AddListener((float value) =>
                {
                    this.verticalNormalizedPosition = value;
                });
                UpdateSilder();
            }
        }

        private void UpdateSilder()
        {
            if(m_VerticalSlider != null)
            {
                m_VerticalSlider.value = this.verticalNormalizedPosition;
            }
        }

        protected override void SetContentAnchoredPosition(Vector2 position)
        {
            base.SetContentAnchoredPosition(position);
            PostionOffes = Vector2.Distance(position, OldTimePostion);
            OldTimePostion = position;
        }                                                             

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (onValueChanged != null)
            {
                onValueChanged.RemoveAllListeners();
                onValueChanged = null;
            }
        }
    }
}
