using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGUI
{
    public class XSpriteMaskOrder:MonoBehaviour
    {
        private SpriteMask m_spriteMask;
        private SpriteMask spriteMask
        {
            get
            {
                if (m_spriteMask == null)
                {
                    m_spriteMask = GetComponent<SpriteMask>();
                }
                return m_spriteMask;
            }
        }

        // Use this for initialization
        void Start()
        {
            UpdateSortingOrder();
        }

        public void UpdateSortingOrder()
        {
            Canvas rcanvas = GetRootCanvas();
            int rootOrder = 0;
            if (null != rcanvas)
            {
                rootOrder = rcanvas.sortingOrder;
            }

            spriteMask.isCustomRangeActive = true;
            spriteMask.frontSortingOrder = rootOrder + 50;
            spriteMask.backSortingOrder = rootOrder - 1;
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
