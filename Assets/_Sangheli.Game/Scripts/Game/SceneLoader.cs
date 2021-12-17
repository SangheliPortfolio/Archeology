using Sangheli.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Sangheli.Game
{
    public class SceneLoader : MonoBehaviour
    {
        private EventController _eventController;

        private void OnDestroy()
        {
            _eventController.onGameReload -= ReloadScene;
        }

        [Inject]
        public void Construct(EventController eventController)
        {
            _eventController = eventController;
            _eventController.onGameReload += ReloadScene;
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}