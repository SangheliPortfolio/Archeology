using Sangheli.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sangheli.Game
{
    public class SceneLoader : MonoBehaviour
    {
        private EventController eventController;

        private void Start()
        {
            eventController = EventController.GetInstance();

            eventController.onGameReload += ReloadScene;
        }

        private void OnDestroy()
        {
            eventController.onGameReload -= ReloadScene;
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}