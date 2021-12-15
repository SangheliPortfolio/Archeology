using System.Collections.Generic;
using System.Threading.Tasks;
using Sangheli.Config;
using Sangheli.Event;
using Sangheli.Save;
using UnityEngine;

namespace Sangheli.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private AbstractMapLoader mapLoader;

        [SerializeField] private ConfigGame configGame;

        private EventController eventController;

        private int currentTargetCount;
        private int currentShovelCount;

        private bool isGameEnabled;

        private void Awake()
        {
            eventController = EventController.GetInstance();
        }

        private void Start()
        {
            eventController.onStartGameClick += mapLoader.SpawnField;
            eventController.onStartGameClick += InitUI;
            eventController.onStartGameClick += StartGame;

            eventController.onQuitAppClick += QuitApp;

            eventController.onCollectTarget += CollectTarget;
            eventController.onCellClicked += SpendShovel;

            eventController.isGameEnabled += () => isGameEnabled;

            eventController.writeSaveGame += CollectSaveGame;
            eventController.restoreSaveGame += RestoreSaveData;

            eventController.writeSaveField += CollectSaveField;
            eventController.restoreSaveField += RestoreSaveField;

            RestoreSaves();
        }

        private void OnDestroy()
        {
            eventController.onStartGameClick -= mapLoader.SpawnField;
            eventController.onStartGameClick -= InitUI;
            eventController.onStartGameClick -= StartGame;

            eventController.onQuitAppClick -= QuitApp;

            eventController.onCollectTarget -= CollectTarget;
            eventController.onCellClicked -= SpendShovel;

            eventController.isGameEnabled -= () => isGameEnabled;

            eventController.writeSaveGame -= CollectSaveGame;
            eventController.restoreSaveGame -= RestoreSaveData;

            eventController.writeSaveField -= CollectSaveField;
            eventController.restoreSaveField -= RestoreSaveField;
        }

        private void StartGame()
        {
            isGameEnabled = true;
        }

        private async void RestoreSaves()
        {
            await Task.Yield();
            bool restored = false;
            if (eventController.onAppStart != null)
                restored = eventController.onAppStart.Invoke();

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

            if (currentShovelCount < 0)
            {
                return;
            }

            UpdateShovelCount(currentShovelCount);

            if (currentShovelCount == 0)
            {
                EndGameLose();
            }
        }

        private void EndGameWin()
        {
            isGameEnabled = false;
            eventController.onGameWin?.Invoke(currentShovelCount, currentTargetCount);
        }

        private void EndGameLose()
        {
            isGameEnabled = false;
            eventController.onGameEnd?.Invoke(currentShovelCount, currentTargetCount);
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
            eventController.onShovelCountUpdate?.Invoke(count);
        }

        private void UpdateTargetCount(int count)
        {
            eventController.onTargetCountUpdate?.Invoke(count);
        }

        private void OnApplicationQuit()
        {
            eventController.onAppQuit?.Invoke();
        }

        private void QuitApp()
        {
            Application.Quit();
        }

        private SaveParameters CollectSaveGame()
        {
            if (!isGameEnabled)
                return null;

            SaveParameters save = new SaveParameters();
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