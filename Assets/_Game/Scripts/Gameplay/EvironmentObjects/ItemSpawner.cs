namespace _Game.Scripts.Gameplay.EvironmentObjects
{
    public interface IItemSpawner
    {
        ItemController SpawnItem(ItemController prefab);
    }
    public class ItemSpawner : IItemSpawner
    {
        
        readonly ItemController.Factory _factory;

        public ItemSpawner(ItemController.Factory factory)
        {
            _factory = factory;
        }

        public ItemController SpawnItem(ItemController prefab) => _factory.Create(prefab);
    }
}