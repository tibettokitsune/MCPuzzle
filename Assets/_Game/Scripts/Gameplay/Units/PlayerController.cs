using _Game.Scripts.Infrustructure;
using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    public class PlayerController : IInitializable
    {
        [Inject] private IUnitSpawner _unitSpawner;
        [Inject] private IMapController _mapController;
        [Inject] private IInputController _inputController;
        [Inject] private LevelEvents _levelEvents;
        [Inject] private CinemachineVirtualCamera _virtualCamera;
        private UnitController _characterController;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void Initialize()
        {
            _characterController = _unitSpawner.SpawnPlayer();
            _virtualCamera.Follow = _characterController.transform;
            _inputController.OnSwipe.Subscribe(_ =>
            {
                _characterController.Move(new Vector3Int(_.x, 0, _.y));

            }).AddTo(_disposable);

            _characterController.OnMovementComplete.Subscribe(cell =>
            {
                _levelEvents.OnPlayerMove.Execute(cell);
            }).AddTo(_disposable);

            _inputController.OnTap.Take(1).Subscribe(_ =>
            {
                _levelEvents.OnGameStart.Execute();
            }).AddTo(_disposable);

        }
    }
}