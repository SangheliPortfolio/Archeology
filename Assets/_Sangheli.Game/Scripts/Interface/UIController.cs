using Sangheli.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sangheli.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Buttons")] [SerializeField] private Button buttonPlay;

        [SerializeField] private Button buttonRestart2;

        [SerializeField] private Button buttonExit;

        [Space] [Header("Text")] [SerializeField]
        private TMP_Text shovelCounter;

        [SerializeField] private TMP_Text targetCounter;

        [Space] [Header("Target container")] [SerializeField]
        private RectTransform targetContainer;

        [Space] [Header("Game Canvas")] [SerializeField]
        private Canvas canvasGame;

        [Space] [Header("End game canvas")] [SerializeField]
        private Canvas canvasEndGame;

        [SerializeField] private TMP_Text endGameText;

        [SerializeField] private Button buttonRestart;

        [SerializeField] private TMP_Text endGameShovelCount;

        [SerializeField] private TMP_Text endGameCoinCount;


        private EventController eventController;

        private bool targetRectReady;
        private Rect targetRect;

        private void Start()
        {
            eventController = EventController.GetInstance();

            buttonPlay.onClick.AddListener(() => eventController.onStartGameClick?.Invoke());
            buttonRestart.onClick.AddListener(() => eventController.onGameReload?.Invoke());
            buttonRestart2.onClick.AddListener(() => eventController.onGameReload?.Invoke());

            buttonExit.onClick.AddListener(() => eventController.onQuitAppClick?.Invoke());

            eventController.onShovelCountUpdate += UpdateShovelCount;
            eventController.onTargetCountUpdate += UpdateTargeetCount;

            eventController.onGameWin += ShowGameWin;
            eventController.onGameEnd += ShowGameLose;

            eventController.getTargetRect += GetTargetRect;
        }

        private void OnDestroy()
        {
            buttonPlay.onClick.RemoveAllListeners();
            buttonRestart.onClick.RemoveAllListeners();
            buttonRestart2.onClick.RemoveAllListeners();

            eventController.onShovelCountUpdate -= UpdateShovelCount;
            eventController.onTargetCountUpdate -= UpdateTargeetCount;

            eventController.onGameWin -= ShowGameWin;
            eventController.onGameEnd -= ShowGameLose;

            eventController.getTargetRect -= GetTargetRect;
        }

        private Rect GetTargetRect()
        {
            if (!targetRectReady)
            {
                targetRect = new Rect(targetContainer.position - new Vector3(targetContainer.rect.width / 2, 0),
                    new Vector2(targetContainer.rect.width, targetContainer.rect.height));

                targetRectReady = true;
            }

            return targetRect;
        }

        private void UpdateShovelCount(int count)
        {
            if (count <= 0)
                count = 0;

            shovelCounter.text = count.ToString();
        }

        private void UpdateTargeetCount(int count)
        {
            if (count <= 0)
                count = 0;

            targetCounter.text = count.ToString();
        }

        private void ShowGameWin(int shovel, int coin)
        {
            ShowEndPanel("Game Win", shovel, coin);
        }

        private void ShowGameLose(int shovel, int coin)
        {
            ShowEndPanel("Game Lose", shovel, coin);
        }

        private void ShowEndPanel(string text, int shovel, int coin)
        {
            endGameText.text = text;
            canvasGame.gameObject.SetActive(false);
            canvasEndGame.gameObject.SetActive(true);

            endGameShovelCount.text = shovel.ToString();
            endGameCoinCount.text = coin.ToString();
        }
    }
}