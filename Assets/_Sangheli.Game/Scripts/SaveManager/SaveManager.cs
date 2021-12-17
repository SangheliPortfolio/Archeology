using System.Collections.Generic;
using Sangheli.Event;
using UnityEngine;
using Zenject;

namespace Sangheli.Save
{
    public class SaveManager : MonoBehaviour
    {
        private EventController _eventController;

        private void OnDestroy()
        {
            _eventController.onAppStart -= OnAppStart;
            _eventController.onAppQuit -= OnAppQuit;
            _eventController.onGameReload -= ResetSaves;
        }

        [Inject]
        public void Construct(EventController eventController)
        {
            _eventController = eventController;
            _eventController.onAppStart += OnAppStart;
            _eventController.onAppQuit += OnAppQuit;
            _eventController.onGameReload += ResetSaves;
        }

        private bool OnAppStart()
        {
            if (_eventController.restoreSaveGame == null || _eventController.restoreSaveField == null)
                return false;

            var saveGame = GetParameters("game");
            var saveField = GetParameters("field");

            if (saveGame == null || saveField == null)
                return false;

            var resultGame = _eventController.restoreSaveGame.Invoke(saveGame);
            var resultField = _eventController.restoreSaveField.Invoke(saveField);
            return resultGame && resultField;
        }

        private void OnAppQuit()
        {
            var saveGame = _eventController.writeSaveGame?.Invoke();
            var saveField = _eventController.writeSaveField?.Invoke();

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
            var count = PlayerPrefs.GetInt($"{name}_count");
            var allData = new List<int>();

            for (var i = 0; i < count; i++) allData.Add(PlayerPrefs.GetInt($"{name}_{i}"));

            var data = new SaveParameters();
            data.name = name;
            data.intList = allData;

            return data;
        }

        private void SetParameters(SaveParameters save)
        {
            PlayerPrefs.SetInt($"{save.name}_count", save.intList.Count);

            for (var i = 0; i < save.intList.Count; i++) PlayerPrefs.SetInt($"{save.name}_{i}", save.intList[i]);
        }
    }
}