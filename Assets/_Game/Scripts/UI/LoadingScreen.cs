using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class LoadingScreen : UIScreen
    {
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private TextMeshProUGUI percentLbl;
        public void StartLoading(Queue<AsyncOperation> loadingOperations)
        {
            OpenScreen();

            StartCoroutine(Loading(loadingOperations));
        }

        private IEnumerator Loading(Queue<AsyncOperation> loadingOperations)
        {
            while (loadingOperations.Count > 0)
            {
                var o = loadingOperations.Dequeue();
                while (!o.isDone)
                {
                    percentLbl.text = o.progress.ToString("F0");
                    loadingSlider.value = o.progress;
                    yield return null;
                }
            }
            
            CloseScreen();
        }
    }
}