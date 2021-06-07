using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XGUI
{
    public class XTouchClick : MonoBehaviour, IPointerClickHandler
    {
        public UnityAction<PointerEventData> onClick { get; set; }

        //按下与抬起距离过远将不响应
        public float distance = -1f;
        public void OnPointerClick(PointerEventData eventData)
        {

            if (distance != -1)
            {
                if (Mathf.Abs((eventData.position - eventData.pressPosition).magnitude) > distance)
                    return;
            }

            if (onClick != null)
                onClick.Invoke(eventData);
        }

        void OnDestroy()
        {
            onClick = null;
        }
    }

}
