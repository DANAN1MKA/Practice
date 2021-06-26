using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    public PlayerData playerData;
    public ItemsDataSObj playerItems;
    public HeroPool heroPool;
    //public ItemsDataSObj 

    public override void InstallBindings()
    {
        //TODO: OdinStorageController
        Container.BindInterfacesAndSelfTo<StorageController>().AsSingle().NonLazy();

        Container.Bind<PlayerData>().FromInstance(playerData);
        Container.Bind<ItemsDataSObj>().FromInstance(playerItems);
        Container.Bind<HeroPool>().FromInstance(heroPool);


    }
}