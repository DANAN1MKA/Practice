using UnityEngine;

public class NewEnemySignal
{
    public GameObject enemy { get; private set; }
    public Vector2 targetPosition { get; private set; }

    public NewEnemySignal(GameObject _enemy, Vector2 _targetPosition)
    {
        enemy = _enemy;
        targetPosition = _targetPosition;
    }

}
