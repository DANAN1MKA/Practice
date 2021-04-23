using System;
using UnityEngine;
using Zenject;

public class ItemsInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<UpdateTextUISignal>();
    }
}