using Sangheli.Config;
using Sangheli.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		private int targetLayer = -1;

		public override void Init(ConfigCell configCell,int cellSize = -1)
		{
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
			System.Func<bool> func = this.eventController.isGameEnabled;
			if (func != null)
			{
				if (!func.Invoke())
				{
					return;
				}
			}

			if(this.currentState == this.targetLayer)
			{
				return;
			}


			this.currentState--;
			if (this.currentState < 1)
			{
				return;
			}

			this.eventController.onCellClicked?.Invoke();
			this.UpdateVisual(this.currentState);
		}

		public override void SetTarget()
		{
			this.targetLayer = Random.Range(1, this.maxState);
		}

		public void CollectTarget()
		{
			this.targetLayer = -1;
		}
	}
}