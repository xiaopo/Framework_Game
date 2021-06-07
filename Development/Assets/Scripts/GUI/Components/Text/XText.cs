using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XGUI
{
	public class XText : Text
	{
        private static Color grayColor = new Color(225 / 255.0f, 225 / 255.0f, 225 / 255.0f, 1);
        private static Color grayOutlineColor = new Color(122 / 255.0f, 122 / 255.0f, 122 / 255.0f, 1);
        private static Color VoidColor = new Color(0, 0, 0, 0);
        /// <summary>
        /// 是否静态字体
        /// </summary>
        [SerializeField]
        public bool isStatic = false;
        [SerializeField]
        public int languageId = 0;

        [HideInInspector]
        public Color defaultColor = Color.white;
        [SerializeField]
        private bool m_IsCanSortingMask = false;



        private Outline _outline;
        public Outline OutLine
        {
            get
            {
                if (this._outline == null)
                    this._outline = gameObject.GetComponent<Outline>();
                return _outline;
            }
        }

        private GradientEffect m_gradientEffect;
        public GradientEffect GradientEffect
        {
            get
            {
                if (this.m_gradientEffect == null)
                    this.m_gradientEffect = gameObject.GetComponent<GradientEffect>();
                return m_gradientEffect;
            }
        }

        private Color m_CacheColor;
        private Color _outlineCacheColor;
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
        public override string text
        {
            get
            {
                return m_Text;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    if (String.IsNullOrEmpty(m_Text))
                        return;
                    m_Text = "";
                }
                else if (m_Text != value)
                {
                    m_Text = value;
                }
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            //如果是静态则替换为zh_cn文件对应字符串
            //如果是动态则删除

            if(Application.isPlaying && isStatic && languageId != 0)
            {
                string str = CSharpLuaInterface.GetLanguage(languageId);
                if (!string.IsNullOrEmpty(str))
                    m_Text = str;
            }
            m_CacheColor = this.color;
            if (OutLine)
            {
                _outlineCacheColor = OutLine.effectColor;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateClipParent();
        }

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();

            if (!IsActive() || string.IsNullOrEmpty(m_Text))
                return;

            UnityEngine.Profiling.Profiler.BeginSample("XText.Space");
            m_Text = Space(m_Text);
            UnityEngine.Profiling.Profiler.EndSample();
        }


        static string Space(string str)
        {
            bool isSpace = false;
            for (int i = 0; i < str.Length; i++)
                if (str[i] == ' ')
                {
                    isSpace = true;
                    break;
                }
            return isSpace ? str.Replace(" ", "\u00A0") : str;
        }

        public void SetGray(bool res)
        {
            if(this.OutLine != null)
            {   
                if (res)
                {
                    if (this.OutLine.effectColor != XText.grayOutlineColor) 
                    this._outlineCacheColor = this.OutLine.effectColor;
                }
                if (this._outlineCacheColor == VoidColor)
                {
                    this._outlineCacheColor = this.OutLine.effectColor;
                }

                this.OutLine.effectColor = res ? XText.grayOutlineColor : this._outlineCacheColor;
            }
            if (this.GradientEffect != null)
            {
                this.GradientEffect.SetGray(res);
            }

            if (this.m_CacheColor == VoidColor)
            {
                this.m_CacheColor = this.color;
            }
            base.color = res ? XText.grayColor : this.m_CacheColor;
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

            // don't re-add it if the newparent is inactive
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
