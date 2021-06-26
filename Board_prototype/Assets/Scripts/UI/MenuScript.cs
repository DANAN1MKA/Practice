using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuScript : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    [Inject] private StorageController storageController;
    [Inject] private PlayerData playerData;
    [Inject] private ItemsDataSObj playerItems;
    [Inject] private HeroPool heroPool;


    public void resume()
    {
        transform.gameObject.SetActive(false);
    }

    public void save()
    {
        storageController.saveData();
    }

    public void resetData()
    {
        playerData.currentHeroID = 0;
        playerData.currentHeroPrefab = heroPool.heroPrefab[playerData.currentHeroID];
        playerData.score = 0;
        playerData.money = 0;

        resetItems();
        resetHeroes();

        storageController.saveData();

        signalBus.Fire<UpdateTextUISignal>();
    }

    public void exit()
    {
        Application.Quit();
    }

    private void resetItems()
    {
        //playerItems. = new ItemData[DefaultCoef.itemsData.Length];
        //Array.Copy(DefaultCoef.itemsData, playerItems.itemData, DefaultCoef.itemsData.Length);
        for (int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            playerItems.baseCoast[i] = 0 + DefaultCoef.itemsData[i].baseCoast;
            playerItems.baseGrowthRate[i] = 0 + DefaultCoef.itemsData[i].baseGrowthRate;
            playerItems.isBought[i] = false;
            playerItems.level[i] = 0;
        }
    }

    private void resetHeroes()
    {
        for (int i = 1; i < heroPool.isBought.Length; i++)
            heroPool.isBought[i] = false;

    }
}
