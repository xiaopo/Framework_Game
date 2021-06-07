using UnityEngine;
using UnityEngine.UI;     
namespace XGUI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class XImagePolygon : XImage
    {
        private PolygonCollider2D areaPolygon;          

        protected XImagePolygon()
        {
            useLegacyMeshGeneration = true;
        }

        private PolygonCollider2D Polygon
        {
            get
            {
                if (areaPolygon != null)
                    return areaPolygon;

                areaPolygon = GetComponent<PolygonCollider2D>();
                return areaPolygon;
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector3 point;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);                

            return Polygon.OverlapPoint(point);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.localPosition = Vector3.zero;
            var w = rectTransform.sizeDelta.x * 0.5f + 0.1f;
            var h = rectTransform.sizeDelta.y * 0.5f + 0.1f;
            Polygon.points = new[]
            {
            new Vector2(-w, -h),
            new Vector2(w, -h),
            new Vector2(w, h),
            new Vector2(-w, h)
        };
        }
#endif
    }
}