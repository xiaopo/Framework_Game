using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XGUI
{
    [ExecuteInEditMode]
    public class XSortingOrder: MonoBehaviour
    {
        private Renderer[] m_renders;
        private Renderer m_render;

        public void Start()
        {
            _updateSortingOrder();
        }

        public void UpdateSortingOrder()
        {
            m_renders = null;
            _updateSortingOrder();
        }

        private void _updateSortingOrder()
        {
            Canvas rcanvas = GetRootCanvas();
            int rootOrder = 0;
            if (null != rcanvas)
            {
                rootOrder = rcanvas.sortingOrder;
            }

            if (m_isUI)
            {
                Canvas canvas = this.GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = this.gameObject.AddComponent<Canvas>();
                    this.gameObject.AddComponent<GraphicRaycaster>();
                }
                canvas.overrideSorting = true;
                canvas.sortingOrder = m_sortingOrder + rootOrder;
            }
            else
            {
                if (null != CacheRenderers)
                {
                    foreach (Renderer renderer in m_renders)
                    {
                        if (renderer && !renderer.IsNull())
                        {
                            renderer.sortingOrder = m_sortingOrder + rootOrder;
                        }
                    }
                }
            }

            //if (CacheRenderers != null)
            //{
            //    Debug.Log("子Render数量" + m_renders.Length);
            //}
            //Debug.Log("SortingOrder数量" + GetComponentsInChildren<XSortingOrder>(true).Length);
            //Debug.Log("父节点:" + rootOrder + " 子节点：" + m_sortingOrder);
        }

        public Renderer[] CacheRenderers
        {
            get
            {
                if (null == m_renders) m_renders = GetComponentsInChildren<Renderer>(true);
                return m_renders;
            }
        }

        public Renderer CacheRenderer
        {
            get
            {
                if (null == m_render) m_render = GetComponent<Renderer>();
                return m_render;
            }
        }

        [SerializeField]
        private int m_sortingOrder = 0;
        public int sortingOrder
        {
            get { return m_sortingOrder; }
            set { m_sortingOrder = value; _updateSortingOrder(); }
        }

        [SerializeField]
        private bool m_isUI = false;
        public bool isUI
        {
            get { return m_isUI; }
            set { m_isUI = value; _updateSortingOrder(); }
        }


        public Canvas GetRootCanvas()
        {
            Transform parent = transform.parent;
            Canvas rootCanvas = null;
            while (parent)
            {
                rootCanvas = parent.GetComponent<Canvas>();
                if (null != rootCanvas) return rootCanvas;
                parent = parent.parent;
            }
            return null;
        }
    }


}

