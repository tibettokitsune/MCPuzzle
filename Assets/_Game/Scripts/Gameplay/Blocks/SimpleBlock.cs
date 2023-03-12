using System.Linq;
using _Game.Scripts.Configs;
using UniRx;
using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class SimpleBlock : BlockController
    {
        public float Strenght { get; private set; }

        private float _defaultStrenght;

        private DestructionBlock _destructionBlock;

        private float _lastDamageTime;
        private const float RecoveryDelay = 0.3f;
        private const float RecoveryCounter = 2f;
        
        public SimpleBlock(Vector3Int position , BlockType blockType) : base(position, blockType)
        {
            Walkable = false;
            Destructible = true;
        }

        public override void Build(GameItemsConfig itemsConfig)
        {
            base.Build(itemsConfig);

            if (!Application.isPlaying) return;
            Strenght = itemsConfig.blockPresets.First(x => x.blockType == BlockType).BlockStrenght;
            _defaultStrenght = Strenght;
            _destructionBlock = Object.Instantiate(DestructionBlockPrefab,
                new Vector3(Position.x, Position.y, Position.z), Quaternion.identity);
            _destructionBlock.gameObject.SetActive(false);
            OnBlockDestroy.Take(1).Subscribe(_ => { Object.Destroy(_destructionBlock.gameObject); })
                .AddTo(_destructionBlock);
        }
        
        public void Build(GameItemsConfig itemsConfig, Transform root)
        {
            base.Build(itemsConfig, root);

            if (!Application.isPlaying) return;
            Strenght = itemsConfig.blockPresets.First(x => x.blockType == BlockType).BlockStrenght;
            _defaultStrenght = Strenght;
            _destructionBlock = Object.Instantiate(DestructionBlockPrefab,
                new Vector3(Position.x, Position.y, Position.z), Quaternion.identity);
            _destructionBlock.gameObject.SetActive(false);
            OnBlockDestroy.Take(1).Subscribe(_ => { Object.Destroy(_destructionBlock.gameObject); })
                .AddTo(_destructionBlock);
        }

        public override void TryDestroy(float impact)
        {
            base.TryDestroy(impact);

            Strenght -= impact;
            if (Strenght <= 0f)
            {
                OnBlockDestroy.Execute();
            }
            
            _destructionBlock.gameObject.SetActive(true);
            _destructionBlock.UpdateDestructionStage(Strenght / _defaultStrenght);
            _lastDamageTime = Time.time;
        }

        public void Clear()
        {
            Object.DestroyImmediate(View);
        }

        public override void Update()
        {
            if (Strenght < _defaultStrenght && Time.time - _lastDamageTime > RecoveryDelay)
            {
                Strenght += Time.deltaTime * _defaultStrenght * RecoveryCounter;
                Strenght = Mathf.Min(Strenght, _defaultStrenght);
                
                if(Strenght >= _defaultStrenght)_destructionBlock.UpdateDestructionStage(1);
                else _destructionBlock.UpdateDestructionStage(Strenght / _defaultStrenght);
            }
        }
    }
}