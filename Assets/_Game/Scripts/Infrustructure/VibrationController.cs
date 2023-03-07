using MoreMountains.NiceVibrations;
using UnityEngine;

namespace _Game.Scripts.Infrustructure
{
    public interface IVibrationController
    {
        void BlockPlaceVibration();

        void DestroyBlockVibration();
    }
    
    public class VibrationController : IVibrationController
    {

        private float _lastDestroyBlockTime;
        private const float DestroyVibrationDeltaTime = 0.1f;
        
        public void BlockPlaceVibration()
        {
            MMVibrationManager.Haptic(HapticTypes.Success);
        }

        public void DestroyBlockVibration()
        {
            if (Time.time - _lastDestroyBlockTime > DestroyVibrationDeltaTime)
            {
                _lastDestroyBlockTime = Time.time;
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }
        }
    }
}