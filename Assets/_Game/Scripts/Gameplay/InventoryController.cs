using System.Collections.Generic;
using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{

    public interface IInventoryController
    {
        ReactiveDictionary<BlockType, ReactiveProperty<int>> InventoryBlocks { get; }
        void AddItem(BlockType block);
        void RemoveItem(BlockType blockType);
    }
    
    
    public class InventoryController : IInventoryController, IInitializable
    {
        [Inject] private LevelEvents _levelEvents;
        public ReactiveDictionary<BlockType, ReactiveProperty<int>> InventoryBlocks { get; private set; } = new ReactiveDictionary<BlockType, ReactiveProperty<int>>();
        private CompositeDisposable _disposable = new CompositeDisposable();
        public void AddItem(BlockType block)
        {
            if (InventoryBlocks.ContainsKey(block))
            {
                InventoryBlocks[block].Value++;
            }
            else
            {
                var newValue = new ReactiveProperty<int>();
                newValue.Value = 1;
                InventoryBlocks.Add(block, newValue);
            }
        }

        public void RemoveItem(BlockType blockType)
        {
            InventoryBlocks[blockType].Value--;
            if (InventoryBlocks[blockType].Value <= 0)
            {
                InventoryBlocks.Remove(blockType);
            }
        }

        public void Initialize()
        {
            _levelEvents.OnGameLoaded.Subscribe(_ =>
            {
                InventoryBlocks.Clear();
            }).AddTo(_disposable);
        }
    }
}