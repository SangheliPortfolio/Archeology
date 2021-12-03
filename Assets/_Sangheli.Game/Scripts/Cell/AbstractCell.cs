using Sangheli.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Game
{
	public abstract class AbstractCell : MonoBehaviour 
	{
		public abstract void Init(ConfigCell configCell, int cellSize = -1);
		public abstract void UpdateVisual(int state = -1);
	}
}