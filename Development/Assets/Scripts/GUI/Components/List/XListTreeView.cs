using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XGUI;
using XLua;
using DG.Tweening;
                
namespace XGUI
{

    [Serializable]
    public class TreeItemInfo
    {
        public string title;
        public bool multiple  = false;//多选
        public bool layerSele = false;//子集的层只能选中一个对象
        public bool notclickCancel = false;//点击选中子项，不取消选中
       // [NonSerialized]
        public TreeItemInfo[] list;//子集
        public string key;

        public int index;//所处list 的index
        public int count;//对应第几个TreeItem
        public int layer;//树结构的底基层
        [NonSerialized]
        public TreeItemInfo parent = null;
        public TreeItem parent_item = null;
    }

    public  class TreeItem
    {
        private TreeItemInfo m_info;

        public bool isOpen;//是否被父标签激活
        public bool b_Selected;//是否被选中
        public List<int> selectIndex;//选中的子对象index

        public bool visible;//是否显示
        public bool needFresh;
        public Vector2 fPosition;//绝对位置
        public RectTransform transfrom;//对象
        public GameObject gameObject;
        public Vector2 defaultSize = new Vector2(100.0f, 50.0f);
        public Vector2 gap;

        //public bool enableSelect = true;
        public XButton button;
        private TreeItemInfo lastInfo;
        public Action<TreeItem> onClick;

        public TreeItemInfo info
        {
            set { m_info = value; lastInfo = null; }
            get { return m_info; }
        }

        public void Show(GameObject gmObject, Transform parent, bool needEffect, Rect viewRect,bool invaild_scroll)
        {                        
            if (visible == true) return;
            visible = true;
            needFresh = false;
            transfrom = gmObject.GetComponent<RectTransform>();
            gameObject = gmObject;
            gameObject.SetActive(true);  
            //Button click 事件

            button = transfrom.gameObject.GetComponentInChildren<XButton>();
            if (button == null) button = transfrom.gameObject.AddComponent<XButton>();

            button.onClick.AddListener(OnClick);
            button.group = null;
         
            button.label = info.title;

            isSelected = b_Selected;
            PlayShowEffect(parent, needEffect, viewRect, invaild_scroll);
        }

        public void PlayShowEffect(Transform parent, bool needEffect, Rect viewRect, bool invaild_scroll)
        {
            if (!transfrom) return;
            if (info.layer == 1)
            {
                if (!invaild_scroll)
                {
                    Vector3 maxPos = new Vector3(0, viewRect.yMax - height, 0);
                    transfrom.SetParentEx(parent, maxPos, Vector3.zero, Vector3.one);
                }
                else
                {
                    transfrom.SetParentOEx(parent);
                    ApplyPos();
                    return;
                }
                if (needEffect)
                    ApplyDotweenPos();                 
                else
                    ApplyPos();
            }
            else
            {
                if (invaild_scroll)
                {
                    transfrom.SetParentOEx(parent);
                    ApplyPos();
                    return;
                }
                transfrom.SetParentOEx(parent);
                ApplyPos();

                CanvasGroup group = gameObject.GetComponent<CanvasGroup>() ? gameObject.GetComponent<CanvasGroup>() : gameObject.AddComponent<CanvasGroup>();
                group.alpha = 0;
                group.DOFade(1, 0.5f);
            }
        }
        public bool enableSelect
        {
            get { return button.enableSelect; }
            set
            { 
                button.enableSelect = value;
            }
        }
        private TreeItemInfo NextFirstInfo(TreeItemInfo info)
        {
            if (info.list != null && info.list.Length > 0) return NextFirstInfo(info.list[0]);
            else
                return info;
        }

        public TreeItemInfo GetLastFirstInfo()
        {
            if(lastInfo == null) lastInfo = NextFirstInfo(m_info);
            return lastInfo;
        }           

        public void Hide()
        {
            visible = false;
            if (button != null)
            {
                button.onClick.RemoveListener(OnClick);
                button.isSelected = false;
            }
            button = null;
            needFresh = false;
            transfrom = null;
            gameObject = null;
            onClick = null;
        }

        public bool isSelected
        {
            get { return b_Selected; }
            set
            {
                b_Selected = value;
                if (button != null) button.isSelected = value;
         
            }
        }
        private bool old_Selected = false;//之前的选中状态
        public bool oldSelected
        {
            get { return old_Selected; }
            set
            {
                old_Selected = value;
            }
        }
        private void OnClick()
        {
            if(onClick != null)onClick(this);                

        }
        public void ApplyDotweenPos()
        {
            if (transfrom != null)
                transfrom.DOLocalMoveY(fPosition.y, 0.2f);      
        }

        public void ApplyPos()
        {
            if (transfrom != null)
                transfrom.anchoredPosition = fPosition;
        }

        public float with
        {
            get
            {
                if (transfrom != null) return transfrom.sizeDelta.x;

                return defaultSize.x;
            }
        }

        public float height
        {
            get
            {
                if (transfrom != null) return transfrom.sizeDelta.y;

                return defaultSize.y;
            }
        }

    }

    [Serializable]
    public class ListTreeObj
    {
        public GameObject template;
        private Vector2 m_size = Vector2.zero;
        public Vector2 gap = Vector2.zero;

        private RectTransform m_rect;
        public Vector2 size
        {
            get
            {
                if(m_size == Vector2.zero)
                {
                     m_size = transform.sizeDelta;
                }

                return m_size;
            }
        }

        public RectTransform transform
        {
            get
            {
                if(m_rect == null) m_rect = template.GetComponent<RectTransform>();

                return m_rect;
            }
        }

    }
    /// <summary>
    /// 龙跃
    /// 树结构列表组件
    /// 结合 ListTreeMxNView.lua 使用
    /// 支持 1 - N 层的树结构
    /// 每个节点有(夸节点层选择，节点下单选，节点下多选) 三种选择模式
    /// </summary>
    public class XListTreeView : XView, XIEvent
    {

        /// <summary>
        /// 数据单元
        /// </summary>

        //模板列表
        public List<ListTreeObj> templates = new List<ListTreeObj>();
        private ListTreeObj GetTemplate(int layer)
        {
            if (layer > templates.Count) layer = templates.Count;

            return templates[layer - 1];
        }

        public XScrollRect m_scrollRect;
        public RectTransform m_content;//容器   
        private bool m_needEffect = false;//是否需要各种打开时的展示特效
        public RectTransform m_viewPort;//可视化矩形
        private RectTransform m_transform;
        private Rect s_viewRect = new Rect();
        private bool invalidDatas;
        private bool invalidLayout;
        private bool invalidView;
       // private bool invalidAutoSele;
        private bool invalidDraw;
        private bool invalidUpdate;
        //排序
        public enum Layout
        {
            Horizontal, //水平
            Vertical,   //垂直
        }

        [SerializeField]
        private Layout layout = Layout.Vertical;

        [SerializeField]
        private float m_top = 0;
        [SerializeField]
        private float m_botton = 0;
        [SerializeField]
        private float m_right = 0;
        [SerializeField]
        private float m_left = 0;

        [HideInInspector]       
        private bool scrollDrag = false;

        //点击标签时，默认选择子集的第一个
        public bool m_autoSelFirst = false;
        public bool m_scrollToSelect = false;
        //callLuaFun
        public Action<int> onItemClick;//子项点击
        public Action<int,bool,bool> onItemSelected;//子项选中
        public Action onItemPlayTween;//显示完成的动画
        private void CallSelectedFun(TreeItem item,bool value)
        {
            if (onItemSelected != null && item.gameObject != null)
            {
                onItemSelected.Invoke(item.gameObject.GetInstanceID(), value, item.oldSelected);
                item.oldSelected = value;
            }
                 

        }

        public Action<string> onShowItem;//子项创建
        public Action<int> onCircleItem;//回收一个子项

        private void Awake()
        {
            m_transform = gameObject.GetComponent<RectTransform>();
            m_transform.anchorMin = new Vector2(0.5f, 0.5f);
            m_transform.anchorMax = new Vector2(0.5f, 0.5f);
            m_transform.pivot = new Vector2(0.5f, 0.5f);
        }

        public override void Start()
        {
            base.Start();

        
            if (templates.Count == 0)
            {
                ListTreeObj treeObj = new ListTreeObj();
                GameObject obj = new GameObject("template", typeof(XButton));
                obj.transform.SetParentOEx(this.transform);
                treeObj.template = obj;
                treeObj.transform.sizeDelta = new Vector2(200, 20);
                templates.Add(treeObj);
            }


            if (m_viewPort == null)
            {
                m_viewPort = new GameObject("ViewPort", typeof(RectMask2D)).GetComponent<RectTransform>();
                m_viewPort.SetParentOEx(m_transform);
            }

            m_viewPort.anchorMin = new Vector2(0, 0);
            m_viewPort.anchorMax = new Vector2(1, 1);
            m_viewPort.pivot = new Vector2(0, 1.0f);
            m_viewPort.sizeDelta = Vector2.zero;

            if (m_content == null)
            {
                m_content = new GameObject("Content", typeof(RectTransform)).GetComponent<RectTransform>();
                m_content.SetParentOEx(m_viewPort);
            }

            m_content.anchorMin = new Vector2(0, 1);
            m_content.anchorMax = new Vector2(0, 1);
            m_content.pivot = new Vector2(0, 1);
            m_content.anchoredPosition = Vector2.zero;

            if (m_scrollRect != null) m_scrollRect.onValueChanged.AddListener(OnScrollValueChanged);


            CalculationRect();

        }

        public override void ClearEvent()
        {
            //button 按钮为内部事件

            base.ClearEvent();
            if (m_scrollRect != null && m_scrollRect.onValueChanged != null) m_scrollRect.onValueChanged.RemoveAllListeners();

            onItemClick = null;
            onItemSelected = null;
            onItemPlayTween = null;
            onShowItem = null;
            onCircleItem = null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            m_scrollRect = null;
            m_viewPort = null;
            m_content = null;
            templates = null;
        }


        private List<string> needRemove = new List<string>();
        private void Update()
        {
            //绘制
            if (invalidDatas)
            {
                JsonUtility.FromJsonOverwrite(n_treeJson, n_treeData);

                n_count = 0;

                n_totalKeys.Clear();
                n_countKeys.Clear();

                AnalysisTree(n_treeData, 0, topKey, 0, null,null);

                //修正层选择
                OnFreshLayerSlected();

                //剔除多余
                needRemove.Clear();
                foreach (var map in n_totalItem)
                {
                    string key = map.Value.info.key;
                    if (!n_totalKeys.ContainsKey(key)) needRemove.Add(key);//比上次数据多出的TreeItem
                }

                for (int i = 0; i < needRemove.Count; i++)
                {
                    TreeItem item = n_totalItem[needRemove[i]];
                    item.isSelected = false;
                    item.isOpen = false;
                    n_totalItem.Remove(needRemove[i]);

                    this.PushItem(item);//回收

                }

                needRemove.Clear();
                invalidDatas = false;
            }

            if(invaildCancelSele)
            {
                //取消全部选中

                TreeItem Topitem = null;
                if(n_totalItem.TryGetValue(topKey,out Topitem))
                {
                    if (Topitem.selectIndex != null) Topitem.selectIndex.Clear();
                    for (int i = 0; i < loops2.Count; i++)
                    {
                        if (loops2[i].selectIndex != null) loops2[i].selectIndex.Clear();
                        loops2[i].isSelected = false;

                    }

                    for (int i = 0; i < n_countKeys.Count; i++)
                    {
                        TreeItem item = n_totalItem[n_countKeys[i]];
                        item.oldSelected = false;
                        if (item.selectIndex != null) item.selectIndex.Clear();
                        item.isSelected = false;
                        if (item.info.layer > 1) item.isOpen = false;
                        CallSelectedFun(item, false);
                    }

                }

                invaildCancelSele = false;
            }

            if(invalidUpdate)
            {
                //刷新全部显示
                for (int i = 0; i < n_countKeys.Count; i++)
                {
                    TreeItem item = n_totalItem[n_countKeys[i]];
                    if (item.isOpen && item.visible) item.needFresh = true;
                }

                invalidUpdate = false;
            }

            //if(invalidAutoSele && m_autoSelFirst)
            //{
            //    //设置选择顶层的第一个
            //    TreeItem Topitem = n_totalItem[topKey];
            //    TreeItemInfo info = Topitem.GetLastFirstInfo();
            //    if(info != null)
            //        this.SetSeleIndex(info.key);

            //}

           // invalidAutoSele = false;   

            if (invalidView || invalidDraw)
            {
                OnVisualization();//显隐
                invalidView = false;
            }

            if (invalidLayout || invalidDraw)
            {
                CountAbsoluteCoordinates();//计算布局
                invalidLayout = false;
                onItemPlayTween.Invoke();
            }

            bool hadDraw = invalidDraw;
           
            invalidDraw = false;

            if(!hadDraw) OutSideOperation(); //渲染完毕才能执行自动选择
        }

        private void OutSideOperation()
        {
            
            //外部滑动设置操作
            if (invaild_scrollToItm != null)
            {
                TreeItem item = null;
                if (n_totalItem.TryGetValue(invaild_scrollToItm, out item))
                {
                  
                    if(!item.visible)
                    {
                        Vector2 postion = item.fPosition;
                        postion.y = -(postion.y + m_transform.sizeDelta.y / 2);
                        if (m_scrollRect != null)
                        {
                            //float distance = (postion - m_content.anchoredPosition).magnitude;
                            //float time = 0.1f * (distance * 0.01f);
                            m_scrollRect.ScrollToPosition(postion, 0.1f);
                        }
                        else
                            m_content.anchoredPosition = postion;
                    }
                }

            }
            else if (invaild_scrollTop)
            {
                if (m_scrollRect != null)
                    m_scrollRect.ScrollToTop(smoothTime);
                else
                    m_content.anchoredPosition = Vector3.zero;
            }
            else if (invaild_scrollBotton)
            {
                if (m_scrollRect != null)
                    m_scrollRect.ScrollToBottom(smoothTime);
                else
                {
                    Vector2 position = total_size;
                    position.x -= m_transform.sizeDelta.x;
                    position.y -= m_transform.sizeDelta.y;
                    m_content.anchoredPosition = position;
                }
 
            }

            invaild_scrollToItm = null;
            invaild_scrollTop = false;
            invaild_scrollBotton = false;


            //外部选中设置操作

            if (invalid_seleIndex_step2 != null)
            {
                TreeItem item = null;
                if (n_totalItem.TryGetValue(invalid_seleIndex_step2, out item))
                    OnSelected(item, true);

                if (m_scrollToSelect) this.OnScrollToItem(invalid_seleIndex_step2);

                invalid_seleIndex_step2 = null;

            }

            if (invalid_seleIndex_step1 != null)
            {
                TreeItem item = null;
                if (n_totalItem.TryGetValue(invalid_seleIndex_step1, out item))
                {
                    item.isOpen = true;
                    OnFreshFatherOpen(item, true);

                    invalid_seleIndex_step2 = invalid_seleIndex_step1;
                }

                invalid_seleIndex_step1 = null;

            }
        }

        public void ClearItems()
        {

        }

        #region 数据处理
        private string topKey = "topTree";
        private TreeItemInfo n_treeData;//树结构数据
        private Dictionary<string, TreeItem> n_totalItem = new Dictionary<string, TreeItem>();//全部Item数据
        private Dictionary<string, bool> n_totalKeys = new Dictionary<string, bool>();
        private List<string> n_countKeys = new List<string>();//从上到下的一个队列
        private string n_treeJson;
        private int n_count; //总结的数量        
        /// <summary>
        /// 结构 是一个 0 - n 层的树结构。传字符串为了减小CG
        /// </summary>
        /// <param name="listTree"></param>
        public void SetData(string listJson)
        {
            if (listJson == null)
            {
                ClearItems();
                return;
            }

            if (n_treeData == null) n_treeData = new TreeItemInfo();
            if (n_treeJson == listJson)
            {
                //不用刷新数据
                invalidUpdate = true;
                invalidDraw = true;
                //invalidAutoSele = true;
            }
            else
            {
                n_treeJson = listJson;

                invalidDatas = true;
                invalidDraw = true;
                //invalidAutoSele = true;
            }


        }

        private void AnalysisTree(TreeItemInfo info, int index, string key, int layer, TreeItemInfo parent, TreeItem parentItem)
        {
            if (key != topKey)
            {
                n_count++;
                n_countKeys.Add(key);
            }

            info.key = key;//index 组合 
            n_totalKeys.Add(key, true);
            info.count = n_count;
            info.layer = layer;
            info.parent = parent;
            info.index = index;
            if (parentItem != null)
                info.parent_item = parentItem;
            TreeItem item = null;
            if (!n_totalItem.TryGetValue(key, out item))
            {
                item = this.GetPoolItem();
                if (key != topKey)
                {
                    ListTreeObj obj = GetTemplate(info.layer);
                    item.defaultSize = obj.size;
                    item.gap = obj.gap;
                }


                n_totalItem.Add(key, item);
            }

            if (layer <= 1) item.isOpen = true;
            item.needFresh = true;//刷新已在的Item数据显示
            item.info = info;//更新数据
            OnFreshSelectedIndex(info);


            if (info.list != null && info.list.Length > 0)
            {
                layer = layer + 1;
                for (int i = 0; i < info.list.Length; i++)
                {
                    if (key == topKey)
                        AnalysisTree(info.list[i], i, i.ToString(), layer, info, item);
                    else
                        AnalysisTree(info.list[i], i, key + "_" + i, layer, info, item);
                }
            }
        }

        public TreeItem GetTreeItem(string key)
        {
            TreeItem item = null;
            n_totalItem.TryGetValue(key, out item);

            return item;
        }
        #endregion

        #region 选中处理
        private Dictionary<int, string> n_layerSeleMap  = new Dictionary<int, string>();

        //根据数据刷新选中
        private void OnFreshSelectedIndex(TreeItemInfo info)
        {
            TreeItem item = null;
            n_totalItem.TryGetValue(info.key, out item);
            if (item.selectIndex == null) return;
            if (info.list == null)
            {
                item.selectIndex.Clear();
                return;
            }

            //更新选中数据
            if (info.multiple)
            {
                //多选
                for (int i = 0; i < item.selectIndex.Count; i++)
                {
                    int sIndex = item.selectIndex[i];
                    if (sIndex >= info.list.Length)
                    {
                        item.selectIndex.RemoveAt(i);
                        TreeItemInfo itemInfo = item.info.list.Length > sIndex ? item.info.list[sIndex] : null;
                        CancelSelect(itemInfo);
                        i--;
                    }
                }

            }
            else
            {
                //单选
                if (item.selectIndex.Count == 1)
                {
                    int sIndex = item.selectIndex[0];
                    if (sIndex >= info.list.Length)
                    {
                        TreeItemInfo itemInfo = item.info.list.Length > sIndex ? item.info.list[sIndex] : null;
                        CancelSelect(itemInfo);
                        item.selectIndex.Clear();
                    }

                }
                else
                    item.selectIndex.Clear();
            }
        }

        private void CancelSelect(TreeItemInfo itemInfo)
        {
            if (itemInfo == null) return;

            TreeItem item = null;
            if (n_totalItem.TryGetValue(itemInfo.key, out item))
            {
                item.isSelected = false;
            }
        }


        private void OnFreshLayerSlected()
        {
            foreach (var map in n_layerSeleMap)
            {
                if (!n_totalKeys.ContainsKey(map.Value)) needRemove[map.Key] = null;

            }

        }

        protected void OnItemClick(TreeItem item)
        {
            if (onItemClick != null) onItemClick.Invoke(item.transfrom.gameObject.GetInstanceID());

            if (!item.enableSelect || layerNotClickMap.ContainsKey(item.info.layer)) return;//不能选中

            OnSelected(item, false);              

            if(m_autoSelFirst && item.b_Selected)
            {
                TreeItemInfo firstInfo = item.GetLastFirstInfo();
                TreeItem sonItem = n_totalItem[firstInfo.key];
               if(!sonItem.b_Selected)
                SetSeleIndex(firstInfo.key);
            }

        }


        protected void OnSelected(TreeItem item, bool bForce = false,bool callLua = true)
        {
            TreeItem tParent = n_totalItem[item.info.parent.key];
            if (tParent.selectIndex == null) tParent.selectIndex = new List<int>();

            if (tParent.info.notclickCancel && item.isSelected) return;//点击相同不可取消选中

            if (item.b_Selected && bForce) return;//已经选中

            if (tParent.info.layerSele)
            {
                if (!item.isSelected || bForce)
                {
                    //层选择模式
                    string oldSkey = null;
                    if (n_layerSeleMap.TryGetValue(item.info.layer, out oldSkey))
                    {
                        //取消旧的
                        if (oldSkey != null)
                        {
                            TreeItem oldItem = n_totalItem[oldSkey];
                            oldItem.isSelected = false;
                            OnFreshSonOpen(oldItem, false);

                            //callLua
                            if(callLua)CallSelectedFun(oldItem, false);
                        }

                    }

                    //选中新的
                    item.isSelected = true;
                    n_layerSeleMap[item.info.layer] = item.info.key;

                }
                else
                {
                    //取消选中
                    item.isSelected = false;
                    OnFreshSonOpen(item, false);
                    n_layerSeleMap[item.info.layer] = null;

                }


            }
            else if (tParent.info.multiple)
            {
                //多选
                if (!item.isSelected || bForce)
                {
                    //开启
                    if (!item.isSelected)
                    {

                        tParent.selectIndex.Add(item.info.index);
                        item.isSelected = true;

                    }

                }
                else
                {
                    //取消
                    for (int i = 0; i < tParent.selectIndex.Count; i++)
                    {
                        if (tParent.selectIndex[i] == item.info.index)
                        {
                            tParent.selectIndex.RemoveAt(i);
                            break;
                        }

                    }

                    item.isSelected = false;
                }
            }
            else
            {
                if (!item.isSelected || bForce)
                {

                    //取消旧的
                    if (tParent.selectIndex.Count > 0)
                    {
                        for(int i = 0;i< tParent.selectIndex.Count;i++)
                        {
                            int oldIndex = tParent.selectIndex[i];
                            TreeItem oldItem = n_totalItem[tParent.info.list[oldIndex].key];
                            oldItem.isSelected = false;
                            OnFreshSonOpen(oldItem, false);

                            //callLua
                            if (callLua) CallSelectedFun(oldItem, false);
                        }
                       
                      
                    }
                    tParent.selectIndex.Clear();
                    //开启新的
                    tParent.selectIndex.Add(item.info.index);
                    item.isSelected = true;
 
                }
                else
                {
                    //关闭
                    item.isSelected = false;
                    tParent.selectIndex.Clear();

                }
            }

            //根据选中刷新开放
            OnFreshSonOpen(item, item.isSelected,false);

            //callLua
            if (callLua) CallSelectedFun(item, item.isSelected);


            if(m_scrollToSelect && item.isSelected)
            {
                OnScrollToItem(item.info.key);
            }


            //刷新渲染
            invalidDraw = true;

        }

        private void OnFreshSonOpen(TreeItem item, bool bOpen,bool openGrandson = true)
        {

            if (item.info.list != null && item.info.list.Length > 0)
            {
                for (int i = 0; i < item.info.list.Length; i++)
                {
                    TreeItem sonItem = n_totalItem[item.info.list[i].key];
                    sonItem.isOpen = bOpen;
                    if (!bOpen) sonItem.isSelected = false;

                    if (sonItem.info.list != null && sonItem.info.list.Length > 0)
                    {
                        if(bOpen)
                        { 
                            if(openGrandson)
                                OnFreshSonOpen(sonItem, bOpen, true);
                        }
                        else
                            OnFreshSonOpen(sonItem, bOpen, true);
                    }

                }
            }
        }

        private void OnFreshFatherOpen(TreeItem item, bool bOpen)
        {
            if (item.info.parent == null) return;

            TreeItem tParent = n_totalItem[item.info.parent.key];

            if (tParent.info.parent != null)
            {
                OnSelected(tParent, true);
                OnFreshFatherOpen(tParent, bOpen);
            }
        }

        private bool invaildCancelSele = false;
        /// <summary>
        /// 取消全部选中,关闭未选中的子项
        /// </summary>
        public void CancelSelected()
        {
            invaildCancelSele = true;
            invalidDraw = true;

            layerNotClickMap.Clear();
            invalid_seleIndex_step1 = null;
            invalid_seleIndex_step2 = null;
        }


        private string invalid_seleIndex_step1 = null;
        private string invalid_seleIndex_step2 = null;
        /// <summary>
        /// 强制选中哪个
        /// </summary>
        /// <param name="key">index树key: 1_2_3_4 </param>
        public void SetSeleIndex(string key)
        {                                     
            if (string.IsNullOrEmpty(key)) return;
           
            invalid_seleIndex_step1 = key;
            //invalidAutoSele = false;
        }

        private Dictionary<int, bool> layerNotClickMap = new Dictionary<int, bool>();
        public void LayerNotClick(int layer,bool sele)
        {
            if(!layerNotClickMap.ContainsKey(layer))
            {
                layerNotClickMap[layer] = sele;
            }
        }


        #endregion

        #region 对象池
        private Dictionary<int, List<GameObject>> loops = new Dictionary<int, List<GameObject>>();
        private List<TreeItem> loops2 = new List<TreeItem>();
        private GameObject PopInstance(int layer, out bool bCreateObj)// 0 - n
        {
            GameObject obj = null;
            List<GameObject> list = null;
            bCreateObj = false;
            if (!loops.TryGetValue(layer, out list))
            {
                list = new List<GameObject>();
                loops.Add(layer, list);
            }

            if (list.Count > 0)
            {
                obj = list[0];
                list.RemoveAt(0);
            }
            else
            {

                GameObject template = GetTemplate(layer).template;

                obj = GameObject.Instantiate(template);
                RectTransform rtform = obj.GetComponent<RectTransform>();
                rtform.anchorMin = new Vector2(0, 1);
                rtform.anchorMax = new Vector2(0, 1);
                rtform.pivot = new Vector2(0, 1);
                bCreateObj = true;
            }

            return obj;
        }

        private void PushInstance(GameObject obj, int layer)
        {
            List<GameObject> list = null;

            if (!loops.TryGetValue(layer, out list))
            {
                list = new List<GameObject>();
                loops.Add(layer, list);
            }

            obj.SetActive(false);
            list.Add(obj);

        }


        private TreeItem GetPoolItem()
        {
            TreeItem item = null;
            if (loops2.Count > 0)
            {
                item = loops2[0];
                loops2.RemoveAt(0);
            }
            else
                item = new TreeItem();

            return item;
        }

        private void PushItem(TreeItem item)
        {
            if (item.gameObject != null)
            {
                PushInstance(item.transfrom.gameObject, item.info.layer);
            }

            item.Hide();
            item.info = null;

            loops2.Add(item);
        }

        #endregion

        #region 计算布局

        public void ForceCountLayer()
        {
            invalidDraw = true;
        }

        //根据标签的开放情况，计算绝对坐标
        private Vector2 total_size;//Content
        private void CountAbsoluteCoordinates()
        {
            total_size = new Vector2(m_left, m_top);

            for (int i = 0; i < n_countKeys.Count; i++)
            {

                TreeItem item = n_totalItem[n_countKeys[i]];

                if (!item.isOpen) continue;

                if (layout == Layout.Vertical)
                {

                    item.fPosition = new Vector2(0, -total_size.y);
                    if (item.with + m_left > total_size.x) total_size.x = item.with + m_left;

                    total_size.y += (item.height + item.gap.y);
                }
                else if (layout == Layout.Horizontal)
                {
                    item.fPosition = new Vector2(total_size.x, 0);
                    total_size.x += (item.with + item.gap.x);
                    if (item.height + m_top > total_size.y) total_size.y = item.height + m_top;

                }
                if (m_needEffect && item.info.layer == 1)
                    item.ApplyDotweenPos();
                else
                    item.ApplyPos();
            }

            total_size.x += m_right;
            total_size.y += m_botton;

            m_content.sizeDelta = total_size;
        }

        #endregion

        #region 可视化处理
        private float olddelateY = 100;
        private float dragYType = 1;//1.下 2.上
        private void OnScrollValueChanged(Vector2 delate)
        {                     
            if (n_totalItem.Count == 0) return;
            CalculationRect();

            if (delate.y > olddelateY)
                dragYType = 1;
            else
                dragYType = 2;

             olddelateY = delate.y;        
            invalidView = true;
            if (m_scrollRect.PostionOffes >= 0.35f)
                scrollDrag = true;
            else
                scrollDrag = false;
        }

        //一个左上角为 0，0点的区域,子项坐标在这个范围内的，为可显示
      
        private void CalculationRect()
        {
           // float dx = m_content.anchoredPosition.x;
           // float dy = m_content.anchoredPosition.y;
            float w = m_transform.sizeDelta.x;//ViewPort 自适应 m_transform 的大小
            float h = m_transform.sizeDelta.y;

            s_viewRect.xMin = 0;
            s_viewRect.xMax = w;
            s_viewRect.yMin = 0;
            s_viewRect.yMax = -h;
           
           
        }

        private void OnVisualization()
        {
            //子项的绝对坐标在 s_viewRect 内的为可视化对象

            if(dragYType == 1)
            {
                for (int i = 0; i < n_countKeys.Count; i++)
                {
                    TreeItem item = n_totalItem[n_countKeys[i]];
                    OnVisualizationItem(item);
                }
            }
            else
            {
                for(int i = n_countKeys.Count -1;i>=0;i--)
                {
                    TreeItem item = n_totalItem[n_countKeys[i]];
                    OnVisualizationItem(item);
                }
            }
           
        }


        private bool OnVisualizationItem(TreeItem item)
        {
            //子项的绝对坐标在 s_viewRect 内的为可视化对象                                                
            if (!item.isOpen)
            {
                if (item.visible)
                {
                    GameObject obj = item.transfrom.gameObject;
                    PushInstance(obj, item.info.layer);
                    item.Hide();

                    //callLua
                    if (onCircleItem != null) onCircleItem.Invoke(obj.GetInstanceID());
                }

                return false;
            }                                                                

            if (inViewRect(item))
            {
                //显示
                bool bset = false;
                if (!item.visible)
                {
                    bool bCreate = false;
                    GameObject obj = PopInstance(item.info.layer, out bCreate);
                    obj.name = "Item-" + item.info.key;        
                    item.Show(obj, m_content,m_needEffect, s_viewRect, scrollDrag);
                    item.onClick = OnItemClick;

                    bset = true;


                }
                else if (item.needFresh)
                {
                    item.needFresh = false;
                    item.gameObject.name = "Item-" + item.info.key;
                    item.isSelected = item.b_Selected;

                    bset = true;

                }

                if (bset)
                {
                    //callLua
                    //激活对应的Lua Renderer
                    if (onShowItem != null) onShowItem.Invoke(item.info.key);
                    if (item.b_Selected) CallSelectedFun(item, true);
                    if (m_scrollToSelect && item.isSelected) OnScrollToItem(item.info.key);

                    invalidDraw = true;//渲染后如果要开启大小自适应，还需要计算一遍布局
                }

            }
            else
            {
                if (item.visible)
                {
                    //隐藏

                    //callLua
                    // 对应lua回收一个ItemRenderer
                    if (onCircleItem != null) onCircleItem.Invoke(item.transfrom.GetInstanceID());

                    PushInstance(item.transfrom.gameObject, item.info.layer);
                    item.Hide();
                }

            }

            return true;
 
        }

        private bool inViewRect(TreeItem item)
        {
            //转换成ViewPort空间坐标
            Vector2 position = m_content.anchoredPosition;
            position.x += item.fPosition.x;
            position.y += item.fPosition.y;

            bool showH = position.x >= s_viewRect.xMin - item.with && position.x <= s_viewRect.xMax;
            bool showV = position.y <= s_viewRect.yMin + item.height && position.y >= s_viewRect.yMax;

            return showH && showV;
        }
        #endregion

        #region 滑动设置
        private string invaild_scrollToItm;
        private bool invaild_scrollBotton;
        private bool invaild_scrollTop;
        private float smoothTime = 0;
        public void OnScrollToItem(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            invaild_scrollToItm = key;                             
        }
        public void ScrollToTop(float smothTime = 0.1f)
        {
            invaild_scrollTop = true;
            smoothTime = smothTime;    
        }

        public void ScrollToBottom(float smothTime = 0.1f)
        {
            invaild_scrollBotton = true;
            smoothTime = smothTime;    
        }


        #endregion
    }
}