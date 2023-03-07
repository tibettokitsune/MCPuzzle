using System.Collections.Generic;
using _Game.Scripts.Gameplay;
using UnityEngine;

namespace _Game.Scripts.Configs
{
    [System.Serializable]
    public class MapData
    {
        public List<MapBlockData> blockData;

        public List<MapItemsData> itemsData;
    }
    
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/Map")]
    public class MapConfig : ScriptableObject
    {
        public MapData[] mapsData;
    }
}