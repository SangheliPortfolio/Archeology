using Sangheli.Game;
using Sangheli.Save;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Sangheli.Event
{
	public class EventController
	{
		private static EventController Instance;

		public System.Action onStartGameClick;
		public System.Action onQuitAppClick;

		public System.Action<int> onShovelCountUpdate;
		public System.Action<int> onTargetCountUpdate;
		
		public System.Action onCollectTarget;
		public System.Action onCellClicked;
		
		public System.Func<bool> isGameEnabled;
		public System.Action onGameWin;
		public System.Action onGameEnd;
		
		public System.Action onGameReload;
		
		public System.Func<Rect> getTargetRect;
		
		public System.Func<Target> createTarget;
		
		public System.Func<bool> onAppStart;
		public System.Action onAppQuit;

		public System.Func<SaveParameters> writeSaveGame;
		public System.Func<SaveParameters, bool> restoreSaveGame;

		public System.Func<SaveParameters> writeSaveField;
		public System.Func<SaveParameters, bool> restoreSaveField;

		public static EventController GetInstance()
		{
			if (Instance == null)
				Instance = new EventController();

			return Instance;
		}
	}
}