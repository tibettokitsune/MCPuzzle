using System;
using _Game.Scripts.Infrustructure;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.UI
{
    public class WinScreen : UIScreen
    {
        [Inject] private ISceneController _sceneController;
        [Inject] private LevelEvents _levelEvents;
        [SerializeField] private Button continueBtn;

        private void Start()
        {
            _levelEvents.OnGameEnd.Subscribe(_ => OpenScreen()).AddTo(this);
            
            continueBtn.onClick.AddListener(() =>
            {
                CloseScreen();
                _sceneController.NextLevel();
            });
        }
    }
}