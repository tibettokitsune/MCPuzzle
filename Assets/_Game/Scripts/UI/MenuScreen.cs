using System;
using _Game.Scripts.Infrustructure;
using UniRx;
using Zenject;

namespace _Game.Scripts.UI
{
    public class MenuScreen : UIScreen
    {
        [Inject] private LevelEvents _levelEvents;
        private void Start()
        {
            _levelEvents.OnGameLoaded.Subscribe(_ =>
            {
                OpenScreen();
            }).AddTo(this);

            _levelEvents.OnGameStart.Subscribe(_ =>
            {
                CloseScreen();
            }).AddTo(this);
        }
    }
}