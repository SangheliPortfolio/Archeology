using Sangheli.Config;
using Sangheli.Event;
using Sangheli.Game;
using UnityEngine;

namespace Sangheli.Factory
{
	public class TargetFactory : MonoBehaviour
	{
		[SerializeField]
		private RectTransform parentContainer;

		[SerializeField]
		private ConfigTargetPrefab configTargetPrefab;

		private EventController eventController;

		private void Start()
		{
			this.eventController = EventController.GetInstance();

			this.eventController.createTarget += this.CreateTarget;
		}

		private void OnDestroy()
		{
			this.eventController.createTarget -= this.CreateTarget;
		}

		private Target CreateTarget()
		{
			return Instantiate(this.configTargetPrefab.prefab, Vector3.zero, Quaternion.identity,this.parentContainer);
		}
	}
}