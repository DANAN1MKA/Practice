using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HeroItemController : MonoBehaviour
{
    private SignalBus signalBus;

    public delegate void updateUICallback();
    public updateUICallback updateUIcb;


    [SerializeField] Text heroName;
    [SerializeField] Text heroDescription;
    [SerializeField] Button actionButton;
    [SerializeField] Text actionButtonText;
    [SerializeField] Image portrait;


    private int ID;
    private HeroPool heroPool;
    private PlayerData playerData;

    public void setupItem(int _ID, HeroPool _heroPool, PlayerData _playerData, SignalBus _signalBus)
    {
        ID = _ID;
        heroPool = _heroPool;
        playerData = _playerData;
        signalBus = signalBus;

        //signalBus.Subscribe<UpdateTextUISignal>(updateUI);

        updateUI();
    }

    public void actioonButtonClick()
    {
        if (heroPool.isBought[ID])
        {
            playerData.currentHeroPrefab = heroPool.heroPrefab[ID];
            playerData.currentHeroID = ID;
        }
        else
        {
            if(playerData.money >= (System.UInt64)heroPool.price[ID])
            {
                heroPool.isBought[ID] = true;
                playerData.currentHeroID = ID;
                playerData.currentHeroPrefab = heroPool.heroPrefab[ID];
                playerData.money -= (System.UInt64)heroPool.price[ID];
            }
        }
        updateUI();
        updateUIcb();

    }

    public void updateUI()
    {
        heroName.text = heroPool.name[ID];
        portrait.sprite = heroPool.portrait[ID];
        if (heroPool.isBought[ID])
        {
            if (playerData.currentHeroID != ID)
                actionButtonText.text = "Выбрать";
            else 
            { 
                actionButtonText.text = "Выбран";
            }

        }
        else actionButtonText.text = heroPool.price[ID] + " валежника";

        //TODO: портрет и описание
    }

    public void showHero()
    {

    }
}
