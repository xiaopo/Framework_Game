using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
namespace XGUI
{
    public class XTouchRotTarget : MonoBehaviour, IDragHandler
    {
        public Transform target;
        public float speed = 3f;
        public float time = 1f;
        public Ease ease = Ease.OutExpo;
        Tweener m_tween;

        public void OnDrag(PointerEventData eventData)
        {
            if (null == target) return;

            float tv = eventData.delta.x;//< 0 ? -1 : 1;
            Vector3 end = new Vector3(0, -tv * speed, 0);
            target.DOLocalRotate(end, time, RotateMode.LocalAxisAdd).SetEase(ease);

            //Vector3 end = new Vector3(0, target.eulerAngles.y + -tv * speed, 0);
            //if (null == m_tween || !m_tween.IsActive())
                //m_tween = target.DOLocalRotate(end, time, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            //else
            //    m_tween.ChangeEndValue(end, true).SetEase(Ease.Linear); ;
        }

    }
}
