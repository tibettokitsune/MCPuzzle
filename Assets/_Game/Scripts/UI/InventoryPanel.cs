using System.Linq;
using _Game.Scripts.Configs;
using _Game.Scripts.Gameplay;
using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.UI
{
    public class InventoryPanel : MonoBehaviour
    {
        public ReactiveCommand<BlockType> OnBlockPicked { get; } = new ReactiveCommand<BlockType>();

        [Inject] private LevelEvents _levelEvents;
        [Inject] private IInventoryController _inventoryController;
        [Inject] private GameItemsConfig _itemsConfig;
        [SerializeField] private InventoryBtn[] btns;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private void Start()
        {
            _levelEvents.OnGameStart.Subscribe(_ =>
            {
                _disposable.Clear();
                foreach (var btn in btns)
                {
                    btn.Clear();
                }
                _inventoryController.InventoryBlocks.ObserveAdd().Subscribe(_ =>
                            {
                                var neededBtn = btns.First(x => x.IsVacant);
                                var neededItem = _itemsConfig.blockPresets.First(x => x.blockType == _.Key);
                                
                                neededBtn.Fill(neededItem.icon, _inventoryController.InventoryBlocks[_.Key], () =>
                                {
                                    OnBlockPicked.Execute(_.Key);
                                });
                
                                _inventoryController.InventoryBlocks.ObserveRemove()
                                    .Where(x => x.Key == _.Key).Subscribe(
                                    _ =>
                                    {
                                        neededBtn.Clear();
                                    }).AddTo(this);
                            }).AddTo(_disposable);
                
            }).AddTo(this);
            
        }
    }
}