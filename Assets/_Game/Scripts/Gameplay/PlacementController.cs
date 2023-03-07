using System;
using _Game.Scripts.Infrustructure;
using _Game.Scripts.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        [Inject] private IMapController _mapController;
        [Inject] private IInventoryController _controller;
        [Inject] private InventoryPanel _panel;
        [Inject] private IVibrationController _vibrationController;
        [SerializeField] private UnitController unitController;
        private void Start()
        {
            _panel.OnBlockPicked.Subscribe(_ =>
            {
                TryPlaceBlock(_);

            }).AddTo(this);
        }

        private void TryPlaceBlock(BlockType blockType)
        {
            for (int i = 0; i < 2; i++)
            {
                var targetPosition =
                    unitController.UnitData.currentPosition + unitController.currentDirection 
                                                            + Vector3Int.up * i;

                var res = _mapController.TryCreateBlock(blockType, targetPosition);

                if (res)
                {
                    _controller.RemoveItem(blockType);
                    _vibrationController.BlockPlaceVibration();
                    return;
                }
            }
        }
    }
}