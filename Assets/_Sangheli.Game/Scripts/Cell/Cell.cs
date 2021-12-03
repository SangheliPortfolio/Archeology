using Sangheli.Config;
using Sangheli.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sangheli.Game
{
	public class Cell : AbstractCell
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private ConfigCell configCell;
		private int currentState;
		private int maxState;

		private EventController eventController;

		private bool targetCollected;
		private int targetLayer = -1;
		private AbstractTarget currentTarget;

		private Camera _camera;

		public override void Init(ConfigCell configCell,int cellSize = -1)
		{
			this._camera = Camera.main;
			this.configCell = configCell;
			this.eventController = EventController.GetInstance();

			if (cellSize < 0 || cellSize > configCell.cellStepCount)
				cellSize = configCell.cellStepCount;

			this.maxState = cellSize;
			this.currentState = cellSize;
			this.UpdateVisual(this.currentState);
		}

		public override void UpdateVisual(int state = -1)
		{
			this.spriteRenderer.sprite = this.configCell.GetSprite(state);
		}

		void OnMouseDown()
		{
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (!this.IsGameEnabled())
				return;

			if (this.IsTargetActive())
				return;

			this.currentState--;
			if (this.currentState < 1)
			{
				return;
			}

			this.eventController.onCellClicked?.Invoke();
			this.UpdateVisual(this.currentState);
			this.CheckLayerForTarget();
		}

		private bool IsGameEnabled()
		{
			System.Func<bool> func = this.eventController.isGameEnabled;
			if (func != null)
			{
				if (!func.Invoke())
				{
					return false;
				}
			}

			return true;
		}

		private void CheckLayerForTarget()
		{
			if (!this.targetCollected && this.currentState == this.targetLayer)
			{
				if (this.currentTarget != null)
					return;

				System.Func<AbstractTarget> funcCreateTarget = this.eventController.createTarget;
				if (funcCreateTarget != null)
				{
					this.currentTarget = funcCreateTarget.Invoke();

					var viewportPosition = this._camera.WorldToViewportPoint(transform.position);
					var screenPos = this._camera.ViewportToScreenPoint(viewportPosition);
					this.currentTarget.SetRectPosition(screenPos);

					this.currentTarget.onCollect += this.CollectTarget;
				}
			}
		}

		private bool IsTargetActive()
		{
			if (!this.targetCollected && this.currentState == this.targetLayer)
			{
				if (this.currentTarget != null)
					return true;
			}

			return false;
		}

		public override void SetTarget()
		{
			this.targetLayer = Random.Range(1, this.maxState);
		}

		public void CollectTarget()
		{
			this.targetCollected = true;
			this.targetLayer = -1;
			this.currentTarget.onCollect -= this.CollectTarget;
		}
	}
}