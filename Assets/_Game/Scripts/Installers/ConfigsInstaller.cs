using _Game.Scripts.Configs;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class ConfigsInstaller : MonoInstaller
    {
        public GameItemsConfig gameItemsConfig;
        public ScenesConfig scenesConfig;
        public MapConfig mapConfig;
        public override void InstallBindings()
        {
            Container.BindInstance(scenesConfig);
            Container.BindInstance(gameItemsConfig);
            Container.BindInstance(mapConfig);
        }
    }
}