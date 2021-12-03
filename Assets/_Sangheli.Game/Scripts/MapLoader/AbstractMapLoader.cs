using Sangheli.Save;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Game
{
	public abstract class AbstractMapLoader : MonoBehaviour
	{
		public abstract void SpawnField();
		public abstract List<AbstractCell> SpawnField(int _sizeX = -1, int _sizeY = -1);
		public abstract SaveParameters GetSave();
		public abstract bool RestoreSave(SaveParameters save);
	}
}