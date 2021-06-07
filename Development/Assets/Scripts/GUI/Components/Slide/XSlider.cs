using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XGUI
{
    public class XSlider : Slider, XIEvent
    {
        public void ClearEvent()
        {
            if (onValueChanged != null)
            {
                onValueChanged.RemoveAllListeners();
                onValueChanged = null;
            }
        }

        /// <summary>
        /// 不触发 OnValueChanged
        /// </summary>
        public float Value2
        {
            set { this.Set(value, false); }
            get { return this.value; }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.ClearEvent();
        }

    }
}


