using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class MapVisualiser : MonoBehaviour
    {
        [SerializeField] private MapConfig _mapConfig;
        [SerializeField] private GameItemsConfig _itemsConfig;
        [SerializeField] private Transform root;

        [SerializeField] private Transform currentBlock;
        [SerializeField] private Transform pointerPrefab;
        
        public MapData CurrentMapData;

        [Button]
        public void LoadMapData(int level)
        {
            if (level > _mapConfig.mapsData.Length - 1) throw new Exception("Bad map level");
            CurrentMapData = _mapConfig.mapsData[level];
            
            Clear();
            Build();
        }

        [Button]
        public void SaveMapData(int level)
        {
            if (level > _mapConfig.mapsData.Length - 1)
            {
                var newData = new MapData[_mapConfig.mapsData.Length + 1];

                for (int i = 0; i < _mapConfig.mapsData.Length; i++)
                {
                    newData[i] = _mapConfig.mapsData[i];
                }

                newData[^1] = CurrentMapData;

                _mapConfig.mapsData = newData;

            }
            else
            {
                _mapConfig.mapsData[level] = CurrentMapData;
            }
            
            
        }


        [SerializeField] private List<SimpleBlock> _blocks = new List<SimpleBlock>();
        [OnValueChanged("OnPositionMove")]
        [SerializeField] private Vector3Int position;
        [SerializeField] private BlockType blockType;
        [Button]
        private void Build()
        {
            foreach (var block in CurrentMapData.blockData)
            {
                if (block.blockType is BlockType.Dirt or BlockType.Grass or BlockType.Rock or BlockType.Wood or BlockType.Sand)
                {
                    var _block = new SimpleBlock(block.position, block.blockType);
                    _block.Build(_itemsConfig, root);
                    _blocks.Add(_block);
                }
            }
        }

        [Button]
        private void Clear()
        {
            var lim = root.childCount;
            for (var i = 0; i < lim; i++)
            {
                DestroyImmediate(root.GetChild(0).gameObject);
            }
        }
        
        [Button]
        private void AddBlock()
        {
            if (CurrentMapData.blockData.Any(x => x.position.Equals(position)))
            {
                RemoveBlock();
            }
            CurrentMapData.blockData.Add(new MapBlockData()
            {
                blockType = blockType,
                position = position
            });
            
            Clear();
            Build();
            // newData[^1] = 
        }
        
        [Button]
        private void RemoveBlock()
        {
            CurrentMapData.blockData.Remove(CurrentMapData.blockData.Find(x => x.position.Equals(position)));
            
            Clear();
            Build();
        }

        private void OnPositionMove()
        {
            if(!pointerPrefab) return;
            
            if (!currentBlock)
            {
                currentBlock = GameObject.Instantiate(pointerPrefab);
            }

            currentBlock.transform.position = position;
        }
    }
}