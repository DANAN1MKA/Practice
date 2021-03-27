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
    private int comboScale;

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

        characterScript.setSignalBus(signalBus);
        characterAtack = characterScript.Attack;

        Vector2 newPositionPlayer = new Vector2(config.characterPosition.x * config.scale, 
                                          config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);



        character.transform.position = newPositionPlayer;
        character.transform.localScale *= config.scale;

        createEnemy(new SwipeDamageSignal(0));
    }

    private void nextEnemy()
    {
        if (damageAmount > 1)
        {
            killEnemy();
            signalBus.Fire<CheracterAttackSignal>();
            damageAmount -= comboScale;
            comboScale += 3;

        }
        if (movingEnemy == null ||  movingEnemy.nextEnemy == null) signalBus.Fire<KillingCompletedSignal>();
        Debug.Log("comboScale " + comboScale);
    }

    private void killEnemy()
    {
        if (movingEnemy != null && movingEnemy.nextEnemy != null)
        {
            characterAtack();
            movingEnemy.kill();
            movingEnemy = movingEnemy.nextEnemy;
        }
    }

    private void createEnemy(SwipeDamageSignal signal)
    {
        killEnemy();
        int scale = 3;
        comboScale = 0;

        damageAmount = signal.damageAmount;

        int i = 0;
        do
        {
            enemy = Instantiate(enemiesPull.pool[Random.Range(0, enemiesPull.pool.Length)]);
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();
            enemyScript.setupEnemy(signalBus, 3);

            Vector3 newPositionEnemy = new Vector3(config.characterPosition.x * config.scale * -1 + 5,
                                           config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale + 5, 1);

            Vector2 targetPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                                   config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale);



            enemy.transform.position = newPositionEnemy;
            enemy.transform.localScale *= config.scale;

            MovingEnemy nextEnemy = new MovingEnemy(enemy, targetPositionEnemy, movingEnemy, enemyScript.recieveDamage, enemyScript.stopJump);
            movingEnemy = nextEnemy;

            i += scale;
            scale += 3;
            Debug.Log("scale " + scale);

        } while (damageAmount > i);


        signalBus.Fire(new NewEnemySignal(movingEnemy));
    }
}
