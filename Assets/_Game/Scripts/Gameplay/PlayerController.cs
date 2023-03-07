using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Inject] private IMapController _mapController;
        [Inject] private IInputController _inputController;
        [Inject] private LevelEvents _levelEvents;
        [SerializeField] private UnitController _characterController;

        private void Start()
        {
            _inputController.OnSwipe.Subscribe(_ =>
            {
                _characterController.Move(new Vector3Int(_.x, 0, _.y));

            }).AddTo(this);

            _characterController.OnMovementComplete.Subscribe(cell =>
            {
                _levelEvents.OnPlayerMove.Execute(cell);
            }).AddTo(this);

            _inputController.OnTap.Take(1).Subscribe(_ => { _levelEvents.OnGameStart.Execute(); }).AddTo(this);

        }
    }
}