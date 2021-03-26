using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemiesMoveManager : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    private float speed = 4f;
    private GameObject movingEnemy;
    private Vector2 targetPosition;
    private bool isActive;

    private void Awake()
    {
        signalBus.Subscribe<NewEnemySignal>(setNewEnemy);
        isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            movingEnemy.transform.position = Vector2.MoveTowards(movingEnemy.transform.position, targetPosition, Time.deltaTime * speed);

            if (isOnPosition())
            {
                movingEnemy = null;
                isActive = false;
                signalBus.Fire<MoveEnemyCompliteSignal>();
            }
        }

    }
    private bool isOnPosition()
    {
        return (movingEnemy.transform.position.x == targetPosition.x &&
                movingEnemy.transform.position.y == targetPosition.y);
    }

    private void setNewEnemy(NewEnemySignal signal)
    {
        //TODO: отладка
        Debug.Log("пиздарики");

        movingEnemy = signal.enemy;
        targetPosition = signal.targetPosition;
        isActive = true;
    }
}
