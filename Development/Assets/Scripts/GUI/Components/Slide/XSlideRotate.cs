using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using XGUI;

public class XSlideRotate : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector2 centerPoint = Vector2.zero;      //中心点
    public float horizontalValue = 200;             //横长
    public float verticalValue = 200;               //竖长
    public float rotateSpeed = 1;                   //旋转速度
    public int count = 4;                           //cell数量
    public List<float> anglesList;                  //终点角度
    public List<RectTransform> cellList;                //旋转Tr

    public float rotateAngles = 0;                  //总旋转角度（方便观测使用）

    public UnityAction onBeginDragHandler;
    public UnityAction<int> onBeginTurnHandler;
    public UnityAction onEndDragHandler;

    public bool isRun = false;                     

    private float _input;                           //鼠标点击点（左右滑动判断）

    private bool isTurn = true;                            //转动限制

    private float turnProportion;

	void Awake () {
        turnProportion = 360 / count;
	}

    void Start()
    {
        
    }
	
	void Update () {
        if (isRun)
        {
            RunTar();
        }
	}

    void CellTurn(RectTransform cell, float angle)
    {
        float tagAngle = angle + rotateAngles;
        // dx = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = cos(弧度) , 弧度 = 角度 *3.14f / 180f;
        float dx = centerPoint.x + (horizontalValue / 2) * Mathf.Cos(tagAngle * Mathf.PI / 180f);
        float dy = centerPoint.y + (verticalValue / 2) * Mathf.Sin(tagAngle * Mathf.PI / 180f);
        cell.anchoredPosition = new Vector2(dx, dy);
    }

    public void AddAngles(float angle)
    {
        anglesList.Add(angle);
    }

    public void AddCell(RectTransform transform)
    {
        if (transform != null)
        {
            cellList.Add(transform);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _input = Input.mousePosition.x;
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LeftTurn(true);
        RightTurn(true);
    }

    public void RightTurn(bool isSlide)
    {
        if (isTurn)
        {
            if (isSlide)
            {
                if (Input.mousePosition.x > (_input + 5))
                {
                    RightTurnEx();
                }
            }
            else
            {
                RightTurnEx();
            }
        }
    }

    public void RightTurnEx()
    {
        isRun = true;
        isTurn = false;
        if (onBeginTurnHandler != null)
        {
            onBeginTurnHandler.Invoke(1);
        }
        DOTween.To(() => rotateAngles, x => rotateAngles = x, rotateAngles - 90, rotateSpeed).SetEase(Ease.InQuad).OnComplete(() =>
        {
            isTurn = true;
            isRun = false;
            RunTar();
            if (onEndDragHandler != null)
            {
                onEndDragHandler.Invoke();
            }
        });
    }

    public void LeftTurn(bool isSlide)
    {
        if (isTurn) 
        {
            if (isSlide)
            {
                if (Input.mousePosition.x < (_input + 5))
                {
                    LeftTurnEx();
                }
            }
            else
            {
                LeftTurnEx();
            }
        }
    }

    private void LeftTurnEx()
    {
        isRun = true;
        isTurn = false;
        if (onBeginTurnHandler != null)
        {
            onBeginTurnHandler.Invoke(2);
        }
        DOTween.To(() => rotateAngles, x => rotateAngles = x, rotateAngles + 90, rotateSpeed).SetEase(Ease.InQuad).OnComplete(() =>
        {
            isTurn = true;
            isRun = false;
            RunTar();
            if (onEndDragHandler != null)
            {
                onEndDragHandler.Invoke();
            }
        });
    }

    private void RunTar()
    {
        if (cellList.Count == count && anglesList.Count == count)
        {
            for (int i = 0; i < cellList.Count; i++)
            {
                CellTurn(cellList[i], anglesList[i]);
            }
        }
    }

    private void OnDestroy()
    {
        if (onBeginDragHandler != null)
        {
            onBeginDragHandler = null;
        }
        if (onBeginTurnHandler != null)
        {
            onBeginTurnHandler = null;
        }
        if (onEndDragHandler != null)
        {
            onEndDragHandler = null;
        }
        anglesList.Clear();
        cellList.Clear();
        rotateAngles = 0;
    }
}
