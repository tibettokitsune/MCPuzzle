using _Game.Scripts.Gameplay;
using _Game.Scripts.Gameplay.EvironmentObjects;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class EnvironmentSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MapController>().AsSingle();
            
            Container.BindFactory<ItemController, ItemController, ItemController.Factory>().FromFactory<ItemController.Factory>();
            Container.BindInterfacesTo<ItemSpawner>().AsSingle();
        }
    }
}