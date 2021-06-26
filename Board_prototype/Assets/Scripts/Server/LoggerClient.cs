using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoggerClient : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    private bool isActive;

    private void Awake()
    {
        signalBus.Subscribe<StartBoardStateSignal>(startLogger);
        signalBus.Subscribe<SwipeElementSignal>(rememberSwipe);
        signalBus.Subscribe<MoveManagerSwipeSignal>(verifySwipe);
        signalBus.Subscribe<NewGemsSignal>(newGems);
        signalBus.Subscribe<SwipeDamageSignal>(generateJSON);
    }

    [Inject] BoardProperties config;

    private LiteElemet[] boardStartState;

    private void startLogger(StartBoardStateSignal signal)
    {
        boardStartState = new LiteElemet[signal.startState.Length];
        swipeHistory = new List<SwipeHistory>();
        newGemsType = new List<int>();

        int counter = 0;

        for (int i = 0; i < config.width; i++)
            for (int j = 0; j < config.height; j++)
            {
                LiteElemet newElement = new LiteElemet();
                newElement.posX = signal.startState[i, j].posX;
                newElement.posY = signal.startState[i, j].posY;
                newElement.type = signal.startState[i, j].type;
                boardStartState[counter] = newElement;
                counter++;
            }

        isActive = true;
    }

    private SwipeHistory lastSwipe;
    private bool isVerifyed = true;
    private List<SwipeHistory> swipeHistory;

    private void rememberSwipe(SwipeElementSignal signal)
    {
        //TODO: отладка
        Debug.Log("SwipeElementSignal");

        if (isActive)
        {
            lastSwipe = new SwipeHistory();
            lastSwipe.posX = signal.posX;
            lastSwipe.posY = signal.posY;
            lastSwipe.directionX = signal.direction.x;
            lastSwipe.directionY = signal.direction.y;
            isVerifyed = false;
        }
    }

    private void verifySwipe(MoveManagerSwipeSignal signal)
    {
        //TODO: отладка
        Debug.Log("MoveManagerSwipeSignal");


        if (isActive &&
            signal.element1.nextPosition == null &&
            signal.element2.nextPosition == null)
        {
            isVerifyed = true;

            saveSwipe();
        }
    }

    private void saveSwipe()
    {
        swipeHistory.Add(lastSwipe);

    }

    private List<int> newGemsType;

    private void newGems(NewGemsSignal signal)
    {
        newGemsType.AddRange(signal.newGemsType);
    }

    private void generateJSON(SwipeDamageSignal signal)
    {
        isActive = false;

        SetStepJSON json = new SetStepJSON(signal.damageAmount, 
                                            boardStartState,
                                            swipeHistory.ToArray(),
                                            newGemsType.ToArray());

        signalBus.Fire(new ClientReplaySignal(json));
    }



    void Start()
    {
        swipeHistory = new List<SwipeHistory>();
        newGemsType = new List<int>();
    }

}
