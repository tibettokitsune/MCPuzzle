using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI
{
    public class TutorialScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI tutorialLbl;
        
        public void ShowText(string text)
        {
            tutorialLbl.text = text;
            
            OpenScreen();

            DOVirtual.DelayedCall(3f, CloseScreen);
        }
    }
}