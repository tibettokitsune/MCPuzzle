using System;
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

        public MapData Clone()
        {
            var res = new MapData();
            
            res.blockData = new List<MapBlockData>(blockData.Count);

            blockData.ForEach((item)=>
            {
                res.blockData.Add(new MapBlockData(item));
            });
            
            res.itemsData = new List<MapItemsData>(itemsData.Count);

            itemsData.ForEach((item)=>
            {
                res.itemsData.Add(new MapItemsData(item));
            });

            return res;
        }
    }
    
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/Map")]
    public class MapConfig : ScriptableObject
    {
        public MapData[] mapsData;
    }
}