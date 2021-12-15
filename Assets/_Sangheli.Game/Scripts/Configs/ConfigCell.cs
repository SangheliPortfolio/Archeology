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
			if (layerindex <= 0 || layerindex > spritesPerLayer.Length) return noneSprite;
			return spritesPerLayer[layerindex-1] != null ? spritesPerLayer[layerindex-1] : noneSprite;
		}
	}
}