using _Game.Scripts.Gameplay;
using _Game.Scripts.Gameplay.EvironmentObjects;
using UnityEngine;

namespace _Game.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameItems", menuName = "Configs/GameItems")]
    public class GameItemsConfig : ScriptableObject
    {
        public BlockPreset[] blockPresets;

        public ItemPreset[] itemPresets;
        
        public DestructionBlock destructionBlockPrefab;
    }
}