 using System.Collections;
 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites; 

namespace XGUI
{
    public class XProgressImage : XImage
    {
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            if (overrideSprite == null)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            if (type == Type.Sliced)
            {
                GenerateSlicedSprite_(toFill);
            }
        }

        Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
        {
            Rect originalRect = rectTransform.rect;
            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = rect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
                // If the rect is smaller than the combined borders, then there‘s not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];
                if (rect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = rect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }

        static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        private void GenerateSlicedSprite_(VertexHelper toFill)
        {
            Vector4 outer, inner, padding, border;

            if (overrideSprite != null)
            {
                outer = DataUtility.GetOuterUV(overrideSprite);
                inner = DataUtility.GetInnerUV(overrideSprite);
                padding = DataUtility.GetPadding(overrideSprite);
                border = overrideSprite.border;
            }
            else
            {
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }

            Rect rect = GetPixelAdjustedRect();
            border = GetAdjustedBorders(border / pixelsPerUnit, rect);
            padding = padding / pixelsPerUnit;
            float condition = (border.z + border.x) / rect.width;

            #region 实际显示size
            float[] x = { 0, 0, 0, 0 };


            x[0] = 0;
            if (fillAmount < condition)
            {
                x[1] = fillAmount / 2 * rect.width;
                x[2] = x[1] + 0;
                x[3] = x[1] * 2;
            }
            else
            {
                x[1] = border.x;
                x[2] = rect.width * fillAmount - border.z;
                x[3] = x[2] + border.z;
            }
            float[] y = { 0 + rect.y, rect.height + rect.y };

            for (int i = 0; i < 4; ++i)
            {
                x[i] += rect.x;

            }
            #endregion

            #region uv值
            float[] x_uv = { 0, 0, 0, 0 };

            x_uv[0] = outer.x;

            float curWidth = rect.width * fillAmount;
            bool isWidthLessBorder = curWidth <= border.x;

            //如果本身大小比九宫格大小小
            if (condition > 1f)
            {
                //就采样一头一尾
                x_uv[1] = inner.x / condition / 2;
                x_uv[2] = outer.z - x_uv[1];
            }
            else
            {
                //如果当前的宽度小于九宫格的左部，则需要重新取uv点，而且只绘制左部一个片体 
                if (isWidthLessBorder)
                {
                    x_uv[1] = (curWidth / border.x) * inner.x; 
                    x_uv[2] = inner.z;
                }
                else//采样全部
                {
                    x_uv[1] = inner.x;
                    x_uv[2] = inner.z; 
              }
               
            }
            x_uv[3] = outer.z;

            float y_uv = outer.w;
            #endregion

            toFill.Clear();
            for (int i = 0; i < 3; i++)
            {
                int i2 = i + 1;
                AddQuad(toFill,
                        new Vector2(x[i], y[0]),
                        new Vector2(x[i2], y[1]),
                        color,
                        new Vector2(x_uv[i], outer.y),
                        new Vector2(x_uv[i2], y_uv));

                if (isWidthLessBorder) 
                    break;  

            }
        }
    }
}

