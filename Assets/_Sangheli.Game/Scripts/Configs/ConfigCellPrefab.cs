using Sangheli.Game;
using UnityEngine;

namespace Sangheli.Config
{
	[CreateAssetMenu(fileName = "ConfigCellPrefab", menuName = "SangheliGame/CreateCellPrefabConfig", order = 1)]

	public class ConfigCellPrefab : ScriptableObject
	{
		public AbstractCell cellPrefab;
	}
}