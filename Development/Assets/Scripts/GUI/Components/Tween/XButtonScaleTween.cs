using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace XGUI
{
    public class XButtonScaleTween : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float EndValue = 0.9f;
        public float Duration = 0.1f;
        private Vector3 orginScale;
        private bool isdown;
        private Selectable target;
        public bool isEnable = true;
        public UnityAction onPointerDown;
        public UnityAction onPointerUp;
        public UnityAction onPointerExit;
        public Transform handleTarget;


        private void Awake()
        {
            if (handleTarget == null)
            {
                handleTarget = this.transform;
            }

            orginScale = handleTarget.localScale;
            Selectable btn = GetComponent<Selectable>();
            if (btn != null) target = btn;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (target == null || !target.enabled || !isEnable) return;
            if (isdown)
            {
                isdown = false;
                ScaleAnim(isdown);
                if (onPointerExit != null)
                {
                    onPointerExit.Invoke();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (target == null || !target.enabled || !isEnable) return;
            if (isdown)
            {
                isdown = false;
                ScaleAnim(isdown);
                if (onPointerUp != null)
                {
                    onPointerUp.Invoke();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (target == null || !target.enabled || !isEnable) return;
            isdown = true;
            ScaleAnim(isdown);
            if (onPointerDown != null)
            {
                onPointerDown.Invoke();
            }
        }

        void ScaleAnim(bool isForward = true)
        {
            handleTarget.DOKill();
            if (isForward)
            {
                handleTarget.DOScale(orginScale * EndValue, Duration);
            }
            else
            {
                handleTarget.DOScale(orginScale, Duration);
            }
        }

        void OnDestroy()
        {
            handleTarget.DOKill();
            if (onPointerDown != null) onPointerDown = null;
            if (onPointerUp != null) onPointerUp = null;
            if (onPointerExit != null) onPointerExit = null;
        }
    }
}

