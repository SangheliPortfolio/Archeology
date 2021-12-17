using Sangheli.Config;
using Sangheli.Event;
using UnityEngine;
using Zenject;

namespace Sangheli.Target
{
    public class TargetFactory : MonoBehaviour
    {
        [SerializeField] private RectTransform parentContainer;
        [SerializeField] private ConfigTargetPrefab configTargetPrefab;

        private EventController _eventController;

        private void OnDestroy()
        {
            _eventController.createTarget -= CreateTarget;
        }

        [Inject]
        public void Construct(EventController eventController)
        {
            _eventController = eventController;
            _eventController.createTarget += CreateTarget;
        }

        private Target CreateTarget()
        {
            var target = Instantiate(configTargetPrefab.prefab, Vector3.zero, Quaternion.identity, parentContainer);
            target.Init(_eventController);
            return target;
        }
    }
}