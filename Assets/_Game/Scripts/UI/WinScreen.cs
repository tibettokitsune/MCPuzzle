using _Game.Scripts.Infrustructure;
using Game.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts.UI
{
    public class WinScreen : UIScreen
    {
        [Inject] private IPlayerDataController _dataController;
        [Inject] private ISceneController _sceneController;
        [Inject] private LevelEvents _levelEvents;
        [SerializeField] private Button continueBtn;
        [SerializeField] private MoneyEffect moneyEffect;
        
        private void Start()
        {
            _levelEvents.OnGameEnd.Subscribe(_ => OpenScreen()).AddTo(this);

            OnScreenOpen.Subscribe(_ =>
            {
                continueBtn.interactable = false;
                moneyEffect.Effect(() =>
                {
                    _dataController.IncrementMoney(Random.Range(40, 80));
                    continueBtn.interactable = true;
                });
                
            }).AddTo(this);
            continueBtn.onClick.AddListener(() =>
            {
                CloseScreen();
                _sceneController.NextLevel();
            });
        }
    }
}