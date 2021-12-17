using System;
using Sangheli.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sangheli.Target
{
    public class Target : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform rectTransform;

        private EventController _eventController;
        private Vector3 diffVector;
        private Vector2 lastMousePosition;
        public Action onCollect;

        private Vector2 rectSize;

        private Vector3 startPos;

        public void Init(EventController eventController)
        {
            _eventController = eventController;
        }
        
        private void Start()
        {
            rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            diffVector = new Vector3(rectTransform.rect.width / 2, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = rectTransform.position;
            lastMousePosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var currentMousePosition = eventData.position;
            var diff = currentMousePosition - lastMousePosition;

            var newPosition = rectTransform.position + new Vector3(diff.x, diff.y, transform.position.z);
            var oldPos = rectTransform.position;
            rectTransform.position = newPosition;

            if (!IsRectTransformInsideSreen(rectTransform)) rectTransform.position = oldPos;

            lastMousePosition = currentMousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var overlapTarget = IsOverlapTarget();
            if (overlapTarget)
            {
                onCollect?.Invoke();
                _eventController.onCollectTarget?.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                rectTransform.position = startPos;
            }
        }

        public void SetRectPosition(Vector2 pos)
        {
            rectTransform.position = pos;
        }

        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            var isInside = false;
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var visibleCorners = 0;
            var rect = new Rect(0, 0, Screen.width, Screen.height);
            
            foreach (var corner in corners)
                if (rect.Contains(corner))
                    visibleCorners++;

            if (visibleCorners == 4) isInside = true;
            return isInside;
        }

        private bool IsOverlapTarget()
        {
            var func = _eventController.getTargetRect;
            if (func != null)
            {
                var rect1 = new Rect(rectTransform.position - diffVector, rectSize);
                var rect2 = func.Invoke();
                return rect1.Overlaps(rect2);
            }

            return false;
        }
    }
}