using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace _Game.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : MonoBehaviour
    {
        public ReactiveCommand OnScreenOpen { get; } = new ReactiveCommand();
        public ReactiveCommand OnScreenClose { get; } = new ReactiveCommand();

        [SerializeField] private CanvasGroup canvasGroup;

        private const float OpenDuration = 0.3f;
        private const float CloseDuration = 0.3f;

        private Tween _tween;
        
        private void OnValidate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Button]
        public void OpenScreen()
        {
            if (!Application.isPlaying)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            
            DOTween.Kill(_tween);
            
            _tween = canvasGroup.DOFade(1f, OpenDuration).OnComplete(() =>
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                OnScreenOpen.Execute();
            });
            
            
        }

        [Button]
        public void CloseScreen()
        {
            if (!Application.isPlaying)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            
            DOTween.Kill(_tween);

            _tween = canvasGroup.DOFade(0f, CloseDuration).OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                OnScreenClose.Execute();
            });
        }
    }
}