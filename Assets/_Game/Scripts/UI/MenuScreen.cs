using _Game.Scripts.Infrustructure;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.UI
{
    public class MenuScreen : UIScreen
    {
        [Inject] private LevelEvents _levelEvents;
        [Inject] private IPlayerDataController _dataController;
        [SerializeField] private TextMeshProUGUI levelLbl;
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
            
            OnScreenOpen.Subscribe(_ => levelLbl.text = "Level " + (_dataController.PlayerData.Value.level + 1).ToString()).AddTo(this);
        }
    }
}