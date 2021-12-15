using System;
using Sangheli.Game;
using Sangheli.Save;
using UnityEngine;

namespace Sangheli.Event
{
    public class EventController
    {
        private static EventController _instance;

        public Action onStartGameClick;
        public Action onQuitAppClick;

        public Action<int> onShovelCountUpdate;
        public Action<int> onTargetCountUpdate;

        public Action onCollectTarget;
        public Action onCellClicked;

        public Func<bool> isGameEnabled;
        public Action<int, int> onGameWin;
        public Action<int, int> onGameEnd;

        public Action onGameReload;

        public Func<Rect> getTargetRect;

        public Func<Target> createTarget;

        public Func<bool> onAppStart;
        public Action onAppQuit;

        public Func<SaveParameters> writeSaveGame;
        public Func<SaveParameters, bool> restoreSaveGame;

        public Func<SaveParameters> writeSaveField;
        public Func<SaveParameters, bool> restoreSaveField;

        public static EventController GetInstance() => _instance ??= new EventController();
    }
}