using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private PlayerData playerData;
    [Inject] private PlayerItems playerItems;


    private int currentDamage;

    void Awake()
    {
        signalBus.Subscribe<SwipeDamageSignal>(getDamage);
        signalBus.Subscribe<CheracterAttackSignal>(sendScoreMoney);
    }

    private void getDamage(SwipeDamageSignal signal)
    {
        currentDamage = signal.damageAmount;
    }

    private void sendScoreMoney()
    {
        System.UInt64 score = 1;

        for(int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            if(playerItems.itemData[i].isBought)
                score += playerItems.itemData[i].baseGrowthRate;
        }

        playerData.score += score;
        playerData.money += 4;

        signalBus.Fire(new AddScoreSignal(score, 4));
        signalBus.Fire<UpdateTextUISignal>();
    }


}
