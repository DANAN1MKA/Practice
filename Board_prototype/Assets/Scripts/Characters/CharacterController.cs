using UnityEngine;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    [Inject] private EnemiesPool enemiesPull;

    private GameObject character;
    private delegate void CharacterAtack();
    private CharacterAtack characterAtack;

    private GameObject enemy;

    private int damageAmount;
    private int comboCount;

    MovingEnemy movingEnemy = null;

    public void Awake()
    {
        signalBus.Subscribe<SwipeDamageSignal>(createEnemy);
        signalBus.Subscribe<MoveEnemyCompliteSignal>(nextEnemy);
    }

    public void Start()
    {

        character = Instantiate(config.characterPrefab);
        MainCharacterController characterScript = character.GetComponent<MainCharacterController>();

        characterAtack = characterScript.attack;

        Vector2 newPositionPlayer = new Vector2(config.characterPosition.x * config.scale, 
                                                config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);

        character.transform.position = newPositionPlayer;
        character.transform.localScale *= config.scale;

        createEnemy(new SwipeDamageSignal(0));
    }

    private void nextEnemy()
    {
        if (comboCount > 0)
        {
            if (movingEnemy.nextEnemy != null)
            {
                killEnemy();
                signalBus.Fire<CheracterAttackSignal>();
                comboCount--;
            }
            else
            if (movingEnemy.nextEnemy == null)
            {
                //TODO: убийства завершены сигнал
                Debug.Log("убийства завершены сигнал");

                signalBus.Fire<KillingCompletedSignal>();
            }

        }
    }

    private void killEnemy()
    {
        characterAtack();
        movingEnemy.kill();
        movingEnemy = movingEnemy.nextEnemy;
    }

    private void createEnemy(SwipeDamageSignal signal)
    {
        if(movingEnemy != null) killEnemy();
        int scale = 3;
        comboCount = 0;

        damageAmount = signal.damageAmount;

        int i = 0;
        do
        {
            instantiateEnemy();

            i += scale;
            scale += 3;
            comboCount++;

        } while (damageAmount > i);

        signalBus.Fire(new NewEnemySignal(movingEnemy));
    }

    public void instantiateEnemy()
    {
        enemy = Instantiate(enemiesPull.pool[Random.Range(0, enemiesPull.pool.Length)]);
        EnemyController enemyScript = enemy.GetComponent<EnemyController>();

        Vector3 newPositionEnemy = new Vector3(config.characterPosition.x * config.scale * -1 + 5,
                                               config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale + 5, 1);

        Vector2 targetPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                               config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale);



        enemy.transform.position = newPositionEnemy;
        enemy.transform.localScale *= config.scale;

        MovingEnemy nextEnemy = new MovingEnemy(enemy, targetPositionEnemy, movingEnemy, enemyScript.die, enemyScript.stopJump);
        movingEnemy = nextEnemy;
    }
}
