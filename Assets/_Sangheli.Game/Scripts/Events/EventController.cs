using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Event
{
	public class EventController
	{
		private static EventController Instance;

		public System.Action onStartGameClick;
		public System.Action<int> onShovelCountUpdate;
		public System.Action<int> onTargetCountUpdate;
		public System.Action onCollectTarget;
		public System.Action onCellClicked;
		public System.Func<bool> isGameEnabled;

		public static EventController GetInstance()
		{
			if (Instance == null)
				Instance = new EventController();

			return Instance;
		}
	}
}