using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class UnitAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Swim1 = Animator.StringToHash("Swim");
        private static readonly int Work1 = Animator.StringToHash("Work");

        public void Movement(float speed)
        {
            _animator.SetFloat(Speed, speed);
        }

        public void Swim(bool state)
        {
            _animator.SetBool(Swim1, state);
        }

        public void Work(bool getMouseButton)
        {
            _animator.SetLayerWeight(1, getMouseButton? 1 : 0);
            _animator.SetBool(Work1, getMouseButton);
        }
    }
}