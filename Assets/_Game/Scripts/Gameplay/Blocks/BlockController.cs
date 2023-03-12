using System.Linq;
using _Game.Scripts.Configs;
using UniRx;
using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public enum BlockType
    {
        Air = 0, 
        Water = 1, 
        Dirt = 2, 
        Grass = 3,
        Sand = 4,
        Wood = 5,
        Rock = 6,
    }
    
    public class BlockController
    {
        public ReactiveCommand OnBlockDestroy { get; } = new ReactiveCommand();
        public Vector3Int Position { get; private set; }

        public bool Walkable { get; set; }
        
        public bool Destructible { get; set; }
        public virtual BlockType BlockType { get; private set; }

        protected GameObject View;

        protected DestructionBlock DestructionBlockPrefab;

        public BlockController(Vector3Int position, BlockType blockType)
        {
            Position = position;
            BlockType = blockType;
        }

        public virtual void Build(GameItemsConfig itemsConfig)
        {
            if(BlockType is BlockType.Air or BlockType.Water) return;
            
            View = Object.Instantiate(itemsConfig.blockPresets.First(x => x.blockType == BlockType).viewPrefab, 
                new Vector3(Position.x, Position.y, Position.z),
                Quaternion.identity);

            DestructionBlockPrefab = itemsConfig.destructionBlockPrefab;
            OnBlockDestroy.Take(1).Subscribe(_ =>
            {
                Object.Destroy(View);
            }).AddTo(View);
        }
        
        public virtual void Build(GameItemsConfig itemsConfig, Transform root)
        {
            if(BlockType is BlockType.Air or BlockType.Water) return;
            
            View = Object.Instantiate(itemsConfig.blockPresets.First(x => x.blockType == BlockType).viewPrefab, 
                new Vector3(Position.x, Position.y, Position.z),
                Quaternion.identity, root);

            DestructionBlockPrefab = itemsConfig.destructionBlockPrefab;
            OnBlockDestroy.Take(1).Subscribe(_ =>
            {
                Object.Destroy(View);
            }).AddTo(View);
        }

        public virtual void TryDestroy(float impact)
        {
            
        }
        
        public virtual void Update(){}
    }
}