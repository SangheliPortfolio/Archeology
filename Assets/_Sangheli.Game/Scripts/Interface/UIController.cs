using Sangheli.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sangheli.UI
{
	public class UIController : MonoBehaviour
	{
		[Header("Buttons")]
		[SerializeField]
		private Button buttonPlay;

		[SerializeField]
		private Button buttonRestart2;

		[SerializeField]
		private Button buttonExit;

		[Space]
		[Header("Text")]

		[SerializeField]
		private TMP_Text shovelCounter;

		[SerializeField]
		private TMP_Text targetCounter;

		[Space]
		[Header("Target container")]

		[SerializeField]
		private RectTransform targetContainer;

		[Space]
		[Header("Game Canvas")]
		[SerializeField]
		private Canvas canvasGame;

		[Space]
		[Header("End game canvas")]
		[SerializeField]
		private Canvas canvasEndGame;

		[SerializeField]
		private TMP_Text endGameText;

		[SerializeField]
		private Button buttonRestart;


		private EventController eventController;

		private bool targetRectReady;
		private Rect targetRect;

		private void Start()
		{
			this.eventController = EventController.GetInstance();

			this.buttonPlay.onClick.AddListener(()=> this.eventController.onStartGameClick?.Invoke());
			this.buttonRestart.onClick.AddListener(() => this.eventController.onGameReload?.Invoke());
			this.buttonRestart2.onClick.AddListener(() => this.eventController.onGameReload?.Invoke());
			
			this.buttonExit.onClick.AddListener(() => this.eventController.onQuitAppClick?.Invoke());

			this.eventController.onShovelCountUpdate += this.UpdateShovelCount;
			this.eventController.onTargetCountUpdate += this.UpdateTargeetCount;

			this.eventController.onGameWin += this.ShowGameWin;
			this.eventController.onGameEnd += this.ShowGameLose;

			this.eventController.getTargetRect += this.GetTargetRect;
		}

		private void OnDestroy()
		{
			this.buttonPlay.onClick.RemoveAllListeners();
			this.buttonRestart.onClick.RemoveAllListeners();
			this.buttonRestart2.onClick.RemoveAllListeners();

			this.eventController.onShovelCountUpdate -= this.UpdateShovelCount;
			this.eventController.onTargetCountUpdate -= this.UpdateTargeetCount;

			this.eventController.onGameWin -= this.ShowGameWin;
			this.eventController.onGameEnd -= this.ShowGameLose;

			this.eventController.getTargetRect -= this.GetTargetRect;
		}

		private Rect GetTargetRect()
		{
			if (!this.targetRectReady)
			{
				this.targetRect = new Rect(this.targetContainer.position, 
					new Vector2(this.targetContainer.rect.width, this.targetContainer.rect.height));

				this.targetRectReady = true;
			}

			return this.targetRect;
		}

		private void UpdateShovelCount(int count)
		{
			if (count <= 0)
				count = 0;

			this.shovelCounter.text = count.ToString();
		}

		private void UpdateTargeetCount(int count)
		{
			if (count <= 0)
				count = 0;

			this.targetCounter.text = count.ToString();
		}

		private void ShowGameWin()
		{
			this.ShowEndPanel("Game Win");
		}

		private void ShowGameLose()
		{
			this.ShowEndPanel("Game Lose");
		}

		private void ShowEndPanel(string text)
		{
			this.endGameText.text = text;
			this.canvasGame.gameObject.SetActive(false);
			this.canvasEndGame.gameObject.SetActive(true);
		}
	}
}