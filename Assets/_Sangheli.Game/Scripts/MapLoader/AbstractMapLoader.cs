using System.Collections.Generic;
using Sangheli.Save;
using UnityEngine;

namespace Sangheli.Game
{
	public abstract class AbstractMapLoader : MonoBehaviour
	{
		public abstract void SpawnField();
		public abstract List<AbstractCell> SpawnField(int sizeX = -1, int sizeY = -1);
		public abstract SaveParameters GetSave();
		public abstract bool RestoreSave(SaveParameters save);
	}
}