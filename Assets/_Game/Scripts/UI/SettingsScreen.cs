using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class SettingsScreen : UIScreen
    {
        [SerializeField] private Button closeBtn;

        private void Start()
        {
            closeBtn.onClick.AddListener(CloseScreen);
        }
    }
}