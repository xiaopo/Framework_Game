using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/GradientEffect")]
public class GradientEffect : BaseMeshEffect
{
    private static Color _garyTopColor = new Color(225/ 255.0f, 225 / 255.0f, 225/ 255.0f);
    private static Color _garyBottomColor = new Color(242/ 255.0f, 242 / 255.0f, 242 / 255.0f);

    public enum Direction
    {
        Horizontal,
        Vertical,
    }

    [SerializeField]
    public Direction m_Direction = Direction.Vertical;

    private Color _topCacheColor = Color.white;

    [FormerlySerializedAs("topColor")]
    [SerializeField]
    private Color _topColor = Color.white;
    public Color topColor {
        set
        {
            this._topCacheColor = value;
            this. _topColor = value;
        }
        get
        {
            return _topColor;
        }
    }


    private Color _bottomCacheColor = Color.black;

    [FormerlySerializedAs("topColor")]
    [SerializeField]
    private Color _bottomColor = Color.black;

    public Color bottomColor
    {
        set
        {
            this._bottomCacheColor = value;
            this._bottomColor = value;
        }
        get
        {
            return _bottomColor;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this._topCacheColor = this._topColor;
        this._bottomCacheColor = this._bottomColor;
    }

    public void SetGray(bool res)
    {
        this._topColor = res ? GradientEffect._garyTopColor : this._topCacheColor;
        this._bottomColor = res ? GradientEffect._garyBottomColor : this._bottomCacheColor;
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        int count = vertexList.Count;

        if (count > 0)
        {
            if (m_Direction == Direction.Vertical)
                GradientVertical(vertexList, 0, count);
            else
                GradientHorizontal(vertexList, 0, count);
            
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    private void GradientVertical(List<UIVertex> vertexList, int start, int end)
    {
        float bottomY = vertexList[0].position.y;
        float topY = vertexList[0].position.y;
        for (int i = start; i < end; ++i)
        {
            float y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float uiElementHeight = topY - bottomY;
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            uiVertex.color = Color.Lerp(_bottomColor, _topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
            vertexList[i] = uiVertex;
        }
    }


    private void GradientHorizontal(List<UIVertex> vertexList, int start, int end)
    {

        float leftX = vertexList[0].position.x;
        float rightX = vertexList[0].position.x;
        for (int i = start; i < end; ++i)
        {
            float x = vertexList[i].position.x;
            if (x > rightX)
            {
                rightX = x;
            }
            else if (x < leftX)
            {
                leftX = x;
            }
        }

        float uiElementWidth = rightX - leftX;
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];

            uiVertex.color = Color.Lerp(_topColor, _bottomColor, (uiVertex.position.x - leftX) / uiElementWidth);
            vertexList[i] = uiVertex;
        }
    }
}