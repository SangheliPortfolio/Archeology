using UnityEngine;

namespace Sangheli.Config
{
    [CreateAssetMenu(fileName = "ConfigField", menuName = "SangheliGame/CreateFieldConfig", order = 1)]
    public class ConfigField : ScriptableObject
    {
        public int sizeX;
        public int sizeY;
    }
}