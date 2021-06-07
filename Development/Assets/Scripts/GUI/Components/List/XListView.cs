using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XGUI;

namespace XGUI
{
    public class XListView : XView, XIEvent
    {
        public enum ListLayout
        {
            Horizontal, //水平
            Vertical,   //垂直
            Grid,       //格子
            Custom,     //自定义
        }

        public enum GridConstraint
        {
            ColumnCount, //列
            RowCount,    //行
        }

        private enum CustomLayout 
        {
            Horizontal, //水平
            Vertical,   //垂直
        };

        //全局分帧创建控制
        public static int GloablFrameCreateCount = 0;


        [SerializeField]
        private XScrollRect m_ScrollRect;
        public XGUI.XScrollRect xScrollRect { get { return m_ScrollRect; } set { m_ScrollRect = value; } }

        [System.Serializable]
        public class ListEvent : UnityEvent<ListItemRenderer> { }
        public class ListEventLua : UnityEvent<int> { }


        [SerializeField]
        private ListEvent m_OnUpdateRenderer = new ListEvent();
        private ListEvent m_OnRecycleRenderer = new ListEvent();
        private ListEvent m_OnCreateRenderer = new ListEvent();
        private ListEventLua m_OnUpdatePost = new ListEventLua();
        private ListEventLua m_OnUpdateRendererLua = new ListEventLua();
        private ListEventLua m_OnRecycleRendererLua = new ListEventLua();


        [SerializeField]
        private GridConstraint m_GridConstraint = GridConstraint.ColumnCount;
        [SerializeField]
        private int m_GridConstraintCount = 2;

        [SerializeField]
        private ListLayout m_ListLayout = ListLayout.Vertical;
        [SerializeField]
        private CustomLayout m_customLayout = CustomLayout.Horizontal;

        private bool m_Recalculate = false;
        private bool m_IsScroll = false;
        private Vector2 m_ScrollPos = Vector2.zero;
        private float m_ScrollTime = 0f;
        [SerializeField]
        private bool m_ControllChildSize = false;
        [SerializeField]
        private Vector2 m_CellSize = new Vector2(100, 100);
        [SerializeField]
        private int m_DataCount = 0;
        private Vector2 m_ItemSize = Vector2.zero;
        private Vector3 m_ContentSize;
        private Vector2 m_ScrollOffset = Vector2.zero;

        //分帧创建数据 <=0 不分帧 否则按设置的个数
        [SerializeField]
        private int m_WaitCreateCount = 0;

        //慎用 一亘启动将不再再循环利用而是一直创建
        [SerializeField]
        private bool m_IsCacheItem = false;

        [SerializeField]
        private bool m_RecycleDeActive = false;

        [SerializeField]
        private Vector2 m_Spacing = Vector2.zero;
        private Dictionary<int, ListItemRenderer> m_ListItems;
        private List<ListItemRenderer> m_RecyclePool;
        private List<ListItemRenderer> m_Temp;
        private Dictionary<int, ListItemRenderer> m_WaitCreateList;

        private RectTransform m_Viewport;
        private RectTransform m_Content;
        private List<RectTransform> m_CustomParents;
        private LayoutGroup m_ContentLayoutGroup;

        //临时排序
        private static List<int> s_TempSortKeys = new List<int>();


        [SerializeField]
        private GameObject m_Template;
        public GameObject template { get { return m_Template; } set { if (m_Template == value) return; ClearAll(); m_Template = value; } }
        [SerializeField]
        private string m_TemplateAsset;

        //是否合并UI DrawCall
        [SerializeField]
        private bool m_IsBatch = false;

        public string templateAsset { get { return m_TemplateAsset; } set { if (m_TemplateAsset == value) return; m_TemplateAsset = value; } }
        public ListLayout layout { get { return m_ListLayout; } set { if (m_ListLayout != value) { m_ListLayout = value; ForceRefresh(); } } }
        public float horizontalSpacing { get { return m_Spacing.x; } set { if (m_Spacing.x != value) { m_Spacing.x = value; ForceRefresh(); } } }
        public float verticalSpacing { get { return m_Spacing.y; } set { if (m_Spacing.y != value) { m_Spacing.y = value; ForceRefresh(); } } }
        public int dataCount { get { return m_DataCount; } set { if (m_DataCount != value) { m_DataCount = value; ForceRefresh(); } } }
        public Dictionary<int, ListItemRenderer> listItems { get { return m_ListItems; } }
        public Vector2 scrollOffset { get { return m_ScrollOffset; } }

        public RectTransform content { get { return m_Content; } }
        public RectTransform viewRect { get { return m_Viewport; } }
        public Vector2 velocity { get { return this.m_ScrollRect ? this.m_ScrollRect.velocity : Vector2.zero; } set { if (this.m_ScrollRect) this.m_ScrollRect.velocity = value; } }
        public ListEvent onUpdateRenderer { get { return m_OnUpdateRenderer; } }
        public ListEventLua onUpdateRendererLua { get { return m_OnUpdateRendererLua; } }
        public ListEvent onRecycleRenderer { get { return m_OnRecycleRenderer; } }
        public ListEventLua onRecycleRendererLua { get { return m_OnRecycleRendererLua; } }
        public ListEvent onCreateRenderer { get { return m_OnCreateRenderer; } }
        public ListEventLua onUpdatePost { get { return m_OnUpdatePost; } }

        private void Awake()
        {

            if (m_Template != null && !string.IsNullOrEmpty(m_TemplateAsset))
                Debug.LogWarningFormat("同时含有m_Template和m_TemplateAsset请检查你的预制体 {0}", m_TemplateAsset);

            if (GloablFrameCreateCount > 0 && m_WaitCreateCount == 0)
                m_WaitCreateCount = GloablFrameCreateCount;

            if (m_Template != null)
            {
                if (m_Template != null && !string.IsNullOrEmpty(m_TemplateAsset))
                    m_Template.SetActive(true);
                else
                    m_Template.SetActive(false);

                m_TemplateAsset = null;
                CalculateItemSize();
            }
        }

        public override void Start()
        {
            base.Start();
            Initialize();
        }

        void Initialize()
        {
            if (this.m_ScrollRect == null)
                this.m_ScrollRect = GetComponent<XScrollRect>();

            if (this.m_ScrollRect)
            {
                this.m_Viewport = this.m_ScrollRect.viewport;
                this.m_Content = this.m_ScrollRect.content;
                this.m_ScrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            }
            else
            {
                this.m_Viewport = GetComponent<RectTransform>();
                this.m_Content = (RectTransform)this.m_Viewport.Find("Content");
            }

            if (this.m_Content)
            {
                if(!this.m_IsBatch)
                {
                    //防止 item 拖拽或改变 影响画布重建 但合并不了drawCall
                    //临时测试
                    //this.m_Content.TryGetComponent<Canvas>();
                    //this.m_Content.TryGetComponent<GraphicRaycaster>();
                    this.m_Content.SetLayer(LayerMask.NameToLayer("UI"));
                }
            }


            if (m_ListLayout == ListLayout.Custom && this.m_Content != null)
            {                                      
                m_ContentLayoutGroup = m_Content.GetComponent<LayoutGroup>();
                if (this.m_Content.childCount > 0)
                {
                    m_CustomParents = new List<RectTransform>();
                    foreach (RectTransform item in m_Content)
                    {
                        if (item.name != "Ignore")
                            m_CustomParents.Add(item);     
                    }
                }
            }

            if (!string.IsNullOrEmpty(m_TemplateAsset))
                LoadTemplateAsset();
        }

        public void Update()
        {
            if (m_Template == null)
                return;


            if (this.m_Recalculate)
            {
                this.RecalculateView();
                this.m_Recalculate = false;
                try
                {
                    UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnUpdatePost");
                    m_OnUpdatePost.Invoke(0);
                    UnityEngine.Profiling.Profiler.EndSample();
                }
                catch (Exception e)
                {
                    XLogger.ERROR(e.ToString());
                }
            }


            if (m_WaitCreateList != null && m_WaitCreateList.Count > 0)
            {
                if (m_Temp == null)
                    m_Temp = new List<ListItemRenderer>();

                int count = 0;
                foreach (var item in m_WaitCreateList)
                {
                    m_Temp.Add(item.Value);
                    UpdateWiatCreateItem(item.Key, item.Value);
                    if (++count >= m_WaitCreateCount)
                        break;
                }

                if (m_Temp.Count > 0)
                {
                    foreach (var item in m_Temp)
                        m_WaitCreateList.Remove(item.index);
                }
            }
        }

        protected virtual void OnScrollValueChanged(Vector2 normalizedPosition)
        {
            if (this.m_ListLayout == ListLayout.Custom) return;
            UpdateView();
        }

        void UpdateOffset()
        {
            this.m_ScrollOffset.Set(content.anchoredPosition.x, -content.anchoredPosition.y);
            //this.m_ScrollOffset.Set(content.localPosition.x, -content.localPosition.y);
        }

        void UpdateView()
        {
            if (m_Template == null)
                return;

            UnityEngine.Profiling.Profiler.BeginSample("XListView.UpdateView");
            UpdateOffset();
            UpdateItems();
            UnityEngine.Profiling.Profiler.EndSample();
        }


        protected void UpdateItems()
        {
            if (!Application.isPlaying)
                return;

            if (m_DataCount < 1 && !this.m_Recalculate)
                return;

            if (this.m_ListLayout == ListLayout.Grid)
            {
                UpdateHorizontalAndVertical();
            }
            else if (this.m_ListLayout == ListLayout.Horizontal)
            {
                UpdateHorizontalOrVertical(0);
            }
            else if (this.m_ListLayout == ListLayout.Vertical)
            {
                UpdateHorizontalOrVertical(1);
            }
            else if (this.m_ListLayout == ListLayout.Custom)
            {
                UpdateCustomLayout();
                if (m_ContentLayoutGroup != null)
                    LayoutRebuilder.MarkLayoutForRebuild(m_Content);
            }
        }

        /// <summary>
        /// 格子布局
        /// </summary>
        protected void UpdateHorizontalAndVertical()
        {
            int order = 0;
            Vector2 offset = Vector2.zero;
            Vector2 localOffset = Vector2.zero;
            Vector2 viewSize = viewRect.rect.size;

            Vector2 axis = GetGridAxis();
            int axis_0 = (int)axis[0];
            int axis_1 = (int)axis[1];
            Vector2 axis0_range = GetVisibleIndexRange(0);
            Vector2 axis1_range = GetVisibleIndexRange(1);
            axis1_range[1] = Mathf.Max(this.m_GridConstraintCount - 1, axis1_range[1]);

            //不在视区的处理
            if (!m_IsCacheItem)
            {
                if (this.listItems != null)
                {
                    foreach (var item in this.listItems)
                    {
                        int idx1 = Mathf.FloorToInt(item.Value.index / (axis1_range[1] + 1));
                        int idx2 = item.Value.index % ((int)axis1_range[1] + 1);

                        localOffset[axis_0] = (idx1 * m_ItemSize[axis_0] + idx1 * m_Spacing[axis_0]);
                        localOffset[axis_1] = (idx2 * m_ItemSize[axis_1] + idx2 * m_Spacing[axis_1]);


                        offset[axis_0] = this.m_ScrollOffset[axis_0] + localOffset[axis_0];
                        offset[axis_1] = this.m_ScrollOffset[axis_1] + localOffset[axis_1];


                        if (offset.x + m_ItemSize.x < 0 || offset.x > viewSize.x ||
                            offset.y + m_ItemSize.y < 0 || offset.y > viewSize.y ||
                            item.Value.index >= dataCount)
                        {
                            if (m_Temp == null)
                                m_Temp = new List<ListItemRenderer>();
                            m_Temp.Add(item.Value);
                        }
                    }

                    if (m_Temp != null && m_Temp.Count > 0)
                    {
                        foreach (var item in m_Temp)
                            Recycle(item.index);
                        m_Temp.Clear();
                    }
                }
            }

            if (m_DataCount < 1)
                return;

            int index = 0;

            //Debug.Log("index: " + index + "  axis0_range :" + axis0_range + "   axis1_range :" + axis1_range);

            for (int i = (int)axis0_range[0]; i <= axis0_range[1]; i++)
                for (int j = (int)axis1_range[0]; j <= axis1_range[1]; j++)
                {
                    index = i * this.m_GridConstraintCount + j;
                    if (index >= dataCount)
                        return;

                    localOffset[axis_0] = (i * m_ItemSize[axis_0] + i * m_Spacing[axis_0]);
                    localOffset[axis_1] = (j * m_ItemSize[axis_1] + j * m_Spacing[axis_1]);

                    offset = this.m_ScrollOffset + localOffset;

                    if (!(offset.x + m_ItemSize.x < 0 || offset.x > viewSize.x ||
                        offset.y + m_ItemSize.y < 0 || offset.y > viewSize.y))
                    {
                        UpdateVisibleItem(index, order++, localOffset);
                    }
                }
        }

        /// <summary>
        /// 水平或垂直布局
        /// </summary>
        /// <param name="axis"></param>
        protected void UpdateHorizontalOrVertical(int axis)
        {

            int order = 0;
            float offset = 0f;
            Vector2 localOffset = Vector2.zero;
            Vector2 viewSize = viewRect.rect.size;

            //不在视区的处理
            if (!m_IsCacheItem)
            {
                if (this.listItems != null)
                {
                    foreach (var item in this.listItems)
                    {
                        offset = item.Value.index * this.m_ItemSize[axis] + item.Value.index * m_Spacing[axis];
                        localOffset[axis] = this.m_ScrollOffset[axis] + offset;
                        if (localOffset[axis] + m_ItemSize[axis] < 0 || localOffset[axis] > viewSize[axis] || item.Value.index >= dataCount)
                        {
                            if (m_Temp == null) m_Temp = new List<ListItemRenderer>();
                            m_Temp.Add(item.Value);
                        }
                    }

                    if (m_Temp != null && m_Temp.Count > 0)
                    {
                        foreach (var item in m_Temp)
                            Recycle(item.index);
                        m_Temp.Clear();
                    }
                }
            }

            if (m_DataCount < 1)
                return;

            Vector2 range = GetVisibleIndexRange(axis);
            //Debug.Log("range: " + range);
            offset = range[0] * this.m_ItemSize[axis] + range[0] * m_Spacing[axis];
            for (int i = (int)range[0]; i <= range[1]; i++)
            {
                localOffset[axis] = this.m_ScrollOffset[axis] + offset;
                if (!(localOffset[axis] + m_ItemSize[axis] < 0 || localOffset[axis] > viewSize[axis]))
                {
                    localOffset[axis] = offset;
                    UpdateVisibleItem(i, order++, localOffset);
                }
                offset += m_ItemSize[axis] + m_Spacing[axis];
            }
        }


        protected void UpdateCustomLayout()
        {
            if (listItems != null)
            {
                s_TempSortKeys.Clear();
                s_TempSortKeys.AddRange(listItems.Keys);
                //为了顺序释放
                s_TempSortKeys.Sort();
                foreach (int item in s_TempSortKeys)
                    Recycle(item);
            }

            if (m_DataCount < 1) return;

            Vector2 pos = Vector2.zero;
            for (int i = 0; i < m_DataCount; i++)
            {
                ListItemRenderer renderer = UpdateVisibleItem(i, i, pos);
                if (renderer != null && renderer.transform)
                    renderer.transform.SetAsLastSibling();
            }

        }

        /// <summary>
        /// 根据轴返回视图范围索引
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        Vector2 GetVisibleIndexRange(int axis)
        {
            Vector2 result = new Vector2();
            if (m_ListLayout != ListLayout.Grid)
            {
                result[0] = Mathf.Clamp(Mathf.FloorToInt(Mathf.FloorToInt(-this.m_ScrollOffset[axis] / (m_ItemSize[axis] + m_Spacing[axis]))), 0, Mathf.Max(0, m_DataCount - 1));
                result[1] = Mathf.Clamp(result[0] + Mathf.CeilToInt(viewRect.rect.size[axis] / (m_ItemSize[axis] + m_Spacing[axis])), 0, Mathf.Max(0, m_DataCount - 1));
            }
            else
            {
                Vector2 axiss = GetGridAxis();
                Vector2 lineCount = GetGridLineCount();
                int axis_0 = (int)axiss[axis];
                int line_0 = (int)lineCount[axis];

                result[0] = Mathf.Clamp(Mathf.FloorToInt(Mathf.FloorToInt(-this.m_ScrollOffset[axis_0] / (m_ItemSize[axis_0] + m_Spacing[axis_0]))), 0, Mathf.Max(0, line_0 - 1));
                result[1] = Mathf.Clamp(result[0] + Mathf.CeilToInt(viewRect.rect.size[axis_0] / (m_ItemSize[axis_0] + m_Spacing[axis_0])), 0, Mathf.Max(0, line_0 - 1));
            }

            return result;
        }


        /// <summary>
        /// 更新可见的Item
        /// </summary>
        /// <param name="index"></param>
        /// <param name="order"></param>
        /// <param name="offset"></param>
        protected ListItemRenderer UpdateVisibleItem(int index, int order, Vector2 offset)
        {
            if (this.m_ListItems == null)
                this.m_ListItems = new Dictionary<int, ListItemRenderer>();

            ListItemRenderer render;
            if (!this.listItems.TryGetValue(index, out render))
                CreateItemRenderer(index, offset, out render);
            else if (this.m_Recalculate)
            {
                UpdateItemPosition(index, offset, render);
                InvokeUpdateEvent(render, true);
            }

            return render;
        }


        /// <summary>
        /// 回收Item
        /// </summary>
        /// <param name="idx"></param>
        protected void Recycle(int idx)
        {
            if (this.listItems == null)
                return;
            ListItemRenderer render;
            if (this.listItems.TryGetValue(idx, out render))
            {
                if (this.m_RecyclePool == null)
                    this.m_RecyclePool = new List<ListItemRenderer>();
                this.m_RecyclePool.Add(render);
                this.listItems.Remove(idx);

                UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnRecycleRenderer");
                this.m_OnRecycleRenderer.Invoke(render);
                UnityEngine.Profiling.Profiler.EndSample();

                UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnRecycleRendererLua");
                this.m_OnRecycleRendererLua.Invoke(render.instanceID);
                UnityEngine.Profiling.Profiler.EndSample();

                //注意滚动条划动时会频繁的切换Active
                if (this.m_RecycleDeActive)
                    render.gameObject.SetActive(false);
                else
                    render.transform.localPosition = new Vector3(-9999, -9999, 0);
            }
        }

        public virtual ListItemRenderer GetItemRenderer(int index)
        {
            ListItemRenderer item;
            if (this.listItems.TryGetValue(index, out item)) { }
            return item;
        }

        public virtual ListItemRenderer GetItemByInstanceId(int instanceID)
        {
            foreach(var item in this.listItems)
            {
                if (item.Value.instanceID == instanceID) return item.Value;
            }

            return null;
        }

        protected RectTransform GetCreateItemParent(int index)
        {
            if (m_ListLayout == ListLayout.Custom)
            {
                if (m_CustomParents == null || index < 0 || index >= m_CustomParents.Count)
                {
                    return content;
                }
                return m_CustomParents[index];
            }
            return content;
        }


        protected void CreateItemRenderer(int index, Vector2 offset, out ListItemRenderer render)
        {
            render = null;
            if (this.m_RecyclePool != null && this.m_RecyclePool.Count > 0)
            {
                render = this.m_RecyclePool[0];
                this.m_RecyclePool.RemoveAt(0);
                render.SetData(index, render.gameObject);

                if (m_ListLayout == ListLayout.Custom)
                {
                    RectTransform parent = GetCreateItemParent(index);
                    render.transform.SetParent(parent);
                }
                OnItemRendererCreate(index, offset, render);
                InvokeUpdateEvent(render, true);
            }
            else if (m_WaitCreateCount > 0)
            {
                //此为分帧创建
                if (this.m_WaitCreateList == null)
                    m_WaitCreateList = new Dictionary<int, ListItemRenderer>();

                if (!m_WaitCreateList.ContainsKey(index))
                {
                    render = new ListItemRenderer();
                    render.offset = offset;
                    m_WaitCreateList.Add(index, render);
                }
            }
            else
            {
                render = new ListItemRenderer();
                render.SetData(index, Instantiate<GameObject>(this.m_Template, GetCreateItemParent(index), false));

                OnItemRendererCreate(index, offset, render);

                try
                {
                    UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnCreateRenderer");
                    m_OnCreateRenderer.Invoke(render);
                    UnityEngine.Profiling.Profiler.EndSample();
                }
                catch (System.Exception ex)
                {
                    XLogger.ERROR(ex.ToString());
                }

                InvokeUpdateEvent(render, false);
            }
        }

        void OnItemRendererCreate(int index, Vector2 offset, ListItemRenderer render)
        {
            this.listItems[render.index] = render;
#if UNITY_EDITOR
            render.gameObject.name = "ItemRenderer" + index;
#endif

            RectTransform rect = render.transform;
            if (this.m_ListLayout != ListLayout.Custom)
            {

                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
                rect.anchoredPosition = Vector2.zero;
            }

            if (m_ControllChildSize)
            {
                if (this.m_ListLayout == ListLayout.Vertical)
                {
                    rect.sizeDelta = new Vector2(viewRect.rect.size.x, rect.sizeDelta.y);
                }
                else if (this.m_ListLayout == ListLayout.Horizontal)
                {
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, viewRect.rect.size.y);
                }
                else
                {
                    rect.sizeDelta = this.m_CellSize;
                }
            }

            UpdateItemPosition(index, offset, render);
        }

        //分帧创建

        protected void UpdateWiatCreateItem(int index, ListItemRenderer render)
        {
            render.SetData(index, Instantiate<GameObject>(this.m_Template, GetCreateItemParent(index), false));

            OnItemRendererCreate(index, render.offset, render);
            try
            {
                UnityEngine.Profiling.Profiler.BeginSample("XListView::UpdateWiatCreateItem.m_OnCreateRenderer");
                m_OnCreateRenderer.Invoke(render);
                UnityEngine.Profiling.Profiler.EndSample();
            }
            catch (System.Exception ex)
            {
                XLogger.ERROR(ex);
            }

        }


        void InvokeUpdateEvent(ListItemRenderer render, bool isLuaUpdate = false)
        {
            try
            {
                UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnUpdateRenderer");
                //if (m_OnUpdateRenderer.GetPersistentEventCount() > 0)
                m_OnUpdateRenderer.Invoke(render);
                UnityEngine.Profiling.Profiler.EndSample();

                if (isLuaUpdate)
                {
                    UnityEngine.Profiling.Profiler.BeginSample("XListView.m_OnUpdateRendererLua");
                    m_OnUpdateRendererLua.Invoke(render.instanceID);
                    UnityEngine.Profiling.Profiler.EndSample();
                }

            }
            catch (System.Exception ex)
            {
                XLogger.ERROR(ex);
            }

        }


        protected void UpdateItemPosition(int index, Vector2 offset, ListItemRenderer render)
        {
            //render.transform.localPosition = new Vector3(offset.x, -offset.y);
            render.transform.anchoredPosition = new Vector2(offset.x, -offset.y);

            if (this.m_ItemSize != render.transform.sizeDelta)
                render.transform.sizeDelta = this.m_ItemSize;

            if (!render.gameObject.activeSelf)
                render.gameObject.SetActive(true);
        }

        void CalculateContentSize()
        {
            if (!Application.isPlaying)
                return;

            if (this.m_DataCount == 0)
            {
                content.sizeDelta = Vector2.zero;
                return;
            }

            if (this.m_ListLayout == ListLayout.Custom)
            {
                return;
            }


            if (this.m_ListLayout == ListLayout.Horizontal)
            {
                this.m_ContentSize.x = m_DataCount * m_ItemSize.x + Mathf.Max(0, (m_DataCount - 1)) * m_Spacing.x;
                this.m_ContentSize.y = m_ItemSize.y;
            }


            if (this.m_ListLayout == ListLayout.Vertical)
            {
                this.m_ContentSize.x = m_ItemSize.x;
                this.m_ContentSize.y = m_DataCount * m_ItemSize.y + Mathf.Max(0, (m_DataCount - 1)) * m_Spacing.y;
            }


            if (this.m_ListLayout == ListLayout.Grid)
            {
                Vector2 lineCount = GetGridLineCount();
                Vector2 axiss = GetGridAxis();
                this.m_ContentSize[(int)axiss[0]] = m_ItemSize[(int)axiss[0]] * lineCount[0] + Mathf.Max(0, lineCount[0] - 1) * m_Spacing[(int)axiss[0]];
                this.m_ContentSize[(int)axiss[1]] = m_ItemSize[(int)axiss[1]] * lineCount[1] + Mathf.Max(0, lineCount[1] - 1) * m_Spacing[(int)axiss[1]];
            }

            content.anchorMin = content.anchorMax = content.pivot = new Vector2(0, 1);

            content.anchoredPosition = this.m_Recalculate ? content.anchoredPosition : Vector2.zero;
            content.sizeDelta = this.m_ContentSize;
        }

        protected Vector2 GetGridAxis()
        {
            int axis1 = this.m_GridConstraint == GridConstraint.ColumnCount ? 1 : 0;
            int axis2 = axis1 == 0 ? 1 : 0;
            return new Vector2(axis1, axis2);
        }

        protected Vector2 GetGridLineCount()
        {
            int line = Mathf.CeilToInt((float)this.m_DataCount / this.m_GridConstraintCount);
            int line2 = Mathf.Max(this.m_GridConstraintCount, Mathf.CeilToInt((float)this.m_DataCount / line));
            return new Vector2(line, line2);
        }

        protected void CalculateItemSize()
        {
            if (!Application.isPlaying)
                return;

            if (this.m_Template != null)
            {
                RectTransform rect = m_Template.GetComponent<RectTransform>();
                Vector2 size = rect.rect.size;
                this.m_ItemSize.Set(size.x > 0 ? size.x : this.m_ItemSize.x, size.y > 0 ? size.y : this.m_ItemSize.y);
            }
        }

        public Vector2 GetItemPositionByIndex(int index)
        {
            Vector2 result = Vector2.zero;
            if (index >= 0 && index < m_DataCount)
            {
                if (this.m_ListLayout == ListLayout.Horizontal || this.m_ListLayout == ListLayout.Vertical)
                {

                    result.x = this.m_ListLayout == ListLayout.Vertical ? 0 : -(index * m_ItemSize.x + index * m_Spacing.x);
                    result.y = this.m_ListLayout == ListLayout.Horizontal ? 0 : index * m_ItemSize.y + index * m_Spacing.y;
                }
                else if (this.m_ListLayout == ListLayout.Custom)
                {
                    result.x = this.m_customLayout == CustomLayout.Vertical ? 0 : -(index * m_ItemSize.x + index * m_Spacing.x);
                    result.y = this.m_customLayout == CustomLayout.Horizontal ? 0 : index * m_ItemSize.y + index * m_Spacing.y;
                }
                else
                {
                    Vector2 lineCount = GetGridLineCount();
                    Vector2 axiss = GetGridAxis();
                    int axis_0 = (int)axiss[0];
                    int axis_1 = (int)axiss[1];
                    int hNum = (int)lineCount[axis_0];
                    int vNum = (int)lineCount[axis_1];

                    int yIdx = Mathf.CeilToInt(index / hNum);
                    result.y = yIdx * m_ItemSize.y + yIdx * m_Spacing.y;
                }
            }
            return result;
        }

        public void ScrollToIndex(int index, float smoothTime = 0.1f, bool center = false)
        {
            if (!this.m_ScrollRect)
                return;
            Vector2 pos = this.GetItemPositionByIndex(index);
            if (center && m_Viewport != null)
            {
                Vector2 tv = new Vector2(this.m_ListLayout == ListLayout.Horizontal || this.m_ListLayout == ListLayout.Grid ? 1 : 0,
                                         this.m_ListLayout == ListLayout.Vertical || this.m_ListLayout == ListLayout.Grid ? 1 : 0);
                pos.x += tv.x * (m_Viewport.rect.size.x * 0.5f - m_ItemSize.x * 0.5f);
                pos.y -= tv.y * (m_Viewport.rect.size.y * 0.5f - m_ItemSize.y * 0.5f);
            }
            m_ScrollPos = pos;
            m_ScrollTime = smoothTime;
            if (!m_Recalculate)
            {
                this.m_ScrollRect.ScrollToPosition(pos, smoothTime);
            }
            m_IsScroll = m_Recalculate;
        }

        private void DelayScroll()
        {
            if (m_IsScroll)
            {
                this.m_ScrollRect.ScrollToPosition(m_ScrollPos, m_ScrollTime);
                m_IsScroll = false;
            }
        }

        /// <summary>
        /// 参数变化重新计算
        /// </summary>
        protected void RecalculateView()
        {
            CalculateItemSize(); 
            CalculateContentSize();
            UpdateView();
            DelayScroll();
        }

        private AssetManagement.AssetLoaderParcel loader;
        public void LoadTemplateAsset()
        {
            if (string.IsNullOrEmpty(m_TemplateAsset))
                return;


            loader = AssetManagement.AssetUtility.LoadAsset<GameObject>(this.m_TemplateAsset);
            loader.onComplete += LoadDone;
        }

        private void LoadDone(AssetManagement.AssetLoaderParcel load)
        {
        
             this.m_Template = loader.GetRawObject<GameObject>();

            CalculateItemSize();
            ForceRefresh();
        }

        public void ForceRefresh()
        {
            if (!this.m_Recalculate)
            {
                this.m_Recalculate = true;
            }
        }

        //public void OnRectTransformDimensionsChange()
        //{
        //    SetDirty();
        //}

        //protected void SetDirty()
        //{
        //    if (m_Content == null || m_Viewport == null || !isActiveAndEnabled)
        //        return;
        //    ForceRefresh();
        //    LayoutRebuilder.MarkLayoutForRebuild(m_Viewport);
        //}

        public void ClearAll()
        {
            if (this.m_RecyclePool != null)
            {
                foreach (var item in this.m_RecyclePool)
                    item.Destroy();
                m_RecyclePool.Clear();
            }


            if (this.listItems != null)
            {
                foreach (var item in this.listItems)
                    item.Value.Destroy();
                listItems.Clear();
            }

        }


        public override void ClearEvent()
        {
            base.ClearEvent();
            if (m_OnCreateRenderer != null)
            {
                m_OnCreateRenderer.RemoveAllListeners();
                m_OnCreateRenderer = null;
            }
            if (m_OnRecycleRenderer != null)
            {
                m_OnRecycleRenderer.RemoveAllListeners();
                m_OnRecycleRenderer = null;
            }

            if (m_OnRecycleRendererLua != null)
            {
                m_OnRecycleRendererLua.RemoveAllListeners();
                m_OnRecycleRendererLua = null;
            }

            if (m_OnUpdateRenderer != null)
            {
                m_OnUpdateRenderer.RemoveAllListeners();
                m_OnUpdateRenderer = null;
            }

            if (m_OnUpdateRendererLua != null)
            {
                m_OnUpdateRendererLua.RemoveAllListeners();
                m_OnUpdateRendererLua = null;
            }

            if (m_OnUpdatePost != null)
            {
                m_OnUpdatePost.RemoveAllListeners();
                m_OnUpdatePost = null;
            }

            if (m_OnDestroy != null)
            {
                m_OnDestroy.RemoveAllListeners();
                m_OnDestroy = null;
            }
        }

        public override void OnDestroy()
        {
            if (loader != null)
            {
                loader.onComplete -= LoadDone;
                loader = null;
            }

            if (m_Template != null)
            {
                AssetManagement.AssetUtility.DiscardRawAsset(m_Template);
                m_Template = null;
                m_TemplateAsset = null;
            }

            base.OnDestroy();
            this.ClearAll();
            this.ClearEvent();
        }


        public class ListItemRenderer
        {
            private int m_Index;
            public int index { get { return m_Index; } }

            private int m_InstanceID;
            public int instanceID { get { return m_InstanceID; } }

            private GameObject m_GameObject;
            public UnityEngine.GameObject gameObject { get { return m_GameObject; } }
            private RectTransform m_Transform;
            public UnityEngine.RectTransform transform { get { return m_Transform; } }
            public Vector2 offset { get; set; }

            public void SetData(int index, GameObject gameObject)
            {
                this.m_Index = index;
                this.m_GameObject = gameObject;
                this.m_Transform = gameObject.GetComponent<RectTransform>();
                this.m_InstanceID = gameObject.GetInstanceID();
            }

            public void Destroy()
            {
                AssetManagement.AssetUtility.DestroyInstance(this.m_GameObject);
            }
        }
    }
}
