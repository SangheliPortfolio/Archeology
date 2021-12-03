using Sangheli.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sangheli.Game
{
	public class Target : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
	{
		[SerializeField]
		private RectTransform rectTransform;

		private Vector3 startPos;
		private Vector2 lastMousePosition;

		private EventController eventController;

		private void Start()
		{
			this.eventController = EventController.GetInstance();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			this.startPos = this.rectTransform.position;
			this.lastMousePosition = eventData.position;
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 currentMousePosition = eventData.position;
			Vector2 diff = currentMousePosition - this.lastMousePosition;

			Vector3 newPosition = rectTransform.position + new Vector3(diff.x, diff.y, transform.position.z);
			Vector3 oldPos = rectTransform.position;
			rectTransform.position = newPosition;
			
			if (!IsRectTransformInsideSreen(rectTransform))
			{
				rectTransform.position = oldPos;
			}

			this.lastMousePosition = currentMousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			bool overlapTarget = IsOverlapTarget(this.rectTransform);

			if (overlapTarget)
			{
				this.eventController.onCollectTarget?.Invoke();
				gameObject.SetActive(false);
			}
			else
			{
				this.rectTransform.position = this.startPos;
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

		public static bool IsOverlapTarget(RectTransform rectTrans1)
		{
			System.Func<Rect> func = EventController.GetInstance().getTargetRect;
			if (func != null)
			{
				Rect rect1 = new Rect(rectTrans1.position, new Vector2(rectTrans1.rect.width, rectTrans1.rect.height));
				Rect rect2 = func.Invoke();
				return rect1.Overlaps(rect2);
			}

			return false;
		}
	}
}