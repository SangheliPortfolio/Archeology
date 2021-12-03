using Sangheli.Config;
using Sangheli.Event;
using Sangheli.Game;
using Sangheli.Save;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
			this.eventController.onStartGameClick += this.StartGame;

			this.eventController.onQuitAppClick += this.QuitApp;
			
			this.eventController.onCollectTarget += this.CollectTarget;
			this.eventController.onCellClicked += this.SpendShovel;

			this.eventController.isGameEnabled += () => this.isGameEnabled;

			this.eventController.writeSaveGame += this.CollectSaveGame;
			this.eventController.restoreSaveGame += this.RestoreSaveData;

			this.eventController.writeSaveField += this.CollectSaveField;
			this.eventController.restoreSaveField += this.RestoreSaveField;

			this.RestoreSaves();
		}

		private void OnDestroy()
		{
			this.eventController.onStartGameClick -= this.mapLoader.SpawnField;
			this.eventController.onStartGameClick -= this.InitUI;
			this.eventController.onStartGameClick -= this.StartGame;

			this.eventController.onQuitAppClick -= this.QuitApp;

			this.eventController.onCollectTarget -= this.CollectTarget;
			this.eventController.onCellClicked -= this.SpendShovel;

			this.eventController.isGameEnabled -= () => this.isGameEnabled;

			this.eventController.writeSaveGame -= this.CollectSaveGame;
			this.eventController.restoreSaveGame -= this.RestoreSaveData;

			this.eventController.writeSaveField -= this.CollectSaveField;
			this.eventController.restoreSaveField -= this.RestoreSaveField;
		}

		private void StartGame()
		{
			this.isGameEnabled = true;
		}

		private async void RestoreSaves()
		{
			bool restored = false;
			if (this.eventController.onAppStart != null)
				restored = await this.eventController.onAppStart.Invoke();

			if (restored)
			{
				this.UpdateShovelCount(this.currentShovelCount);
				this.UpdateTargetCount(this.currentTargetCount);
				this.isGameEnabled = true;
			}
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
			this.eventController.onGameWin?.Invoke();
		}

		private void EndGameLose()
		{
			this.isGameEnabled = false;
			this.eventController.onGameEnd?.Invoke();
		}

		private async void InitUI()
		{
			await Task.Yield();
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

		private void OnApplicationQuit()
		{
			this.eventController.onAppQuit?.Invoke();
		}

		private void QuitApp()
		{
			Application.Quit();
		}

		private SaveParameters CollectSaveGame()
		{
			SaveParameters save = new SaveParameters();
			save.name = "game";
			save.intList = new List<int>() { this.currentTargetCount, this.currentShovelCount };
			return save;
		}

		private async Task<bool> RestoreSaveData(SaveParameters save)
		{
			await Task.Yield();
			this.currentTargetCount = save.intList[0];
			this.currentShovelCount = save.intList[1];
			return true;
		}

		private SaveParameters CollectSaveField() => this.mapLoader.GetSave();

		private async Task<bool> RestoreSaveField(SaveParameters save)
		{
			await Task.Yield();
			return this.mapLoader.RestoreSave(save);
		}
	}
}