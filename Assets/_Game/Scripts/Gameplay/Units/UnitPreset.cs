using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    [CreateAssetMenu(menuName = "GameData/Character", fileName = "Character")]
    public class UnitPreset : ScriptableObject
    {
        public UnitController gameUnit;
        public Sprite icon;
        public string unitName;
    }
}