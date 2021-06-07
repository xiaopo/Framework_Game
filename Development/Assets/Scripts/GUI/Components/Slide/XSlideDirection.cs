using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using XGUI;

public class XSlideDirection : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public UnityAction onBeginDragHandler;
    public UnityAction<bool> onDragDirectionHandler;  //IsLeft
    public UnityAction onEndDragHandler;

    private float _input;                           //鼠标点击点（左右滑动判断）
    public float move_offset = 5;       //移动范围

    void Start() { }

    void Update() { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _input = Input.mousePosition.x;
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDragHandler != null)
        {
            onEndDragHandler.Invoke();
        }
        DetectionDirection();
    }

    private void DetectionDirection()
    {
        if (onDragDirectionHandler != null)
        {
            float offset = _input + move_offset;
            if (Input.mousePosition.x > offset)
            {
                onDragDirectionHandler.Invoke(false);
            }
            else if (Input.mousePosition.x < offset)
            {
                onDragDirectionHandler.Invoke(true);
            }
        }
    }

    private void OnDestroy()
    {
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler = null;
        }
        if (onEndDragHandler != null)
        {
            onEndDragHandler = null;
        }
        if (onDragDirectionHandler != null)
        {
            onDragDirectionHandler = null;
        }
    }
}
