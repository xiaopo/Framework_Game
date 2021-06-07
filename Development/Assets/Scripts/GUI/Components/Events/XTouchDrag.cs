using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XGUI
{
    public class XTouchDrag : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler
    {
        public enum DragDir
        {
            None,
            Up,
            Down,
            Left,
            Right,
        }

        private Vector3 _startTouchPos;
        private DragDir _dragDir;

        public UnityAction<PointerEventData> onDrag { get; set; }
        public UnityAction<int> onEndDrag { get; set; }
        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
                onDrag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.position.y < _startTouchPos.y)
            {
                _dragDir = DragDir.Down;
            }
            else if (eventData.position.y > _startTouchPos.y)
            {
                _dragDir = DragDir.Up;
            }
            if (onEndDrag != null)
                onEndDrag.Invoke((int)_dragDir);
            _dragDir = DragDir.None;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTouchPos = eventData.position;
        }

        void OnDestroy()
        {
            onDrag = null;
        }
    }
}
