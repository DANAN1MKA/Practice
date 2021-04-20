using System;
using UnityEngine;
using Zenject;

public class ItemsInstaller : MonoInstaller
{

    public PlayerData playerData;
    public PlayerItems playerItems;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<UpdateTextUISignal>();



        Container.Bind<PlayerData>().FromInstance(playerData);
        Container.Bind<PlayerItems>().FromInstance(playerItems);
    }
}