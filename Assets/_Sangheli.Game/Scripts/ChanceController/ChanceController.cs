using UnityEngine;

namespace Sangheli.Game
{
	[CreateAssetMenu(fileName = "ConfigChance", menuName = "SangheliGame/CreateChanceConfig", order = 1)]
	public class ChanceController : ScriptableObject
	{
		[SerializeField]
		private float minChanceForField;

		[SerializeField]
		private float maxChanceForField;

		public float GetChanceForField()
		{
			float value = Random.value;
			return Mathf.Clamp(value,this.minChanceForField, this.maxChanceForField);
		}
	}
}
