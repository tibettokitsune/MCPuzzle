using _Game.Scripts.Configs;
using _Game.Scripts.Infrustructure;
using Zenject;

namespace _Game.Scripts.Gameplay
{
    public interface IUnitSpawner
    {
        public UnitController SpawnUnit(int id);

        public UnitController SpawnPlayer();
    }
    public class UnitSpawner : IUnitSpawner
    {
        [Inject] private IPlayerDataController _dataController;
        [Inject] private GameItemsConfig _config;
        [Inject] private MapConfig _mapConfig;
        readonly UnitController.Factory _factory;

        public UnitSpawner(UnitController.Factory factory)
        {
            _factory = factory;
        }
        public UnitController SpawnUnit(int id) => _factory.Create(_config.units[id].gameUnit);
        public UnitController SpawnPlayer()
        {
            var unit = _factory.Create(_config.units[_dataController.PlayerData.Value.skinId].gameUnit);
            unit.SetupUnit(_mapConfig.CurrentData(_dataController.PlayerData.Value.level).playerData.position, 
                _mapConfig.CurrentData(_dataController.PlayerData.Value.level).playerData.rotation);
            return unit;
        }
    }
}