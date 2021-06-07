using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using AssetManagement;
using System.Collections.Generic;

namespace XGUI
{
    public class XImage : Image
    {
        static Color s_DefulatColor = new Color(1, 1, 1, 0);
        private bool m_LoadTag = false;
        private Sprite m_RawSprite;
        private Color m_CacheColor;
        [SerializeField]
        private string m_SpriteAssetName;
        private string m_CurSpriteAssetName;
        [SerializeField]
        private string m_ImageUrl;
        [SerializeField]
        private bool m_ChangeClearOld = true;
        [SerializeField]
        private bool m_SetNativeSize = true;
        [SerializeField]
        private bool m_Visible = true;
        [SerializeField]
        private bool m_WaitFrame = false;
        [SerializeField]
        private bool m_IsCanSortingMask = false;

        private bool m_DefRaycastTarget;
        public UnityAction onComplete;
        public bool autoSetNativeSize { get { return m_SetNativeSize; } set { m_SetNativeSize = value; } }

        protected override void Awake()
        {
            base.Awake();
            this.m_DefRaycastTarget = raycastTarget;
            this.m_CacheColor = color;
        }

        protected override void Start()
        {
            base.Start();
            if (Application.isPlaying)
            {
                //没有放图片的Image直接透明
                if (sprite == null && base.color.a == 1)
                    base.color = s_DefulatColor;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateClipParent();
            if (m_CurSpriteAssetName != m_SpriteAssetName)
            {
                if (!string.IsNullOrEmpty(m_SpriteAssetName))
                {
                    //StartCoroutine(LoadAsync());
                    //ClearSprite();
                    if (IsActive())
                        base.color = s_DefulatColor;

                    StartLoad();

                }
            }
        }



        public bool changeClearOld
        {
            get { return m_ChangeClearOld; }
            set { m_ChangeClearOld = value; }
        }


        public override Color color
        {
            get
            {

                return base.color;
            }
            set
            {
                this.m_CacheColor = value;
                base.color = value;
            }
        }

        public override bool raycastTarget
        {
            get { return base.raycastTarget; }
            set
            {
                this.m_DefRaycastTarget = value;
                if (m_Visible)
                    base.raycastTarget = value;
            }
        }

        public virtual bool visible
        {
            get { return m_Visible; }
            set
            {
                if (value == m_Visible)
                    return;
                m_Visible = value;
                base.raycastTarget = this.m_DefRaycastTarget;
                SetAllDirty();
            }
        }


        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (m_Visible)
                base.OnPopulateMesh(vh);
            else
                vh.Clear();
        }


        public string spriteAssetName
        {
            get { return m_SpriteAssetName; }
            set
            {
                if (value == m_SpriteAssetName)
                    return;

                if (string.IsNullOrEmpty(value))
                {
                    m_SpriteAssetName = value;
                    ClearSprite();
                    base.color = this.m_CacheColor;
                    return;
                }

                if (m_ChangeClearOld)
                    ClearSprite();

                m_SpriteAssetName = value;

                //if (!m_LoadTag)
                //{
                //    if (!IsActive())
                //        return;
                //    m_LoadTag = true;
                //StopAllCoroutines();
                //StartCoroutine(LoadAsync());

                //}

                StartLoad();
            }
        }

        public string imageUrl
        {
            get { return m_ImageUrl; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Equals(m_ImageUrl))
                    return;

                m_ImageUrl = value;

                if (!m_LoadTag)
                {
                    if (!IsActive())
                        return;
                    m_LoadTag = true;
                    StopAllCoroutines();
                    StartCoroutine(DownLoaderAsync());
                }
            }
        }

        private int frameCount = 0;
        private bool start_load = false;
        private bool is_loading = false;
        private AssetLoaderParcel loader = null;
        private void StartLoad()
        {
            start_load = true;
            is_loading = false;
            loader = null;
        }

        private void FinishLoad()
        {
            start_load = false;
            is_loading = false;
            loader = null;
        }

        private void Update()
        {
            if (!start_load) return;
           
            if (m_WaitFrame)
            {
                if(!is_loading)
                    frameCount = Random.Range(1, 5);
    
                if (--frameCount > 0) return;
            }

            is_loading = true;

            UnityEngine.Profiling.Profiler.BeginSample("XImage.LoadAsync");
            if(loader == null) loader = AssetUtility.LoadAsset<Sprite>(this.spriteAssetName);

            UnityEngine.Profiling.Profiler.EndSample();
            //资源不存在
            if (loader == null)
            {
                string assetname = m_SpriteAssetName;
                ClearSprite();
                m_SpriteAssetName = assetname;
                FinishLoad();
                return;
            }

            if (loader.IsDone())
            {
                SetSprite(loader.GetRawObject<Sprite>());
                FinishLoad();
            }
        }


        void SetSprite(Sprite sp)
        {
            string assetname = m_SpriteAssetName;
            m_CurSpriteAssetName = m_SpriteAssetName;
            ClearSprite();
            if (sp != null)
            {
                m_SpriteAssetName = assetname;
                m_RawSprite = sp;
                sprite = m_RawSprite;
                if (m_SetNativeSize)
                    SetNativeSize();
                base.color = this.m_CacheColor;
            }
            if (onComplete != null) onComplete.Invoke();
        }

        IEnumerator DownLoaderAsync()
        {
            this.m_LoadTag = false;

            using (UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(this.m_ImageUrl))
            {

                yield return uwr.SendWebRequest();
                if (string.IsNullOrEmpty(uwr.error))
                {
                    Texture2D tex2d = UnityEngine.Networking.DownloadHandlerTexture.GetContent(uwr);
                    tex2d.Compress(false);
                    if (tex2d != null)
                    {
                        Sprite nsp = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
                        SetSprite(nsp);
                    }
                }


            }
        }

        public void ClearSprite()
        {
            if (this.m_RawSprite != null)
            {
                AssetUtility.DiscardRawAsset(this.m_RawSprite);
                this.m_RawSprite = null;
                this.m_SpriteAssetName = null;
                this.m_CurSpriteAssetName = null;
                this.sprite = null;
            }

            if (IsActive())
                base.color = s_DefulatColor;
        }

        protected override void OnDestroy()
        {
            if (this.onComplete != null)
                this.onComplete = null;

            loader = null;

            this.ClearSprite();
            base.OnDestroy();
        }

#region 加了sortingOrder也能遮罩代码，必需是2DMask

        private RectMask2D m_ParentMask;

        private void UpdateClipParent()
        {
            var newParent = (maskable && IsActive()) ? XGetRectMaskForClippable(this) : null;

            // if the new parent is different OR is now inactive
            if (m_ParentMask != null && (newParent != m_ParentMask || !newParent.IsActive()))
            {
                m_ParentMask.RemoveClippable(this);
                UpdateCull(false);
            }


            if (newParent != null && newParent.IsActive())
            {
                newParent.AddClippable(this);
            }

            m_ParentMask = newParent;
        }

        private RectMask2D XGetRectMaskForClippable(IClippable clippable)
        {
            List<RectMask2D> rectMaskComponents = ListPool<RectMask2D>.Get();
            List<Canvas> canvasComponents = ListPool<Canvas>.Get();
            RectMask2D componentToReturn = null;

            clippable.rectTransform.GetComponentsInParent(false, rectMaskComponents);

            if (rectMaskComponents.Count > 0)
            {
                for (int rmi = 0; rmi < rectMaskComponents.Count; rmi++)
                {
                    componentToReturn = rectMaskComponents[rmi];
                    if (componentToReturn.gameObject == clippable.gameObject)
                    {
                        componentToReturn = null;
                        continue;
                    }
                    if (!componentToReturn.isActiveAndEnabled)
                    {
                        componentToReturn = null;
                        continue;
                    }
                    if (!m_IsCanSortingMask)
                    {
                        clippable.rectTransform.GetComponentsInParent(false, canvasComponents);
                        for (int i = canvasComponents.Count - 1; i >= 0; i--)
                        {
                            if (!MaskUtilities.IsDescendantOrSelf(canvasComponents[i].transform, componentToReturn.transform) && canvasComponents[i].overrideSorting)
                            {
                                componentToReturn = null;
                                break;
                            }
                        }
                    }
                    return componentToReturn;
                }
            }

            ListPool<RectMask2D>.Release(rectMaskComponents);
            ListPool<Canvas>.Release(canvasComponents);

            return componentToReturn;
        }


        private void UpdateCull(bool cull)
        {
            var cullingChanged = canvasRenderer.cull != cull;
            canvasRenderer.cull = cull;

            if (cullingChanged)
            {
                onCullStateChanged.Invoke(cull);
                SetVerticesDirty();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UpdateClipParent();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateClipParent();
        }

#endif
        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();
            UpdateClipParent();
        }

        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            UpdateClipParent();
        }
        #endregion

    }
}
