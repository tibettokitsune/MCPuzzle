using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Configs;
using _Game.Scripts.Gameplay.EvironmentObjects;
using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Gameplay
{
    [System.Serializable]
    public struct MapBlockData
    {
        public Vector3Int position;
        public BlockType blockType;
    }
    
    [System.Serializable]
    public struct MapItemsData
    {
        public Vector3Int position;
        public Vector3 rotation;
        public ItemType itemType;
    }
    
    public interface IMapController
    {
        int WaterLevel();

        BlockController GetBlockByPosition(Vector3Int position);

        bool TryCreateBlock(BlockType blockType, Vector3Int position);
        void DoBlockNotWalkable(Vector3Int position);
    }

    public class MapController : IInitializable, IMapController
    {
        [Inject] private IPlayerDataController _dataController;
        [Inject] private GameItemsConfig _itemsConfig;
        [Inject] private MapConfig _mapConfig;
        [Inject] private IInventoryController _inventoryController;

        [Inject] private IItemSpawner _itemSpawner;
        // private List<BlockController> _block = new List<BlockController>();

        private BlockController[,,] _block = new BlockController[,,]{};
        
        private const int WaterLevelValue = 0;

        private const int MapRank = 25;
        
        public void Initialize()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            _block = new BlockController[MapRank, MapRank, MapRank];
            
            for(var x =0; x < MapRank; x++)
            {
                for(var y =0; y < MapRank; y++)
                {
                    for(var z =0; z < MapRank; z++)
                    {
                        var block = new BlockController(new Vector3Int(x, y, z), BlockType.Air);
                        
                        if (y == WaterLevelValue)
                        {
                            block = new WaterBlockController(new Vector3Int(x, y, z), BlockType.Water);
                        }
                        else
                        {
                            block = new AirBlockController(new Vector3Int(x, y, z), BlockType.Air);
                        }
                        
                        _block[x, y, z] = block;
                    }
                }
            }



            foreach (var block in CurrentBlocksData())
            {
                if (block.blockType is BlockType.Dirt or BlockType.Grass or BlockType.Rock or BlockType.Wood or BlockType.Sand)
                {
                    _block[block.position.x, block.position.y, block.position.z] = new SimpleBlock(block.position, block.blockType);
                    _block[block.position.x, block.position.y, block.position.z].OnBlockDestroy.Take(1).Subscribe(_ =>
                    {

                        if (block.position.y <= WaterLevel())
                        {
                            _block[block.position.x, block.position.y, block.position.z] = 
                                new WaterBlockController(new Vector3Int(block.position.x, block.position.y, block.position.z), BlockType.Water);

                        }
                        else
                        {
                            _block[block.position.x, block.position.y, block.position.z] = 
                                new AirBlockController(new Vector3Int(block.position.x, block.position.y, block.position.z), BlockType.Air);
                        }

                        _inventoryController.AddItem(block.blockType);
                    });
                    
                    _block[block.position.x, block.position.y, block.position.z].Build(_itemsConfig);
                }
            }
            
            foreach (var item in CurrentItemsData())
            {
                var currentPreset = _itemsConfig.itemPresets.First(x => x.itemType == item.itemType);
                var instance = _itemSpawner.SpawnItem(currentPreset.viewPrefab);
                // var instance = Object.Instantiate(currentPreset.viewPrefab);
                
                instance.Setup(item, currentPreset);
            }
        }

        public int WaterLevel() => WaterLevelValue;
        
        public BlockController GetBlockByPosition(Vector3Int position)
        {
            try
            {
                return _block[position.x, position.y, position.z];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool TryCreateBlock(BlockType blockType, Vector3Int position)
        {
            var currentBlock = _block[position.x, position.y, position.z];
            
            if (currentBlock is SimpleBlock) return false;
            
            _block[position.x, position.y, position.z] = new SimpleBlock(position, blockType);
            _block[position.x, position.y, position.z].OnBlockDestroy.Take(1).Subscribe(_ =>
            {

                if (position.y <= WaterLevel())
                {
                    _block[position.x, position.y, position.z] = 
                        new WaterBlockController(new Vector3Int(position.x, position.y, position.z), BlockType.Water);

                }
                else
                {
                    _block[position.x, position.y, position.z] = 
                        new AirBlockController(new Vector3Int(position.x, position.y, position.z), BlockType.Air);
                }

                _inventoryController.AddItem(blockType);
            });
            
            _block[position.x, position.y, position.z].Build(_itemsConfig);

            return true;
        }

        public void DoBlockNotWalkable(Vector3Int position)
        {
            _block[position.x, position.y, position.z].Walkable = false;
        }

        private List<MapBlockData> CurrentBlocksData() => CurrentMapData().blockData;

        private List<MapItemsData> CurrentItemsData() => CurrentMapData().itemsData;
        
        private MapData CurrentMapData() =>
            _mapConfig.mapsData[_dataController.PlayerData.Value.level % _mapConfig.mapsData.Length];
    }
}