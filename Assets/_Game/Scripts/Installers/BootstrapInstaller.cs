using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay;
using _Game.Scripts.Infrustructure;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        
        Container.BindInterfacesTo<PlayerDataController>().AsSingle();
        Container.BindInterfacesTo<SceneController>().AsSingle();
        
        Container.BindInterfacesTo<InputController>().AsSingle();

        Container.BindInstance(new LevelEvents());

        Container.BindInterfacesTo<InventoryController>().AsSingle();
        Container.BindInterfacesTo<VibrationController>().AsSingle();

        Container.BindInterfacesTo<TutorialController>().AsSingle();
    }
}
