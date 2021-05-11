using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreGeneratorBoss : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;
    [Inject] private PlayerItems playerItems;
    [Inject] private PlayerData playerData;


    [SerializeField] GameObject scoreTextPrefab;
    [SerializeField] GameObject moneyTextPrefab;
    Vector2 newPositionScore;
    Vector2 newPositionMoney;

    System.UInt64 score = 1;
    System.UInt64 money = 1;

    private bool isActive = false;
    private float timer;
    [SerializeField] private float timeStep;


    //TODO: валюта
    [SerializeField] private int moneyPerTimestep;
    //TODO: попробуем
    [SerializeField] private GameObject anchor;

    private void enable()
    {
        generateScore();
        timer = Time.time + timeStep;
        isActive = true;
    }

    private void disable()
    {
        isActive = false;
    }


    private void Awake()
    {
        signalBus.Subscribe<SwipeDamageSignal>(enable);
        signalBus.Subscribe<KillingCompletedSignal>(disable);
        signalBus.Subscribe<VictorySignal>(disable);
    }

    private void Start()
    {
        calculateScore();

        newPositionScore = new Vector2(config.characterPosition.x * config.scale + 1,
                                          config.boardPositionFromResolution.y + (config.height + 3f) * config.scale);

        newPositionMoney = new Vector2(config.characterPosition.x * config.scale + 1,
                                          config.boardPositionFromResolution.y + (config.height + 2.6f) * config.scale);

        newPositionScore = RectTransformUtility.WorldToScreenPoint(GetComponentInParent<Camera>(), newPositionScore);
        newPositionMoney = RectTransformUtility.WorldToScreenPoint(GetComponentInParent<Camera>(), newPositionMoney);

    }

    private void Update()
    {
        if (isActive)
        {
            if(Time.time >= timer)
            {
                generateScore();
                timer = Time.time + timeStep;
            }
        }
    }

    private void generateScore()
    {
        GameObject newTextScore = Instantiate(scoreTextPrefab, this.transform);
        newTextScore.transform.position = newPositionScore;
        TextMover scriptScore = newTextScore.GetComponent<TextMover>();
        scriptScore.setup(score);

        Destroy(newTextScore, 3f);


        GameObject newTextMoney = Instantiate(moneyTextPrefab, this.transform);
        newTextMoney.transform.position = newPositionMoney;
        TextMover scriptMoney = newTextMoney.GetComponent<TextMover>();
        scriptMoney.setup((System.UInt64)moneyPerTimestep);

        Destroy(newTextMoney, 3f);

        playerData.score += score;
        playerData.money += (System.UInt64)moneyPerTimestep;

    }

    private void calculateScore()
    {
        for (int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            if (playerItems.itemData[i].isBought)
                score += playerItems.itemData[i].baseGrowthRate;
        }

        score += score;
        money += (System.UInt64)moneyPerTimestep;
    }



}
