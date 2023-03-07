using System;
using _Game.Scripts.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Infrustructure
{
    
    public class TutorialController : IInitializable, IDisposable
    {
        [Inject] private LevelEvents _levelEvents;
        [Inject] private IPlayerDataController _dataController;
        [Inject] private TutorialScreen _tutorialScreen;
        private CompositeDisposable _disposable = new CompositeDisposable();
        public void Initialize()
        {
            _levelEvents.OnGameStart.Where(x => _dataController.PlayerData.Value.level == 1).Subscribe(_ =>
                {

                    _levelEvents.OnPlayerMove.Where(x => x.Equals(new Vector3Int(13, 1, 14))).Subscribe(_ =>
                        {
                            _tutorialScreen.ShowText("touch screen to destroy block");
                            
                            _disposable.Clear();
                        })
                        .AddTo(_disposable);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}