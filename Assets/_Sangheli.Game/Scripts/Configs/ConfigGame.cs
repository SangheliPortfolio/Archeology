using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sangheli.Config
{
	[CreateAssetMenu(fileName = "ConfigGame", menuName = "SangheliGame/CreateGameConfig", order = 1)]
	public class ConfigGame : ScriptableObject
	{
		public int startShovelCounter;
		public int maxTargetCount;
	}
}
