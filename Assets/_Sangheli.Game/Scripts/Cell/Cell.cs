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

		private EventController eventController;

		public override void Init(ConfigCell configCell,int cellSize = -1)
		{
			this.configCell = configCell;
			this.eventController = EventController.GetInstance();

			if (cellSize < 0 || cellSize > configCell.cellStepCount)
				cellSize = configCell.cellStepCount;

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

			this.currentState--;
			if (this.currentState < 1)
			{
				return;
			}

			this.eventController.onCellClicked?.Invoke();
			this.UpdateVisual(this.currentState);
		}
	}
}