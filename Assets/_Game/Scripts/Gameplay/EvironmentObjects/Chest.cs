using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay.EvironmentObjects
{
    public class Chest : ItemController
    {
        [Inject] private IMapController _mapController;
        [Inject] private LevelEvents _levelEvents;
        [SerializeField] private Animator _animator;
        private static readonly int Open = Animator.StringToHash("Open");

        public override void Start()
        {
            base.Start();
            transform.position = Position;
            transform.rotation = Quaternion.Euler(Rotation);

            _levelEvents.OnPlayerMove.Where(IsBlockInteraction).Subscribe(_ =>
            {
                _levelEvents.OnGameEnd.Execute(true);
                _animator.SetTrigger(Open);
            }).AddTo(this);
        }
    }
}