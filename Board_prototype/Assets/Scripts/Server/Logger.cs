using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Logger : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    private void Awake()
    {
        signalBus.Subscribe<StartBoardStateSignal>(startLogger);
        signalBus.Subscribe<SwipeElementSignal>(rememberSwipe);
        signalBus.Subscribe<MoveManagerSwipeSignal>(verifySwipe);
        signalBus.Subscribe<NewGemsSignal>(newGems);
        signalBus.Subscribe<SwipeDamageSignal>(generateJSON);
    }

    [Inject] BoardProperties config;

    private LiteElemet[,] boardStartState;


    private void startLogger(StartBoardStateSignal signal)
    {
        boardStartState = new LiteElemet[config.width, config.height];
        swipeHistory = new List<SwipeData>();
        newGemsType = new List<int>();

        for (int i = 0; i < config.width; i++)
            for(int j = 0; j < config.height; j++)
            {
                LiteElemet newElement = new LiteElemet();
                newElement.posX = signal.startState[i, j].posX;
                newElement.posY = signal.startState[i, j].posY;
                newElement.type = signal.startState[i, j].type;
                boardStartState[i, j] = newElement;
            }
    }

    private SwipeData lastSwipe;
    private bool isVerifyed = true;
    private List<SwipeData> swipeHistory;

    private void rememberSwipe(SwipeElementSignal signal)
    {
        if (isVerifyed)
        {
            lastSwipe = new SwipeData();
            lastSwipe.posX = signal.posX;
            lastSwipe.posY = signal.posY;
            lastSwipe.direction = signal.direction;
            isVerifyed = false;
        }
    }

    private void verifySwipe(MoveManagerSwipeSignal signal)
    {
        if(signal.element1.nextPosition == null && 
           signal.element2.nextPosition == null)
        {
            swipeHistory.Add(lastSwipe);
            isVerifyed = true;
        }
    }

    private List<int> newGemsType;

    private void newGems(NewGemsSignal signal)
    {
        newGemsType.AddRange(signal.newGemsType);
    }

    private void generateJSON()
    {
        BoardHistory json = new BoardHistory();
        json.board = boardStartState;
        json.swipeHistory = swipeHistory;
        json.newGemsType = newGemsType;
    }



    void Start()
    {
        swipeHistory = new List<SwipeData>();
        newGemsType = new List<int>();
    }
}
