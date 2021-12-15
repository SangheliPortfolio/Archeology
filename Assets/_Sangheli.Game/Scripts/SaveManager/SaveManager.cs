using System;
using System.Collections.Generic;
using Sangheli.Event;
using UnityEngine;

namespace Sangheli.Save
{
	public class SaveManager : MonoBehaviour
	{
		private EventController eventController;

		private void Start()
		{
			eventController = EventController.GetInstance();

			eventController.onAppStart += OnAppStart;
			eventController.onAppQuit += OnAppQuit;

			eventController.onGameReload += ResetSaves;
		}

		private void OnDestroy()
		{
			eventController.onAppStart -= OnAppStart;
			eventController.onAppQuit -= OnAppQuit;

			eventController.onGameReload -= ResetSaves;
		}

		private bool OnAppStart()
		{
			if (eventController.restoreSaveGame == null || eventController.restoreSaveField == null)
				return false;

			var saveGame = GetParameters("game");
			var saveField = GetParameters("field");

			if (saveGame == null || saveField == null)
				return false;

			var resultGame = eventController.restoreSaveGame.Invoke(saveGame);
			var resultField = eventController.restoreSaveField.Invoke(saveField);
			return resultGame && resultField;
		}

		private void OnAppQuit()
		{
			var saveGame = eventController.writeSaveGame?.Invoke();
			var saveField = eventController.writeSaveField?.Invoke();

			if (saveGame != null && saveField != null)
			{
				ResetSaves();

				SetParameters(saveGame);
				SetParameters(saveField);

				PlayerPrefs.Save();
			}
		}

		private void ResetSaves()
		{
			PlayerPrefs.DeleteAll();
		}

		private SaveParameters GetParameters(string name)
		{
			int count = PlayerPrefs.GetInt($"{name}_count");
			List<int> allData = new List<int>();
			
			for (var i = 0; i < count; i++)
			{
				allData.Add(PlayerPrefs.GetInt($"{name}_{i}"));
			}

			var data = new SaveParameters();
			data.name = name;
			data.intList = allData;

			return data;
		}

		private void SetParameters(SaveParameters save)
		{
			PlayerPrefs.SetInt($"{save.name}_count",save.intList.Count);

			for(var i = 0; i < save.intList.Count; i++)
			{
				PlayerPrefs.SetInt($"{save.name}_{i}", save.intList[i]);
			}
		}
	}
}
