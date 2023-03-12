using _Game.Scripts.Infrustructure;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.UI
{
    public class GameplayScreen : UIScreen
    {
        [Inject] private LevelEvents _levelEvents;
        private void Start()
        {
            _levelEvents.OnGameStart.Subscribe(_ => OpenScreen()).AddTo(this);
            _levelEvents.OnGameEnd.Subscribe(_ => CloseScreen()).AddTo(this);
        }
    }
    
}