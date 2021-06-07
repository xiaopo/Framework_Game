using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace XGUI
{
    public class XDropDown : Dropdown, XIEvent
    {
        public void ClearEvent()
        {
            if (onValueChanged != null)
            {
                onValueChanged.RemoveAllListeners();
                onValueChanged = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.ClearEvent();
        }
    }
}
