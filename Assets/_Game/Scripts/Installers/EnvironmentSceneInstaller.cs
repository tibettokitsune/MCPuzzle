using _Game.Scripts.Gameplay;
using _Game.Scripts.Gameplay.EvironmentObjects;
using Cinemachine;
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
            
            Container.BindFactory<UnitController, UnitController, UnitController.Factory>().FromFactory<UnitController.Factory>();
            Container.BindInterfacesTo<UnitSpawner>().AsSingle();

            Container.Bind<CinemachineVirtualCamera>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerController>().AsSingle();
        }
    }
}