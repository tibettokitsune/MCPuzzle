using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class InventoryBtn : MonoBehaviour
    {
        [SerializeField] private Button btn;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countValue;

        public bool IsVacant { get; private set; } = true;
        
        public void Fill(Sprite itemIcon, ReactiveProperty<int> value, Action onTouch)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(onTouch.Invoke);

            icon.sprite = itemIcon;
            icon.SetNativeSize();

            countValue.text = value.Value.ToString();
            value.Subscribe(_ =>
            {
                countValue.text = _.ToString();
            }).AddTo(this);

            IsVacant = false;
            
            icon.gameObject.SetActive(true);
            countValue.gameObject.SetActive(true);
        }

        public void Clear()
        {
            IsVacant = true;
            
            icon.gameObject.SetActive(false);
            countValue.gameObject.SetActive(false);
            
            btn.onClick.RemoveAllListeners();
        }
    }
}