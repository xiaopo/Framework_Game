using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MirrorDirection
{
    Right,
    Left,
}

[RequireComponent(typeof(Image))]
public class XMirrorImage : BaseMeshEffect
{
    //镜像相对于原有图片的位置
    public MirrorDirection m_MirrorDirection = MirrorDirection.Right;

    //原图与镜像的间隔
    public float m_Offset;

    private Image m_TargetImage;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;
        m_TargetImage = graphic as Image;
        if (m_TargetImage == null) return;
        //if (targetImage.type != Image.Type.Simple) return;
        if (vh.currentVertCount <= 0) return;
        CreateMirror(vh);

    }

    private void CreateMirror(VertexHelper vh)
    {
        Rect rect = graphic.GetPixelAdjustedRect();

        if (m_TargetImage.type == Image.Type.Sliced)
        {
            ShrinkSlicedVertex(vh, rect);
        }
        else
        {
            ShrinkVertex(vh, rect);
        }
        AddMirrorVertices(vh, rect);

    }

    private void ShrinkVertex(VertexHelper vh, Rect rect)
    {
        UIVertex uIVertex = UIVertex.simpleVert;
        switch (m_MirrorDirection)
        {
            case MirrorDirection.Right:
                //向左收缩
                for (int i = 0; i < vh.currentVertCount; i++)
                {

                    vh.PopulateUIVertex(ref uIVertex, i);
                    var pos = uIVertex.position;
                    pos.x = (rect.x + pos.x) * 0.5f - m_Offset * 0.5f;
                    uIVertex.position = pos;
                    vh.SetUIVertex(uIVertex, i);
                }
                break;
            case MirrorDirection.Left:
                //向右收缩
                for (int i = 0; i < vh.currentVertCount; i++)
                {
                    vh.PopulateUIVertex(ref uIVertex, i);
                    var pos = uIVertex.position;
                    pos.x = (rect.x + pos.x) * 0.5f + rect.width / 2 + m_Offset;

                    uIVertex.position = pos;
                    vh.SetUIVertex(uIVertex, i);
                }
                break;
        }

    }

    //缩小Sliced格式图片
    private void ShrinkSlicedVertex(VertexHelper vh, Rect rect)
    {
        UIVertex uIVertex = UIVertex.simpleVert;
        switch (m_MirrorDirection)
        {
            case MirrorDirection.Right:
                vh.PopulateUIVertex(ref uIVertex, 24);
                Vector3 pos0 = uIVertex.position;
                vh.PopulateUIVertex(ref uIVertex, 27);
                Vector3 pos3 = uIVertex.position;
                float rect_LD_width = pos3.x - pos0.x;
                for (int i = 0; i < vh.currentVertCount; i++)
                {
                    vh.PopulateUIVertex(ref uIVertex, i);
                    var pos = uIVertex.position;

                    if (IsSlicedShrinkPoint(i))
                    {
                        pos.x = rect.width * 0.5f + rect.x;
                        uIVertex.position = pos;
                        vh.SetUIVertex(uIVertex, i);

                        int offset = (i % 2 == 0) ? 11 : 9;
                        //UIVertex tempVertex = UIVertex.simpleVert;
                        vh.PopulateUIVertex(ref uIVertex, i + offset);

                        uIVertex.position = pos;
                        vh.SetUIVertex(uIVertex, i + offset);
                    }
                    else if (IsRightBordPoint(i))
                    {
                        int offset = (i % 2 == 0) ? 1 : 3;
                        UIVertex tempVertex = UIVertex.simpleVert;

                        vh.PopulateUIVertex(ref tempVertex, i - offset);
                        pos.x = tempVertex.position.x + rect_LD_width;
                        uIVertex.position = pos;
                        vh.SetUIVertex(uIVertex, i);
                    }
                    else
                    {
                        uIVertex.position = pos;
                        vh.SetUIVertex(uIVertex, i);
                    }

                }
              
                break;
            case MirrorDirection.Left:
                //TODO
                break;
        }
    }

    private void AddMirrorVertices(VertexHelper vh, Rect rect)
    {
        int index = vh.currentVertCount;

        UIVertex oldVertex = UIVertex.simpleVert;

        float axisPos = GetAxisPos(rect);
        for (int i = vh.currentVertCount - 1; i >= 0; i--)
        {
            vh.PopulateUIVertex(ref oldVertex, i);
            UIVertex mirrorVertex = GetMirrorVertex(axisPos, oldVertex, rect);
            vh.AddVert(mirrorVertex);
        }

        //三角面绘制顺序
        for (int i = index; i < vh.currentVertCount; i = i + 4)
        {
            vh.AddTriangle(i, i + 1, i + 2);
            vh.AddTriangle(i, i + 2, i + 3);
        }
    }

    private bool IsSlicedShrinkPoint(int index)
    {
        return index == 14
            || index == 15
            || index == 18
            || index == 19
            || index == 22
            || index == 23
            ;
    }

    private bool IsRightBordPoint(int index)
    {
        return index == 26
            || index == 27
            || index == 30
            || index == 31
            || index == 34
            || index == 35;

    }

    //获取镜像点
    private UIVertex GetMirrorVertex(float axisPos, UIVertex origin, Rect rect)
    {
        UIVertex mirrorVertex = origin;
        //var pos = origin.position;
        switch (m_MirrorDirection)
        {
            case MirrorDirection.Right:
                mirrorVertex.position.x = (axisPos - (-rect.x)) + axisPos - origin.position.x - (-rect.x);

                break;
            case MirrorDirection.Left:
                mirrorVertex.position.x = axisPos - (((-rect.x) + origin.position.x) - axisPos) - (-rect.x);
                break;
        }

        return mirrorVertex;
    }

    //获取中心轴
    private float GetAxisPos(Rect rect)
    {
        switch (m_MirrorDirection)
        {
            case MirrorDirection.Right:
                return rect.width / 2;
            case MirrorDirection.Left:
                return rect.width / 2;
            default:
                return 0;
        }
    }

}