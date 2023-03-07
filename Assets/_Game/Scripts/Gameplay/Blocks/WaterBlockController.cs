using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class WaterBlockController : BlockController
    {
        public override BlockType BlockType => BlockType.Water;

        public WaterBlockController(Vector3Int position, BlockType blockType) : base(position, blockType)
        {
            Walkable = true;
        }
    }
}