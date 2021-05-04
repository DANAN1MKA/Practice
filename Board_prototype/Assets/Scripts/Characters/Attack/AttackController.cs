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

    [SerializeField] private GameObject playerAttack;
    [SerializeField] private GameObject enemyAttack;


    private void Awake()
    {
        //signalBus.Subscribe<CheracterAttackSignal>(attack);

        //TODO: временно
        signalBus.Subscribe<SwipeDamageSignal>(attack);
    }

    private void Start()
    {
        playerAttack.transform.position = new Vector3((1.75f / 2), transform.position.y, 0);
        playerAttack.transform.localScale = new Vector3(4.76f / 2, 1, 1);

        enemyAttack.transform.position = new Vector3((-1.75f / 2), transform.position.y, 0);
        enemyAttack.transform.localScale = new Vector3(-4.76f / 2, 1, 1);
    }

    private void newValue()
    {
        playerAttack.transform.position = new Vector3((1.75f / 2) * currentValue, transform.position.y, 0);
        playerAttack.transform.localScale = new Vector3((4.76f / 2) * (2 - currentValue), 1, 1);

        enemyAttack.transform.position = new Vector3((-1.75f / 2) * (2 - currentValue), transform.position.y, 0);
        enemyAttack.transform.localScale = new Vector3((-4.76f / 2) * currentValue, 1, 1);
    }
    
    private void attack(SwipeDamageSignal signal)
    {
        float coef = (float)signal.damageAmount / 20f;

        Debug.Log("damage amount " + signal.damageAmount + "\n" +
                  "coef " + coef);


        targetValue = currentValue + (0.5f * coef);
        isActive = true;
        //value = value >= 2f ? 0.1f : value + 0.1f;
        //newValue();
    }

 

    private void Update()
    {
            //if (isActive) 
            if (isActive)
            {
                currentValue = currentValue < targetValue ? currentValue + (raySpeed * 6) : currentValue - raySpeed;

                if (currentValue >= targetValue)
                {
                    isActive = false;
                    if (currentValue >= 2) signalBus.Fire(new VictorySignal(true));
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
