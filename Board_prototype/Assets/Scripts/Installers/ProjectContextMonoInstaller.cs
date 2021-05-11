using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    public PlayerData playerData;
    public PlayerItems playerItems;
    public HeroPool heroPool;

    public override void InstallBindings()
    {
        //TODO: OdinStorageController
        Container.BindInterfacesAndSelfTo<StorageController>().AsSingle().NonLazy();

        Container.Bind<PlayerData>().FromInstance(playerData);
        Container.Bind<PlayerItems>().FromInstance(playerItems);
        Container.Bind<HeroPool>().FromInstance(heroPool);


    }
}