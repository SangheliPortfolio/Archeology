using Sangheli.Config;
using Sangheli.Event;
using UnityEngine;

namespace Sangheli.Game
{
    public abstract class AbstractCell : MonoBehaviour
    {
        public abstract void Init(EventController eventController,Camera camera, ConfigCell configCell, int cellSize = -1);
        public abstract void UpdateVisual(int state = -1);
        public abstract void InitTarget();
        public abstract int GetCellFinished();
        public abstract int GetCurrentState();
        public abstract int GetTargetLayer();
        public abstract int GetTargetCollected();

        public abstract void SetCellFinished(int index);
        public abstract void SetCurrentState(int index);
        public abstract void SetTargetLayer(int index);
        public abstract void SetTargetCollected(int index);
        public abstract void InitCellSaveData();
    }
}