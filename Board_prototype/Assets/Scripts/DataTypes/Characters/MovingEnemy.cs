using UnityEngine;

public class MovingEnemy
{
    public delegate void KillEnemy();
    public delegate void StopEnemy();

    public KillEnemy kill { get; private set; }
    public StopEnemy stop { get; private set; }

    public MovingEnemy nextEnemy { get; private set; }

    public GameObject enemy { get; private set; }

    public Vector2 targetPosition { get; private set; }

    public MovingEnemy(GameObject _enemy, Vector2 _targetPosition, MovingEnemy _next, KillEnemy killCallback, StopEnemy stopCallback)
    {
        enemy = _enemy;
        targetPosition = _targetPosition;
        nextEnemy = _next;
        kill = killCallback;
        stop = stopCallback;
    }
}
