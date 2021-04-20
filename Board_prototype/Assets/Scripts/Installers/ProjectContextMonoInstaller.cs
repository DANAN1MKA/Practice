using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    public PlayerItems playerItems;


    public override void InstallBindings()
    {
        setup();

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