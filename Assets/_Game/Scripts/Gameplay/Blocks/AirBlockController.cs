using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class AirBlockController : BlockController
    {
        public override BlockType BlockType => BlockType.Air;

        public AirBlockController(Vector3Int position, BlockType blockType) : base(position, blockType)
        {
            Walkable = true;
        }
    }
}