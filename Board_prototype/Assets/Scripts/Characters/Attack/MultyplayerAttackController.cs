using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class MultyplayerAttackController : MonoBehaviour
{
    [Inject] SignalBus signalBus;


    [SerializeField] float maxPositionValue = 1.75f;
    [SerializeField] float maxScaleValue = 4.76f;

    // >0,  <2
    private float currentValue = 1f;
    private float targetValue = 1f;
    [SerializeField] private float raySpeed;
    [SerializeField] private float attackCoef;
    private bool isActive = false;

    [SerializeField] private GameObject playerAttackGameObj;
    [SerializeField] private GameObject enemyAttackGameObj;
    [SerializeField] private GameObject sparks;


    private bool isItPlayerAttack;
    private void setActive()
    {
        isActive = true;
    }

    private void Awake()
    {
        //TODO: временно
        signalBus.Subscribe<SwipeDamageSignal>(playerAttack);
        signalBus.Subscribe<ServerReplaySignal>(enemyAttack);
        signalBus.Subscribe<ReplayCompliteSignal>(setActive);
    }

    private void Start()
    {
        playerAttackGameObj.transform.localPosition = new Vector3((maxPositionValue / 2), 0, 0);
        playerAttackGameObj.transform.localScale = new Vector3(maxScaleValue / 2, 0.4f, 1);

        enemyAttackGameObj.transform.localPosition = new Vector3((-maxPositionValue / 2), 0, 0);
        enemyAttackGameObj.transform.localScale = new Vector3(-maxScaleValue / 2, 0.4f, 1);

        sparks.transform.localPosition = new Vector2(0, 0);
    }

    private void newValue()
    {
        playerAttackGameObj.transform.localPosition = new Vector3((maxPositionValue / 2) * currentValue, 0, 0);
        playerAttackGameObj.transform.localScale = new Vector3((maxScaleValue / 2) * (2 - currentValue), 0.4f, 1);

        enemyAttackGameObj.transform.localPosition = new Vector3((-maxPositionValue / 2) * (2 - currentValue), 0, 0);
        enemyAttackGameObj.transform.localScale = new Vector3((-maxScaleValue / 2) * currentValue, 0.4f, 1);

        float sparkPositionCoef = (1f - currentValue) * -1;

        sparks.transform.localPosition = new Vector2(maxPositionValue * sparkPositionCoef, 0);
    }

    private void playerAttack(SwipeDamageSignal signal)
    {

        float coef = (float)signal.damageAmount / 20f;
        targetValue = targetValue + (attackCoef * coef);

        if (targetValue > 2f) targetValue = 2f;

        //TODO: отладка
        Debug.Log("playerAttack coef = " + coef + "\ntargetValue = " + targetValue);

        //isItPlayerAttack = true;
        //isActive = true;
    }

    private void enemyAttack(ServerReplaySignal signal)
    {
        float enemyCoef = (float)signal.json.damageAmount / 20f;
        targetValue = targetValue - (attackCoef * enemyCoef);

        if (targetValue < 0) targetValue = 0;

        //TODO: отладка
        Debug.Log("enemyAttack coef = " + enemyCoef + "\ntargetValue = " + targetValue);

        //isItPlayerAttack = false;
        //isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            if (currentValue != targetValue)
            {
                if (currentValue < targetValue)
                {
                    currentValue = currentValue + raySpeed > targetValue ? targetValue : currentValue + raySpeed;
                }
                if (currentValue > targetValue)
                {
                    currentValue = currentValue - raySpeed < targetValue ? targetValue : currentValue - raySpeed;
                }

                if (currentValue == targetValue)
                {
                    signalBus.Fire<KillingCompletedSignal>();
                    isActive = false;
                }
            }

            if (currentValue >= 2)
            {
                victoryHandler(true);
                newValue();
                return;
            }
            if (currentValue <= 0)
            {
                victoryHandler(false);
                newValue();
                return;
            }

            newValue();
        }
    }

    private void victoryHandler(bool _victory)
    {
        signalBus.Fire(new VictorySignal(_victory));
        isActive = false;
    }
}
