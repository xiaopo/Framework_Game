using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace XGUI
{
    /// <summary>
    /// 多视图切换创建
    /// </summary>
    public class XViewport : MonoBehaviour
    {
        public class ViewportInfo
        {
            private string m_AssetName;
            public string AssetName
            {
                get { return m_AssetName; }
                set { m_AssetName = value; }
            }
            private GameObject m_GameObject;
            public UnityEngine.GameObject GameObject
            {
                get { return m_GameObject; }
                set { m_GameObject = value; }
            }

            private RectTransform m_RectTransform;
            public UnityEngine.RectTransform RectTransform
            {
                get
                {
                    if (this.m_RectTransform == null)
                    {
                        this.m_RectTransform = GameObject.GetComponent<RectTransform>();
                        if (this.m_RectTransform == null)
                            this.m_RectTransform = GameObject.AddComponent<RectTransform>();
                    }
                    return m_RectTransform;
                }
            }

            private CanvasGroup m_CanvasGroup;
            public UnityEngine.CanvasGroup CanvasGroup
            {
                get
                {
                    if (m_CanvasGroup == null)
                        m_CanvasGroup = GameObject.AddComponent<CanvasGroup>();
                    return m_CanvasGroup;
                }
            }
        }

        public enum ActiveType
        {
            Active,
            Layer,
        }

        public enum LoadType
        {
            AssetManagement,
            Resources,
        }

        [SerializeField]
        public class ViewportChangeEvent : UnityEvent<string, GameObject> { }
        private ViewportChangeEvent m_OnChange = new ViewportChangeEvent();
        public XViewport.ViewportChangeEvent onChange { get { return m_OnChange; } }

        static ObjectPool<Dictionary<string, ViewportInfo>> s_Pool = new ObjectPool<Dictionary<string, ViewportInfo>>();
        private Dictionary<string, ViewportInfo> m_Views;
        private Transform m_Transform;
        [SerializeField]
        private LoadType m_LoadType = LoadType.AssetManagement;


        [SerializeField]
        private bool m_FadeTween = true;

        public bool fadeTween
        {
            get { return m_FadeTween; }
            set { m_FadeTween = value; }
        }

        [SerializeField]
        private bool m_WaitComplete = true;

        public bool waitComplete
        {
            get { return m_WaitComplete; }
            set { m_WaitComplete = value; }
        }


        [SerializeField]
        private float m_TweenTime = 0.1f;
        public float TweenTime
        {
            get { return m_TweenTime; }
            set { m_TweenTime = value; }
        }

        [SerializeField]
        private ActiveType m_ActiveType = ActiveType.Active;
        public XViewport.ActiveType activeType
        {
            get { return m_ActiveType; }
            set { m_ActiveType = value; }
        }
    
        private bool m_Loading = false;
        public bool loading { get { return m_Loading; } }

        private string m_LastView = string.Empty;
        public string lastView { get { return m_LastView; } }

        private string m_ActiveView = string.Empty;
        public string activeView
        {
            get { return m_ActiveView; }
            set
            {
                if (m_ActiveView == value)
                    return;

                if (string.IsNullOrEmpty(value))
                {
                    Debug.LogError("XViewport:activeView  value IsNullOrEmpty ");
                    return;
                }

                if (this.m_Views == null)
                    this.m_Views = s_Pool.Get();

                ViewportInfo lastInfo = GetViewInfo(m_ActiveView);
                if (lastInfo != null && lastInfo.GameObject != null)
                {
                    this.m_LastView = this.m_ActiveView;
                }



                ViewportInfo info;
                if (!this.m_Views.TryGetValue(value, out info))
                {
                    info = new ViewportInfo();
                    info.AssetName = value;
                    this.m_Views.Add(value, info);
                }

                this.m_ActiveView = value;
                StopAllCoroutines();
                if (info.GameObject)
                {
                    SelectChange();
                    return;
                }

                StartCoroutine(LoadViewCoroutine());
            }
        }

        void Start()
        {
            m_Transform = transform;
        }

        IEnumerator LoadViewCoroutine()
        {
            yield return 0;

            this.m_Loading = true;

            string assetName = this.m_ActiveView;

            if (m_LoadType == LoadType.AssetManagement)
            {
                AssetManagement.AssetLoaderParcel async = AssetManagement.AssetUtility.LoadAsset<GameObject>(assetName);

                if (async == null) yield break;
 
                yield return async;

                this.m_Loading = false;
                if (assetName == this.m_ActiveView)
                {
                    if (async.isFailed)
                    {
                        GameObject go = async.Instantiate<GameObject>(m_Transform);
                        ViewportInfo info = GetViewInfo(this.m_ActiveView);
                        info.GameObject = go;
                        SelectChange();
                    }
                }
            }
            else
            {
                ResourceRequest async = Resources.LoadAsync<GameObject>(assetName);
                yield return async;
                this.m_Loading = false;
                if (assetName == this.m_ActiveView)
                {
                    if (async.asset != null)
                    {
                        GameObject go = Instantiate<GameObject>((GameObject)async.asset, m_Transform);
                        ViewportInfo info = GetViewInfo(this.m_ActiveView);
                        info.GameObject = go;
                        SelectChange();
                    }
                }
            }
        }

        public ViewportInfo GetViewInfo(string name)
        {
            return this.m_Views.ContainsKey(name) && !string.IsNullOrEmpty(name) ? this.m_Views[name] : null;
        }

        void SelectChange()
        {
            ViewportInfo last = GetViewInfo(lastView);
            ViewportInfo active = GetViewInfo(activeView);
            if (!this.fadeTween)
            {
                if (activeType == ActiveType.Active)
                {
                    //if (last != null && last.GameObject)
                    //    last.GameObject.SetActive(false);

                    //if (active != null && active.GameObject)
                    //    active.GameObject.SetActive(true);

                    if (last != null && last.GameObject)
                        last.RectTransform.localPosition = new Vector3(-5000,-5000,0);

                    if (active != null && active.GameObject)
                        active.RectTransform.localPosition = Vector3.zero;
                }
                else
                {
                    if (last != null && last.GameObject)
                        last.RectTransform.SetLayer(XDefine.s_HideLayer);

                    if (active != null && active.GameObject)
                        active.RectTransform.SetLayer(XDefine.s_UILayer);
                }
                this.m_OnChange.Invoke(activeView, active.GameObject);
            }
            else
            {
                if (last != null && last.GameObject)
                {
                    last.CanvasGroup.DOKill();
                    last.CanvasGroup.DOFade(0, this.m_TweenTime).OnComplete(() =>
                    {
                        if (activeType == ActiveType.Active)//last.GameObject.SetActive(false);
                            last.RectTransform.localPosition = new Vector3(-5000, -5000, 0);
                        else
                            last.RectTransform.SetLayer(XDefine.s_HideLayer);
                    });
                }

                if (active != null && active.GameObject)
                {
                    active.CanvasGroup.DOKill();
                    active.CanvasGroup.DOFade(1, this.m_TweenTime).OnComplete(() =>
                    {
                        if (activeType == ActiveType.Active)//active.GameObject.SetActive(true);
                            active.RectTransform.localPosition = Vector3.zero;
                        else
                            active.RectTransform.SetLayer(XDefine.s_UILayer);
                        if (waitComplete)
                        {
                            try
                            {
                                this.m_OnChange.Invoke(activeView, active.GameObject);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }
                    });
                    if (!waitComplete)
                    {
                        try
                        {
                            this.m_OnChange.Invoke(activeView, active.GameObject);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                    active.RectTransform.SetAsLastSibling();
                }
            }
        }

        void OnDestroy()
        {
            if (this.m_Views != null)
            {
                foreach (var view in this.m_Views)
                {
                    if (view.Value.GameObject)
                    {
                        if (m_LoadType == LoadType.AssetManagement)
                            AssetManagement.AssetUtility.DestroyInstance(view.Value.GameObject);
                        else
                            Object.Destroy(view.Value.GameObject);
                    }

                }
                this.m_Views.Clear();
                s_Pool.Release(this.m_Views);
                this.m_Views = null;
            }
            m_OnChange.RemoveAllListeners();
            m_OnChange = null;
        }


        //string[] tempviews = new string[] { "GUI_Default_View.prefab", "AExampleView.prefab", "AExampleView (1).prefab", "AExampleView (2).prefab", "AExampleView (3).prefab" };
        //public void OnGUI()
        //{
        //    if (GUILayout.Button("init", GUILayout.Width(200), GUILayout.Height(100)))
        //    {
        //        AssetManagement.AssetManager.Instance.Initialize(new GameLoaderOptions());
        //    }


        //    if (GUILayout.Button("Change", GUILayout.Width(200), GUILayout.Height(100)))
        //    {
        //        activeView = tempviews[Random.Range(0, tempviews.Length)];
        //    }
        //}

    }
}
