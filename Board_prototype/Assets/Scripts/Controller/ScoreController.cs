using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private PlayerData playerData;

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
        signalBus.Fire(new AddScoreSignal(228, 4));
        playerData.score += 228;
        playerData.money += 4;
    }


}
