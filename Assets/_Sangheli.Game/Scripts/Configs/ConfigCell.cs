using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Config
{
	[CreateAssetMenu(fileName = "ConfigCell", menuName = "SangheliGame/CreateConfigCell", order = 2)]
	public class ConfigCell : ScriptableObject
	{
		[Space]
		public int cellStepCount;

		public float cellSize;

		[Space]
		[Header("CellSize")]
		[SerializeField]
		private Sprite noneSprite;

		[SerializeField]
		private Sprite[] spritesPerLayer;

		public Sprite GetSprite(int layerindex = -1)
		{
			if (layerindex <= 0)
				return this.noneSprite;

			if (layerindex <= this.spritesPerLayer.Length)
			{
				if (this.spritesPerLayer[layerindex-1] != null)
				{
					return this.spritesPerLayer[layerindex-1];
				}
				else
				{
					return this.noneSprite;
				}
			}

			return this.noneSprite;
		}
	}
}