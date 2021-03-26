using UnityEngine;
using Zenject;

public class CharacterController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    private GameObject character;
    private GameObject enemy;

    public void Awake()
    {
        character = Instantiate(config.characterPrefab);
        MainCharacterController characterScript = character.GetComponent<MainCharacterController>();
        enemy = Instantiate(config.enemyPrefab);
        EnemyController enemyScript = enemy.GetComponent<EnemyController>();

        characterScript.setSignalBus(signalBus);
        enemyScript.setupEnemy(signalBus, 20);

    }

    public void Start()
    {
        Vector2 newPositionPlayer = new Vector2(config.characterPosition.x * config.scale, 
                                          config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);

        Vector2 newPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                                               config.boardPositionFromResolution.y + (config.height + 1.8f) * config.scale);


        character.transform.position = newPositionPlayer;
        enemy.transform.position = newPositionEnemy;
        character.transform.localScale *= config.scale;
        character.transform.localScale *= config.scale;
    }
}
