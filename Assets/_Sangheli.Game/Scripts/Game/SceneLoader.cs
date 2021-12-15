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
			this.eventController = EventController.GetInstance();

			this.eventController.onGameReload += this.ReloadScene;
		}

		private void OnDestroy()
		{
			this.eventController.onGameReload -= this.ReloadScene;
		}

		public void ReloadScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}