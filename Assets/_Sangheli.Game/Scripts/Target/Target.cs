using Sangheli.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sangheli.Game
{
	public class Target : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
	{
		public System.Action onCollect;

		[SerializeField]
		private RectTransform rectTransform;

		private Vector3 startPos;
		private Vector2 lastMousePosition;

		private EventController eventController;

		private Vector2 rectSize;
		private Vector3 diffVector;


		private void Start()
		{
			this.eventController = EventController.GetInstance();
			this.rectSize = new Vector2(this.rectTransform.rect.width, this.rectTransform.rect.height);
			this.diffVector = new Vector3(this.rectTransform.rect.width / 2, 0);
		}

		public void SetRectPosition(Vector2 pos)
		{
			this.rectTransform.position = pos;
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

			Vector3 newPosition = this.rectTransform.position + new Vector3(diff.x, diff.y, transform.position.z);
			Vector3 oldPos = this.rectTransform.position;
			this.rectTransform.position = newPosition;
			
			if (!IsRectTransformInsideSreen(this.rectTransform))
			{
				this.rectTransform.position = oldPos;
			}

			this.lastMousePosition = currentMousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			bool overlapTarget = this.IsOverlapTarget();
			if (overlapTarget)
			{
				this.onCollect?.Invoke();
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

		private bool IsOverlapTarget()
		{
			System.Func<Rect> func = EventController.GetInstance().getTargetRect;
			if (func != null)
			{
				Rect rect1 = new Rect(this.rectTransform.position - this.diffVector, this.rectSize);
				Rect rect2 = func.Invoke();
				return rect1.Overlaps(rect2);
			}

			return false;
		}
	}
}