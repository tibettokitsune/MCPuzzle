using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    [CreateAssetMenu(menuName = "GameData/Block", fileName = "Block")]
    public class BlockPreset : ScriptableObject
    {
        public BlockType blockType;
        [PreviewField]
        public GameObject viewPrefab;

        public float BlockStrenght;

        public Sprite icon;
    }
}