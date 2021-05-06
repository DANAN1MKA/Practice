using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AttackController : MonoBehaviour
{
    [Inject] SignalBus signalBus;


    [SerializeField] float maxPositionValue = 1.75f;
    [SerializeField] float maxScaleValue = 4.76f;

    // >0,  <2
    private float currentValue = 1f;
    private float targetValue = 1f;
    [SerializeField] private float raySpeed;
    private bool isActive = false;
    private bool fightToggle = true;

    [SerializeField] private GameObject playerAttack;
    [SerializeField] private GameObject enemyAttack;
    [SerializeField] private GameObject sparks;


    private void Awake()
    {
        signalBus.Subscribe<MoveManagerSwipeSignal>(start);


        //TODO: временно
        signalBus.Subscribe<SwipeDamageSignal>(attack);
    }

    private void Start()
    {
        playerAttack.transform.localPosition = new Vector3((maxPositionValue / 2), 0, 0);
        playerAttack.transform.localScale = new Vector3(maxScaleValue / 2, 0.4f, 1);

        enemyAttack.transform.localPosition = new Vector3((-maxPositionValue / 2), 0, 0);
        enemyAttack.transform.localScale = new Vector3(-maxScaleValue / 2, 0.4f, 1);

        sparks.transform.localPosition = new Vector2(0, 0);
    }

    private void newValue()
    {
        playerAttack.transform.localPosition = new Vector3((maxPositionValue / 2) * currentValue, 0, 0);
        playerAttack.transform.localScale = new Vector3((maxScaleValue / 2) * (2 - currentValue), 0.4f, 1);

        enemyAttack.transform.localPosition = new Vector3((-maxPositionValue / 2) * (2 - currentValue), 0, 0);
        enemyAttack.transform.localScale = new Vector3((-maxScaleValue / 2) * currentValue, 0.4f, 1);

        float sparkPositionCoef = (1f - currentValue) * -1;

        sparks.transform.localPosition = new Vector2(maxPositionValue * sparkPositionCoef, 0);
    }
    
    private void attack(SwipeDamageSignal signal)
    {
        float coef = (float)signal.damageAmount / 20f;

        //TODO: отладка
        Debug.Log("damage amount " + signal.damageAmount + "\n" +
                  "coef " + coef);


        targetValue = currentValue + (0.5f * coef);
        isActive = true;
    }

    private void start()
    {
        fightToggle = false;

        signalBus.Unsubscribe<MoveManagerSwipeSignal>(start);
    }

 

    private void Update()
    {
        if (fightToggle) return;

        if (isActive)
        {
            currentValue = currentValue < targetValue ? currentValue + (raySpeed * 6) : currentValue - raySpeed;

            if (currentValue >= targetValue)
            {
                isActive = false;
                if (currentValue >= 2) 
                { 
                    signalBus.Fire(new VictorySignal(true));
                    fightToggle = true;
                }
                else signalBus.Fire<KillingCompletedSignal>();
            }
        }
        else
        {
            currentValue = currentValue < targetValue ? currentValue - raySpeed : currentValue - raySpeed;

            if (currentValue <= 0) signalBus.Fire(new VictorySignal(false));
        }
        if (currentValue > 0 && currentValue < 2f) newValue();
    }
}
