using System;
using Sangheli.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sangheli.Game
{
    public class Target : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action onCollect;

        [SerializeField] private RectTransform rectTransform;

        private Vector3 startPos;
        private Vector2 lastMousePosition;

        private EventController eventController;

        private Vector2 rectSize;
        private Vector3 diffVector;


        private void Start()
        {
            eventController = EventController.GetInstance();
            rectSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            diffVector = new Vector3(rectTransform.rect.width / 2, 0);
        }

        public void SetRectPosition(Vector2 pos)
        {
            rectTransform.position = pos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = rectTransform.position;
            lastMousePosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - lastMousePosition;

            Vector3 newPosition = rectTransform.position + new Vector3(diff.x, diff.y, transform.position.z);
            Vector3 oldPos = rectTransform.position;
            rectTransform.position = newPosition;

            if (!IsRectTransformInsideSreen(rectTransform))
            {
                rectTransform.position = oldPos;
            }

            lastMousePosition = currentMousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            bool overlapTarget = IsOverlapTarget();
            if (overlapTarget)
            {
                onCollect?.Invoke();
                eventController.onCollectTarget?.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                rectTransform.position = startPos;
            }
        }

        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            bool isInside = false;
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            int visibleCorners = 0;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            foreach (Vector3 corner in corners)
            {
                if (rect.Contains(corner))
                {
                    visibleCorners++;
                }
            }

            if (visibleCorners == 4)
            {
                isInside = true;
            }

            return isInside;
        }

        private bool IsOverlapTarget()
        {
            Func<Rect> func = EventController.GetInstance().getTargetRect;
            if (func != null)
            {
                Rect rect1 = new Rect(rectTransform.position - diffVector, rectSize);
                Rect rect2 = func.Invoke();
                return rect1.Overlaps(rect2);
            }

            return false;
        }
    }
}