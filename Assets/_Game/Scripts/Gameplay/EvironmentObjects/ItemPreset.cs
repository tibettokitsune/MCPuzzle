using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Gameplay.EvironmentObjects
{
    [CreateAssetMenu(menuName = "GameData/Item", fileName = "Item")]
    public class ItemPreset : ScriptableObject
    {
        public ItemType itemType;
        [PreviewField]
        public ItemController viewPrefab;

        public Vector3Int[] occupiedBlocks;
        public Vector3Int[] interactionBlocks;

        public Sprite icon;
    }
}