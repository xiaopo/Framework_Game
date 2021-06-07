using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XGUI
{
    public class XToggle : Toggle, XIEvent
    {
        public virtual void ClearEvent()
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
