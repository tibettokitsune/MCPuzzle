using UnityEngine;
using Zenject;

namespace _Game.Scripts.Gameplay.EvironmentObjects
{
    public enum ItemType
    {
        Chest = 0,
        Forcer = 1,
        Piston = 2
        
    }
    
    public class ItemController : MonoBehaviour
    {
        [Inject] protected IMapController _mapController;
        public Vector3Int Position { get; private set; }

        public Vector3 Rotation { get; private set; }
        
        public Vector3Int[] OccupiedBlocks { get; private set; }
        public Vector3Int[] InteractionBlocks { get; private set; }

        
        public virtual ItemType ItemType { get; private set; }

        public virtual void Start()
        {
            SetupPosition();
            foreach (var occupiedBlock in OccupiedBlocks)
            {
                _mapController.DoBlockNotWalkable(RealPosition(occupiedBlock));
            }
        }

        public void SetupPosition()
        {
            transform.position = Position;
            transform.eulerAngles = Rotation;
        }

        public void Setup(MapItemsData itemsData, ItemPreset preset)
        {
            Position = itemsData.position;
            Rotation = itemsData.rotation;

            OccupiedBlocks = preset.occupiedBlocks;
            InteractionBlocks = preset.interactionBlocks;
            ItemType = preset.itemType;
        }

        protected bool IsBlockInteraction(Vector3Int pos)
        {
            foreach (var intBlock in InteractionBlocks)
            {
                if (RealPosition(intBlock).Equals(pos))
                    return true;
            }

            return false;
        }

        protected Vector3Int RealPosition(Vector3Int additiveVector) 
            => Position + MathHelper.RotateVector3Int(additiveVector, Rotation + new Vector3(0, 180, 0));
        
        public class Factory : PlaceholderFactory<ItemController, ItemController>
        {
            readonly DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public ItemController Create(UnityEngine.Object prefab)
            {
                return _container.InstantiatePrefabForComponent<ItemController>(prefab);
            }
        }
    }
}