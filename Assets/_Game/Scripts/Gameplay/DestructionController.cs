using _Game.Scripts.Infrustructure;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    public class DestructionController : MonoBehaviour
    {
        [Inject] private IMapController _mapController;
        [Inject] private IInputController _inputController;
        [Inject] private IVibrationController _vibrationController;
        [SerializeField] private UnitController _characterController;
        [SerializeField] private UnitAnimationController unitAnimationController;


        private void TryDestroy()
        {
            for (int i = 0; i < 2; i++)
            {
                var targetPosition =
                    _characterController.UnitData.currentPosition + _characterController.currentDirection 
                                                                  + Vector3Int.up * i;

                var targetBlock = _mapController.GetBlockByPosition(targetPosition);

                if (targetBlock.BlockType != BlockType.Air && targetBlock.BlockType != BlockType.Water)
                {
                    targetBlock.TryDestroy(Time.deltaTime);
                    _vibrationController.DestroyBlockVibration();    
                    return;
                }
            }
            unitAnimationController.Work(false);

        }

        private void Update()
        {
            unitAnimationController.Work(Input.GetMouseButton(0));

            if (Input.GetMouseButton(0))
            {
                TryDestroy();
            }
        }
    }
}