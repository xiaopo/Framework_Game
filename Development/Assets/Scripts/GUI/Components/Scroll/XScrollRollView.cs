using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace XGUI
{
    public class XScrollRollView : XView, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        /// <summary>
        /// 坐标曲线
        /// </summary>                              
        [SerializeField]
        private AnimationCurve PositionCurve = new AnimationCurve(), ScaleCurve = new AnimationCurve(), ApaCurve = new AnimationCurve();

        [SerializeField]
        private GameObject prefab = null;

        /// 大小                   
        [SerializeField]
        private float m_AnimaSpeed = 0.5f;
        private float scrollSpeed = 0;

        private float MaxScale = 1;

        [SerializeField]
        [Range(0, 1)]
        private float m_Spacing = 0.2f, m_StartValue = 0.1f;
        /// <summary>
        ///小于vmian 达到最左，大于vmax达到最右
        /// </summary>
        private float VMax = 1.0f;

        private List<XScrollRollView_Item> m_Items = new List<XScrollRollView_Item>();
        /// <summary>
        /// 宽度
        /// </summary>
        [SerializeField]
        private float Width;
        /// <summary>
        /// 计算值
        /// </summary>
        private Vector2 start_point, add_vect;

        /// <summary>
        /// 动画状态
        /// </summary>
        private bool playerAnima = false;
                             
        private int lastIndex = 0,firstIndex = 0;

        private bool recalculate = false;

        private float AddV = 0, Vk = 0, CurrentV = 0, Vtotal = 0, VT = 0;

        private XScrollRollView_Item Current;          

        [System.Serializable]
        public class ListEvent : UnityEvent<XScrollRollView_Item> { }

        private ListEvent m_CreatEvent = new ListEvent(), m_SelectEvent = new ListEvent();          

        public int dataCount = 0;                     

        public List<XScrollRollView_Item> Items
        {
            get
            {
                return m_Items;
            }    
        }

        public ListEvent CreatEvent{ get { return m_CreatEvent;} }

        public ListEvent SelectEvent{get{ return m_SelectEvent;}}

        public override void Start()
        {
            Init();
        }

        private void Init()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Width = rectTransform.rect.width;  

            int InstantCount = Mathf.CeilToInt(1 / m_Spacing) + 2;

            if (InstantCount < 5)
            {
                VMax = m_StartValue + 4 * m_Spacing;
            }
            else
            {
                VMax = m_StartValue + (InstantCount - 1) * m_Spacing;
            }
            PositionCurve.AddKey(VMax, m_StartValue + InstantCount * m_Spacing);

            InstantiateItems(InstantCount);

            //if (m_SelectEvent != null)
            //{
            //    m_SelectEvent.Invoke(Current);
            //}
        }

        private void InstantiateItems(int InstantCount)
        {
            if (!prefab) return;

            for (int i = 0; i < InstantCount; i++)
            {

                GameObject item = GameObject.Instantiate(prefab, transform);
                XScrollRollView_Item Roll_item = item.GetComponent<XScrollRollView_Item>() ? item.GetComponent<XScrollRollView_Item>() : item.AddComponent<XScrollRollView_Item>();

                item.transform.name = "item_" + i;
                m_Items.Add(Roll_item);

                Roll_item.Init(this);
                Roll_item.Index = i;

                m_CreatEvent.Invoke(Roll_item);

                float initTimeValue = m_StartValue + i * m_Spacing;

                Roll_item.Drag(initTimeValue);

                if (initTimeValue - 0.5 < 0.05f)
                {
                    Current = Roll_item;
                }

                lastIndex = i;
            }
            if (prefab.activeSelf)
                prefab.SetActive(false);
        }

        public void ForceRefresh()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Index = firstIndex + i;
            }
            if (!recalculate)
                recalculate = true;
        }      

        public override void ClearEvent()
        {
            base.ClearEvent();
            if (m_CreatEvent != null)
            {
                m_CreatEvent.RemoveAllListeners();
                m_CreatEvent = null;
            }

            if (m_SelectEvent != null)
            {
                m_SelectEvent.RemoveAllListeners();
                m_SelectEvent = null;
            }
        }

            public void OnBeginDrag(PointerEventData eventData)
        {
            start_point = eventData.position;
            add_vect = Vector3.zero;
            playerAnima = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            add_vect = eventData.position - start_point;
            float v = eventData.delta.x / Width;

            for (int i = 0; i < m_Items.Count; i++)
            {
                m_Items[i].Drag(v);
            }
            Check(v);
        }


        public void Check(float _v)
        {
            List<XScrollRollView_Item> GotoFirstItems = new List<XScrollRollView_Item>(), GotoLaserItems = new List<XScrollRollView_Item>();
            if (_v < 0)
            {//向左运动
                for (int i = 0; i < m_Items.Count; i++)
                {
                    if (m_Items[i].TimeValue < 0)
                    {
                        GotoLaserItems.Add(m_Items[i]);     
                        if (firstIndex >= dataCount - 1)
                            firstIndex = 0;
                        else
                            firstIndex += 1;

                        if (lastIndex >= dataCount - 1)
                            lastIndex = 0;
                        else
                            lastIndex += 1;

                        m_Items[i].Index = lastIndex;      
                        m_Items[i].UpdataRender.Invoke();     
                    }
                }
                if (GotoLaserItems.Count > 0)
                {
                    for (int i = 0; i < GotoLaserItems.Count; i++)
                    {
                        GotoLaserItems[i].TimeValue = m_Items[m_Items.Count - 1].TimeValue + m_Spacing;
                        m_Items.Remove(GotoLaserItems[i]);
                        m_Items.Add(GotoLaserItems[i]);
                    }
                }
            }
            else if (_v > 0)
            {//向右运动，需要把右边的放到前面来

                for (int i = m_Items.Count - 1; i > 0; i--)
                {
                    if (m_Items[i].TimeValue >= VMax)
                    {
                        GotoFirstItems.Add(m_Items[i]);  
                        if (lastIndex <= 0)
                            lastIndex = dataCount - 1;
                        else
                            lastIndex -= 1;

                        if (firstIndex <= 0)
                            firstIndex = dataCount - 1;
                        else
                            firstIndex -= 1;

                        m_Items[i].Index = firstIndex;
                        m_Items[i].UpdataRender.Invoke();
                    }
                }
                if (GotoFirstItems.Count > 0)
                {
                    for (int i = 0; i < GotoFirstItems.Count; i++)
                    {
                        GotoFirstItems[i].TimeValue = m_Items[0].TimeValue - m_Spacing;
                        m_Items.Remove(GotoFirstItems[i]);
                        m_Items.Insert(0, GotoFirstItems[i]);
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float k = 0, v1 = 0;
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].TimeValue >= m_StartValue)
                {
                    v1 = (m_Items[i].TimeValue - m_StartValue) % m_Spacing;
                    if (add_vect.x >= 0)
                    {
                        k = m_Spacing - v1;
                    }
                    else
                    {
                        k = v1 * -1;
                    }
                    break;
                }
            }

            add_vect = Vector3.zero;
            AnimToEnd(k);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (add_vect.sqrMagnitude <= 1)
            {
                XScrollRollView_Item rollItem = eventData.pointerPressRaycast.gameObject.GetComponent<XScrollRollView_Item>();
                if (rollItem != null)
                {
                    rollItem.OnClick.Invoke();

                    float k = rollItem.TimeValue;
                    k = 0.5f - k;
                    AnimToEnd(k);
                }

            }
        }

        public void ScrollToIndex(int index,float speed)
        {
            scrollSpeed = speed;
            index = index - firstIndex;
            float k = 0.5f - (index * m_Spacing + m_StartValue);     
            AnimToEnd(k);
        }

        public float GetApa(float v)
        {
            return ApaCurve.Evaluate(v);
        }
        public float GetPosition(float v)
        {
            return PositionCurve.Evaluate(v) * Width;
        }
        public float GetScale(float v)
        {
            return ScaleCurve.Evaluate(v) * MaxScale;
        }


        private List<XScrollRollView_Item> SortValues = new List<XScrollRollView_Item>();
        public void LateUpdate()
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].TimeValue >= 0.1f && m_Items[i].TimeValue <= 0.9f)
                {
                    int index = 0;
                    for (int j = 0; j < SortValues.Count; j++)
                    {
                        if (m_Items[i].ScaleValue >= SortValues[j].ScaleValue)
                        {
                            index = j + 1;
                        }
                    }

                    SortValues.Insert(index, m_Items[i]);
                }
            }

            for (int k = 0; k < SortValues.Count; k++)
            {
                SortValues[k].rect.SetSiblingIndex(k);
            }
            SortValues.Clear();
        }     

        private void AnimToEnd(float k)
        {
            AddV = k;
            if (AddV > 0)
            {
                Vk = 1;
            }
            else if (AddV < 0)
            {
                Vk = -1;
            }
            else
            {
                return;
            }
            Vtotal = 0;
            playerAnima = true;                    
        }

        void Update()
        {
            if (playerAnima)
            {
                float speed = scrollSpeed == 0 ? m_AnimaSpeed : scrollSpeed;
                CurrentV = Time.deltaTime * speed * Vk;
                VT = Vtotal + CurrentV;
                if (Vk > 0 && VT >= AddV) { playerAnima = false; CurrentV = AddV - Vtotal; }
                if (Vk < 0 && VT <= AddV) { playerAnima = false; CurrentV = AddV - Vtotal; }
                //==============
                for (int i = 0; i < m_Items.Count; i++)
                {
                    m_Items[i].Drag(CurrentV);
                    if (m_Items[i].TimeValue - 0.5 < 0.05f)
                    {
                        Current = m_Items[i];
                    }
                }
                Check(CurrentV);
                Vtotal = VT;

                if (!playerAnima)
                {
                    if (m_SelectEvent != null) { m_SelectEvent.Invoke(Current); }
                    scrollSpeed = 0;
                }
            }

            if (recalculate)
            {
                recalculate = false;
                for (int i = 0; i < m_Items.Count; i++)
                {
                    m_Items[i].UpdataRender.Invoke();
                }
            }
        }

    }
}
