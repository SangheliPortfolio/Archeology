using Sangheli.Config;
using Sangheli.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sangheli.Game
{
    public class Cell : AbstractCell
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private ConfigCell _configCell;
        private int currentState;
        private int maxState;

        private EventController _eventController;

        private bool targetCollected;
        private int targetLayer = -1;
        private Target.Target currentTarget;

        private Camera _camera;
        private bool _cellFinished;

        public override void Init(EventController eventController,Camera camera, ConfigCell configCell, int cellSize = -1)
        {
            _eventController = eventController;
            _camera = camera;
            _configCell = configCell;

            if (cellSize < 0 || cellSize > configCell.cellStepCount)
                cellSize = configCell.cellStepCount;

            maxState = cellSize;
            currentState = cellSize;
            UpdateVisual(currentState);
        }

        public override void UpdateVisual(int state = -1)
        {
            spriteRenderer.sprite = _configCell.GetSprite(state);
        }

        private void OnMouseDown()
        {
            if (_cellFinished)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (!IsGameEnabled())
                return;

            if (IsTargetActive())
                return;

            currentState--;

            if (currentState < 1)
            {
                currentState = 1;
                _cellFinished = true;
            }
            else
            {
                _eventController.onCellClicked?.Invoke();
            }

            UpdateVisual(currentState);
            CreateTarget();
        }

        private bool IsGameEnabled()
        {
            var func = _eventController.isGameEnabled;
            return func == null || func.Invoke();
        }

        private void CreateTarget()
        {
            if (targetCollected || currentState != targetLayer || currentTarget != null) return;

            var funcCreateTarget = _eventController.createTarget;
            if (funcCreateTarget == null) return;

            currentTarget = funcCreateTarget.Invoke();

            var viewportPosition = _camera.WorldToViewportPoint(transform.position);
            var screenPos = _camera.ViewportToScreenPoint(viewportPosition);
            currentTarget.SetRectPosition(screenPos);

            currentTarget.onCollect += CollectTarget;
        }

        private bool IsTargetActive() => !targetCollected && currentState == targetLayer && currentTarget != null;

        public override void InitTarget()
        {
            targetLayer = Random.Range(1, maxState);
        }

        private void CollectTarget()
        {
            targetCollected = true;
            targetLayer = -1;
            currentTarget.onCollect -= CollectTarget;
        }

        public override int GetCurrentState() => currentState;

        public override int GetTargetLayer() => targetLayer;

        public override int GetTargetCollected() => targetCollected ? 1 : 0;

        public override void SetCurrentState(int index)
        {
            currentState = index;

            if (currentState < 1)
                currentState = 1;
        }

        public override void SetTargetLayer(int index)
        {
            targetLayer = index;
        }

        public override void SetTargetCollected(int index)
        {
            targetCollected = index == 1;
        }

        public override int GetCellFinished() => _cellFinished ? 1 : 0;

        public override void SetCellFinished(int index)
        {
            _cellFinished = index == 1;
        }

        public override void InitCellSaveData()
        {
            UpdateVisual(currentState);
            CreateTarget();
        }
    }
}