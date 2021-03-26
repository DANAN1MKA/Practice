using UnityEngine;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    [Inject] private EnemiesPool enemiesPull;
    private GameObject character;
    private GameObject enemy;

    public void Awake()
    {
        character = Instantiate(config.characterPrefab);
        MainCharacterController characterScript = character.GetComponent<MainCharacterController>();

        characterScript.setSignalBus(signalBus);
        signalBus.Subscribe<IAmDeadSi>(createEnemy);
    }

    public void Start()
    {
        Vector2 newPositionPlayer = new Vector2(config.characterPosition.x * config.scale, 
                                          config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);



        character.transform.position = newPositionPlayer;
        character.transform.localScale *= config.scale;

        createEnemy();
    }

    private void createEnemy()
    {
        enemy = Instantiate(enemiesPull.pool[Random.Range(0, 2)]);
        EnemyController enemyScript = enemy.GetComponent<EnemyController>();
        enemyScript.setupEnemy(signalBus, 3);

        Vector3 newPositionEnemy = new Vector3(config.characterPosition.x * config.scale * -1 + 10,
                                       config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale, 1);

        Vector2 targetPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                               config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale);



        enemy.transform.position = newPositionEnemy;
        enemy.transform.localScale *= config.scale;

        signalBus.Fire(new NewEnemySignal(enemy, targetPositionEnemy));
    }
}
