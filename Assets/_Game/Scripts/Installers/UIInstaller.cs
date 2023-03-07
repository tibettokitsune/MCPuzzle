using _Game.Scripts.UI;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingScreen>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<InventoryPanel>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<TutorialScreen>().FromComponentInHierarchy().AsSingle();
        }
    }
}