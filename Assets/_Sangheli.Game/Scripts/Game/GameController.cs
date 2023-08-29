using System.Collections.Generic;
using System.Threading.Tasks;
using Sangheli.Config;
using Sangheli.Event;
using Sangheli.Save;
using UnityEngine;
using Zenject;

namespace Sangheli.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private AbstractMapLoader mapLoader;

        [SerializeField] private ConfigGame configGame;

        private EventController _eventController;
        private int currentShovelCount;

        private int currentTargetCount;

        private bool isGameEnabled;

        private void Start()
        {
            // RestoreSaves();
            _eventController.onStartGameClick.Invoke();
        }

        private void OnDestroy()
        {
            _eventController.onStartGameClick -= mapLoader.SpawnField;
            _eventController.onStartGameClick -= InitUI;
            _eventController.onStartGameClick -= StartGame;

            _eventController.onQuitAppClick -= QuitApp;

            _eventController.onCollectTarget -= CollectTarget;
            _eventController.onCellClicked -= SpendShovel;

            _eventController.isGameEnabled -= () => isGameEnabled;

            _eventController.writeSaveGame -= CollectSaveGame;
            _eventController.restoreSaveGame -= RestoreSaveData;

            _eventController.writeSaveField -= CollectSaveField;
            _eventController.restoreSaveField -= RestoreSaveField;
        }

        private void OnApplicationQuit()
        {
            _eventController.onAppQuit?.Invoke();
        }

        [Inject]
        public void Construct(EventController eventController)
        {
            _eventController = eventController;

            _eventController.onStartGameClick += mapLoader.SpawnField;
            _eventController.onStartGameClick += InitUI;
            _eventController.onStartGameClick += StartGame;

            _eventController.onQuitAppClick += QuitApp;

            _eventController.onCollectTarget += CollectTarget;
            _eventController.onCellClicked += SpendShovel;

            _eventController.isGameEnabled += () => isGameEnabled;

            _eventController.writeSaveGame += CollectSaveGame;
            _eventController.restoreSaveGame += RestoreSaveData;

            _eventController.writeSaveField += CollectSaveField;
            _eventController.restoreSaveField += RestoreSaveField;
        }

        private void StartGame()
        {
            isGameEnabled = true;
        }

        private void RestoreSaves()
        {
            var restored = false;
            if (_eventController.onAppStart != null)
                restored = _eventController.onAppStart.Invoke();

            if (!restored) return;

            UpdateShovelCount(currentShovelCount);
            UpdateTargetCount(currentTargetCount);
            isGameEnabled = true;
        }

        private void CollectTarget()
        {
            currentTargetCount++;
            UpdateTargetCount(currentTargetCount);

            if (currentTargetCount >= configGame.maxTargetCount)
                EndGameWin();
        }

        private void SpendShovel()
        {
            currentShovelCount--;
            if (currentShovelCount < 0) return;
            UpdateShovelCount(currentShovelCount);
            if (currentShovelCount == 0) EndGameLose();
        }

        private void EndGameWin()
        {
            isGameEnabled = false;
            _eventController.onGameWin?.Invoke(currentShovelCount, currentTargetCount);
        }

        private void EndGameLose()
        {
            isGameEnabled = false;
            _eventController.onGameEnd?.Invoke(currentShovelCount, currentTargetCount);
        }

        private async void InitUI()
        {
            if (isGameEnabled)
                return;

            await Task.Yield();
            currentShovelCount = configGame.startShovelCounter;
            currentTargetCount = 0;
            UpdateShovelCount(currentShovelCount);
            UpdateTargetCount(currentTargetCount);
        }

        private void UpdateShovelCount(int count)
        {
            _eventController.onShovelCountUpdate?.Invoke(count);
        }

        private void UpdateTargetCount(int count)
        {
            _eventController.onTargetCountUpdate?.Invoke(count);
        }

        private void QuitApp()
        {
            Application.Quit();
        }

        private SaveParameters CollectSaveGame()
        {
            if (!isGameEnabled)
                return null;

            var save = new SaveParameters();
            save.name = "game";
            save.intList = new List<int> {currentTargetCount, currentShovelCount};
            return save;
        }

        private bool RestoreSaveData(SaveParameters save)
        {
            if (save.intList.Count != 2)
                return false;

            currentTargetCount = save.intList[0];
            currentShovelCount = save.intList[1];
            return true;
        }

        private SaveParameters CollectSaveField() => isGameEnabled ? mapLoader.GetSave() : null;

        private bool RestoreSaveField(SaveParameters save) => mapLoader.RestoreSave(save);
    }
}