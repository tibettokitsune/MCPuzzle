using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.UI
{
    public class MoneyEffect : MonoBehaviour
    {
        [SerializeField] private RectTransform moneyEffectElement;
        [SerializeField] private RectTransform fromPos;
        [SerializeField] private RectTransform toPos;

        [SerializeField] private Vector2Int numberDispersion;
        [SerializeField] private Vector2 sieDispersion;
        [SerializeField] private float movementRadius;

        [SerializeField] private Ease fromEase;
        [SerializeField] private Ease toEase;

        [SerializeField] private float fromTime;
        [SerializeField] private float toTime;

        private List<RectTransform> _moneyEffects = new List<RectTransform>();
        [Button]
        public void Effect(Action oneEnd)
        {
            StartCoroutine(AllEffectsWork(oneEnd));
        }

        private IEnumerator AllEffectsWork(Action oneEnd)
        {
            var numberOfEffects = Random.Range(numberDispersion.x, numberDispersion.y);
            var numberOfEndedEffects = 0;
            for (var i = 0; i < numberOfEffects; i++)
            {
                StartCoroutine(MoneyEffectWork(() => numberOfEndedEffects++));
            }

            while (numberOfEndedEffects < numberOfEffects)
            {
                yield return null;
            }
            
            oneEnd?.Invoke();
        }

        private IEnumerator MoneyEffectWork(Action endEffect)
        {
            var moneyEffect = Instantiate(moneyEffectElement, transform);
            _moneyEffects.Add(moneyEffect);
            moneyEffect.transform.position = fromPos.transform.position;
            
            var scale = Random.Range(sieDispersion.x, sieDispersion.y);
            bool isEnded = false;
            var seq = DOTween.Sequence(
                    moneyEffect.transform.DOScale(scale, fromTime + Random.Range(-0.1f, 0.1f)))
                .Append(
                    moneyEffect.DOAnchorPos(fromPos.anchoredPosition + Random.insideUnitCircle * movementRadius, fromTime + Random.Range(-0.1f, 0.1f)).SetEase(fromEase))
                .OnComplete(() => isEnded = true);
            while (!isEnded)
            {
                yield return null;
            }

            isEnded = false;
            
            seq = DOTween.Sequence(
                    moneyEffect.transform.DOScale(sieDispersion.x * 0.8f, toTime + Random.Range(-0.1f, 0.1f)))
                .Append(
                    moneyEffect.DOMove(toPos.position , 1f).SetEase(toEase))
                .OnComplete(() => isEnded = true);
            while (!isEnded)
            {
                yield return null;
            }

            _moneyEffects.Remove(moneyEffect);
            Destroy(moneyEffect.gameObject);
            endEffect.Invoke();
            
        }

        private void OnDisable()
        {
            if(_moneyEffects.Count <= 0) return;
            for (var i = _moneyEffects.Count - 1; i >= 0; i--)
            {
                DOTween.Kill(_moneyEffects[i].transform);
                Destroy(_moneyEffects[i].gameObject);
            }

            _moneyEffects = new List<RectTransform>();
        }
    }
}