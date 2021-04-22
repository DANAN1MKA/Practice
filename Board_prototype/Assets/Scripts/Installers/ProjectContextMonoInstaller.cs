using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    public PlayerData playerData;
    public PlayerItems playerItems;

    public override void InstallBindings()
    {
        //TODO: Storege BIND
        Container.BindInterfacesAndSelfTo<StorageController>().AsSingle().NonLazy();

        Container.Bind<PlayerData>().FromInstance(playerData);
        Container.Bind<PlayerItems>().FromInstance(playerItems);

        //setup();

    }

    //TODO: одноразовое решение чисто для проверки
    public void setup()
    {
        playerItems.itemData = new ItemData[DefaultCoef.itemsData.Length];
        //Array.Copy(DefaultCoef.itemsData, playerItems.itemData, DefaultCoef.itemsData.Length);
        for (int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            playerItems.itemData[i] = new ItemData();
            playerItems.itemData[i].itemName = DefaultCoef.itemsData[i].itemName;
            playerItems.itemData[i].baseCoast = 0 + DefaultCoef.itemsData[i].baseCoast;
            playerItems.itemData[i].baseGrowthRate = 0 + DefaultCoef.itemsData[i].baseGrowthRate;
            playerItems.itemData[i].isBought = false;
            playerItems.itemData[i].level = 0;
        }
    }

}