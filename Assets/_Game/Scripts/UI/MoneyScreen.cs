using System;
using _Game.Scripts.Infrustructure;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.UI
{
    public class MoneyScreen : UIScreen
    {
        [Inject] private IPlayerDataController _dataController;
        [SerializeField] private TextMeshProUGUI moneyValueLbl;

        private int _cashMoneyValue;
        private Tween _changeValueTween;

        private const float ChangeValueTime = 0.6f;
        private void Start()
        {
            _dataController.PlayerData.Subscribe(_ => { UpdateMoneyValue(); }).AddTo(this);
            UpdateMoneyValue(true);
        }

        private void UpdateMoneyValue(bool isImmediately = false)
        {
            if (isImmediately)
            {
                moneyValueLbl.text = _dataController.PlayerData.Value.money.ToString();
                _cashMoneyValue = _dataController.PlayerData.Value.money;
            }
            else
            {
                _changeValueTween.Kill();
                _changeValueTween = DOVirtual.Int(_cashMoneyValue, _dataController.PlayerData.Value.money,
                    ChangeValueTime,
                    v =>
                    {
                        moneyValueLbl.text = v.ToString();
                    }).OnComplete(() =>
                {
                    UpdateMoneyValue(true);
                });
            }
        }
    }
}