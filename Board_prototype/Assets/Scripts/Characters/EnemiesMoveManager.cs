using UnityEngine;
using Zenject;

public class EnemiesMoveManager : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    private float speed = 30f;

    MovingEnemy enemy;

    private bool isActive;

    private void Awake()
    {
        signalBus.Subscribe<NewEnemySignal>(setNewEnemy);
        signalBus.Subscribe<CheracterAttackSignal>(nextEnemy);
        isActive = true;
    }

    void Update()
    {
        if (isActive && enemy != null)
        {
            enemy.enemy.transform.position = Vector2.MoveTowards(enemy.enemy.transform.position, enemy.targetPosition, Time.deltaTime * speed);

            if (isOnPosition())
            {
                enemy.stop();
                isActive = false;
                signalBus.Fire<MoveEnemyCompliteSignal>();
            }
        }

    }
    private void nextEnemy()
    {
        if (enemy.nextEnemy != null)
        {
            enemy = enemy.nextEnemy;
            isActive = true;
        }
    }

    private bool isOnPosition()
    {
        return (enemy.enemy.transform.position.x == enemy.targetPosition.x &&
                enemy.enemy.transform.position.y == enemy.targetPosition.y);
    }

    private void setNewEnemy(NewEnemySignal signal)
    {
        //if (enemy != null) 
        //    enemy.kill();
        enemy = signal.enemies;
        isActive = true;
    }
}
