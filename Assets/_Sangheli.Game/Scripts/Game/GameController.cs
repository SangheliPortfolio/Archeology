using Sangheli.Config;
using Sangheli.Event;
using Sangheli.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Game
{
	public class GameController : MonoBehaviour
	{
		[SerializeField]
		private AbstractMapLoader mapLoader;

		[SerializeField]
		private ConfigGame configGame;

		private EventController eventController;

		private int currentTargetCount;
		private int currentShovelCount;

		private bool isGameEnabled;

		private void Awake()
		{
			this.eventController = EventController.GetInstance();
		}

		private void Start()
		{
			this.eventController.onStartGameClick += this.mapLoader.SpawnField;
			this.eventController.onStartGameClick += this.InitUI;
			
			this.eventController.onCollectTarget += this.CollectTarget;
			this.eventController.onCellClicked += this.SpendShovel;

			this.isGameEnabled = true;

			this.eventController.isGameEnabled += () => this.isGameEnabled;
		}

		private void OnDestroy()
		{
			this.eventController.onStartGameClick -= this.mapLoader.SpawnField;
			this.eventController.onStartGameClick -= this.InitUI;

			this.eventController.onCollectTarget -= this.CollectTarget;
			this.eventController.onCellClicked -= this.SpendShovel;

			this.eventController.isGameEnabled -= () => this.isGameEnabled;
		}

		private void CollectTarget()
		{
			this.currentTargetCount++;
			this.UpdateTargetCount(this.currentTargetCount);

			if (this.currentTargetCount >= this.configGame.maxTargetCount)
				this.EndGameWin();
		}

		private void SpendShovel()
		{
			this.currentShovelCount--;

			if (currentShovelCount < 0)
			{
				return;
			}

			this.UpdateShovelCount(this.currentShovelCount);

			if (currentShovelCount == 0)
			{
				this.EndGameLose();
				return;
			}
		}

		private void EndGameWin()
		{
			this.isGameEnabled = false;
		}

		private void EndGameLose()
		{
			this.isGameEnabled = false;
		}

		private void InitUI()
		{
			this.currentShovelCount = this.configGame.startShovelCounter;
			this.UpdateShovelCount(this.currentShovelCount);
			this.UpdateTargetCount(this.currentTargetCount);
		}

		private void UpdateShovelCount(int count)
		{
			this.eventController.onShovelCountUpdate?.Invoke(count);
		}

		private void UpdateTargetCount(int count)
		{
			this.eventController.onTargetCountUpdate?.Invoke(count);
		}
	}
}