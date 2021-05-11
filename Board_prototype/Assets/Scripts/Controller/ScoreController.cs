using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private PlayerData playerData;
    [Inject] private PlayerItems playerItems;

    [SerializeField] private int moneyPerKill;


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
        playerData.money += (System.UInt64)moneyPerKill;

        signalBus.Fire(new AddScoreSignal(score, (System.UInt64)moneyPerKill));
        signalBus.Fire<UpdateTextUISignal>();
    }


}
