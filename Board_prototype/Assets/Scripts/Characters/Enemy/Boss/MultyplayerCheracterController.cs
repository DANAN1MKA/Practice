﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultyplayerCheracterController : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    [Inject] private EnemiesPool enemiesPull;
    [Inject] private PlayerData playerData;
    [Inject] private HeroPool heroPool;

    private GameObject character;
    private GameObject enemy;

    private void Awake()
    {
        signalBus.Subscribe<SwipeDamageSignal>(attack);
        signalBus.Subscribe<VictorySignal>(victoryHandler);
        //TODO: герой врага
        signalBus.Subscribe<IsGameReadySignal>(setEnemyHero);
    }
    private void Start()
    {
        setupScene();
    }

    private void attack()
    {

    }

    private void victoryHandler(VictorySignal signal)
    {
        if (signal.victory)
        {
            //TODO: босс огребает
        }
        else
        {
            //TODO: игрок огребает
        }
    }

    private void setupScene()
    {
        character = Instantiate(playerData.currentHeroPrefab);
        MainCharacterController characterScript = character.GetComponent<MainCharacterController>();

        Vector2 newPositionPlayer = new Vector2(config.characterPosition.x * config.scale,
                                                config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);

        character.transform.position = newPositionPlayer;
        character.transform.localScale *= config.scale;




        enemy = Instantiate(enemiesPull.pool[Random.Range(0, enemiesPull.pool.Length)]);

        //пригодится когда у босса будут анимации
        //EnemyController enemyScript = enemy.GetComponent<EnemyController>(); 

        Vector2 targetPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                               config.boardPositionFromResolution.y + (config.height + 2.1f) * config.scale);



        enemy.transform.position = targetPositionEnemy;
        enemy.transform.localScale *= config.scale;

        //подрубаем анимауию атаки
        //characterScript.attack();
    }

    private void setEnemyHero(IsGameReadySignal signal)
    {
        if (signal.status)
        {
            //TODO: отладка
            Debug.Log("opponentHeroID" + signal.heroID);

            if (signal.heroID >= 0 && signal.heroID < heroPool.heroPrefab.Length)
            {
                setupEnemy(signal.heroID);
            }
            else
            {
                setupEnemy(0);
            }
        }
    }

    private void setupEnemy(int heroID)
    {
        Destroy(enemy);
        enemy = null;
        enemy = Instantiate(heroPool.heroPrefab[heroID]);
        MainCharacterController characterScript = character.GetComponent<MainCharacterController>();

        Vector2 targetPositionEnemy = new Vector2(config.characterPosition.x * config.scale * -1,
                                    config.boardPositionFromResolution.y + (config.height + 0.8f) * config.scale);

        enemy.transform.position = targetPositionEnemy;
        enemy.transform.localScale *= config.scale;
        enemy.transform.localScale *= new Vector2(-1, 1);
    }
}