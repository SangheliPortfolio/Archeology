using Sangheli.Event;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Sangheli.Save
{
	public class SaveManager : MonoBehaviour
	{
		private EventController eventController;

		private void Start()
		{
			this.eventController = EventController.GetInstance();

			this.eventController.onAppStart += this.OnAppStart;
			this.eventController.onAppQuit += this.OnAppQuit;

			this.eventController.onGameReload += this.ResetSaves;
		}

		private void OnDestroy()
		{
			this.eventController.onAppStart -= this.OnAppStart;
			this.eventController.onAppQuit -= this.OnAppQuit;

			this.eventController.onGameReload -= this.ResetSaves;
		}

		private bool OnAppStart()
		{
			if (this.eventController.restoreSaveGame == null || this.eventController.restoreSaveField == null)
				return false;

			var saveGame = this.GetParameters("game");
			var saveField = this.GetParameters("field");

			if (saveGame == null || saveField == null)
				return false;

			bool resultGame = this.eventController.restoreSaveGame.Invoke(saveGame);
			bool resultField = this.eventController.restoreSaveField.Invoke(saveField);
			return resultGame && resultField;
		}

		private void OnAppQuit()
		{
			var saveGame = this.eventController.writeSaveGame?.Invoke();
			var saveField = this.eventController.writeSaveField?.Invoke();

			if (saveGame != null && saveField != null)
			{
				this.ResetSaves();

				this.SetParameters(saveGame);
				this.SetParameters(saveField);

				PlayerPrefs.Save();
			}
		}

		private void ResetSaves()
		{
			PlayerPrefs.DeleteAll();
		}

		private SaveParameters GetParameters(string name)
		{
			int count = PlayerPrefs.GetInt(name + "_count");
			List<int> allData = new List<int>();
			
			for (int i = 0; i < count; i++)
			{
				allData.Add(PlayerPrefs.GetInt(name + "_" + i));
			}

			var data = new SaveParameters();
			data.name = name;
			data.intList = allData;

			return data;
		}

		private void SetParameters(SaveParameters save)
		{
			PlayerPrefs.SetInt(save.name +"_count",save.intList.Count);

			for(int i = 0; i < save.intList.Count; i++)
			{
				PlayerPrefs.SetInt(save.name + "_"+i, save.intList[i]);
			}
		}
	}
}
