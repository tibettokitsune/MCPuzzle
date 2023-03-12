using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    [System.Serializable]
    public class UnitData
    {
        public Vector3Int currentPosition;
        public float health;
        public float speed;
        public int height;
    }
    public class UnitController : MonoBehaviour
    {
        public ReactiveCommand<Vector3Int> OnMovementComplete { get; } = new ReactiveCommand<Vector3Int>();
        [Inject] private IMapController _mapController;
        public UnitData UnitData { get; } = new UnitData();

        [SerializeField] private UnitAnimationController unitAnimationController;
        
        private bool _isMoving;

        [SerializeField] private AnimationCurve jumpCurve;

        public Vector3Int currentDirection = new Vector3Int(0, 0, -1);

        private void Start()
        {
            OnMovementComplete.Subscribe(
                _ =>
                {
                    unitAnimationController.Swim(_mapController.GetBlockByPosition(_).BlockType == BlockType.Water);
                }).AddTo(this);
        }

        [Button]
        public void Move(Vector3Int direction)
        {
            if(_isMoving) return;
            
            if (direction.y == 0)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                for (var h = UnitData.height; h > 0; h--)
                {
                    var nextUpperBlock =
                        _mapController.GetBlockByPosition(UnitData.currentPosition + direction + Vector3Int.up * h);
                        
                    if(!nextUpperBlock.Walkable) return;
                }
                
                
                var nextBlock =
                    _mapController.GetBlockByPosition(UnitData.currentPosition + direction);
                currentDirection = direction;
                if(!nextBlock.Walkable)
                    direction += Vector3Int.up;
            }
            else
            {
                if((UnitData.currentPosition + direction).y < _mapController.WaterLevel())
                    return;
                
                var nextBlock =
                    _mapController.GetBlockByPosition(UnitData.currentPosition + direction);
                
                if(!nextBlock.Walkable) return;
            }
            
            _isMoving = true;
            unitAnimationController.Movement(1f);
            UnitData.currentPosition += direction;

            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + direction;

            DOVirtual.Float(0, 1,   direction.y<0? UnitData.speed / 2f : UnitData.speed, v =>
                {
                    transform.position = Vector3.Lerp(startPos, endPos, v) + (direction.y > 0 ? jumpCurve.Evaluate(v) : 0)  * Vector3.up;
                }).SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                transform.position = endPos;
                _isMoving = false;
                unitAnimationController.Movement(0f);
                OnMovementComplete.Execute(UnitData.currentPosition);
                Move(Vector3Int.down);
            });
            
            if(direction.y == 0)
                transform.rotation = Quaternion.LookRotation(direction);
        }
        
        public class Factory : PlaceholderFactory<UnitController, UnitController>
        {
            readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public UnitController Create(UnityEngine.Object prefab) 
                => _container.InstantiatePrefabForComponent<UnitController>(prefab);
        }

        public void SetupUnit(Vector3Int position, Vector3 rotation)
        {
            transform.eulerAngles = rotation;
            transform.position = position;
            UnitData.currentPosition = position;
            UnitData.health = 10;
            UnitData.speed = 1f / 3f;
            UnitData.height = 2;
            
            Move(Vector3Int.down);
        }
    }
}