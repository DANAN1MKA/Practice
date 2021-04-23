using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    public PlayerData playerData;
    public PlayerItems playerItems;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StorageController>().AsSingle().NonLazy();

        Container.Bind<PlayerData>().FromInstance(playerData);
        Container.Bind<PlayerItems>().FromInstance(playerItems);

    }
}