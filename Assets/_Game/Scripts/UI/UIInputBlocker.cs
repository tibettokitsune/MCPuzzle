using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI
{
    public class UIInputBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsOverUIElement { get; private set; }
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsOverUIElement = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsOverUIElement = false;
        }
    }
}