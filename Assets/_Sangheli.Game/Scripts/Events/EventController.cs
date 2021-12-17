using System;
using Sangheli.Save;
using UnityEngine;

namespace Sangheli.Event
{
    public class EventController
    {
        public Func<Target.Target> createTarget;

        public Func<Rect> getTargetRect;

        public Func<bool> isGameEnabled;
        public Action onAppQuit;

        public Func<bool> onAppStart;
        public Action onCellClicked;

        public Action onCollectTarget;
        public Action<int, int> onGameEnd;

        public Action onGameReload;
        public Action<int, int> onGameWin;
        public Action onQuitAppClick;

        public Action<int> onShovelCountUpdate;
        public Action onStartGameClick;
        public Action<int> onTargetCountUpdate;
        public Func<SaveParameters, bool> restoreSaveField;
        public Func<SaveParameters, bool> restoreSaveGame;

        public Func<SaveParameters> writeSaveField;

        public Func<SaveParameters> writeSaveGame;
    }
}