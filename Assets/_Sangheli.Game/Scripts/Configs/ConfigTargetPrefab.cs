using Sangheli.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Config
{
	[CreateAssetMenu(fileName = "ConfigTargetPrefab", menuName = "SangheliGame/CreateTargetPrefabConfig", order = 1)]

	public class ConfigTargetPrefab : ScriptableObject
	{
		public Target prefab;
	}
}