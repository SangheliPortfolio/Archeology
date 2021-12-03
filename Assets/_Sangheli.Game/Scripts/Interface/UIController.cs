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
		private Button buttonRestart;

		[SerializeField]
		private Button buttonExit;

		[Space]
		[Header("Text")]

		[SerializeField]
		private TMP_Text shovelCounter;

		[SerializeField]
		private TMP_Text targetCounter;


		private EventController eventController;

		private void Start()
		{
			this.eventController = EventController.GetInstance();

			this.buttonPlay.onClick.AddListener(()=>eventController.onStartGameClick?.Invoke());

			this.eventController.onShovelCountUpdate += this.UpdateShovelCount;
			this.eventController.onTargetCountUpdate += this.UpdateTargeetCount;
		}

		private void OnDestroy()
		{
			
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
	}
}