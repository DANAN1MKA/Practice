using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultyplayerManager : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private MultyplayerService multyplayerService;


    private int mode;                           // 1 - find new game
                                                // 2 - game found not ready
                                                // 3 - game found and ready waiting for player move
                                                // 4 - player made move waiting opponent move

    private float waitingTime = 60f;
    private float timeInterval = 2f;
    private float firstRequestTime;
    private float lastRequestTime;

    private void Awake()
    {
        signalBus.Subscribe<IsGameReadySignal>(gamefound);
        signalBus.Subscribe<ClientReplaySignal>(postNewReplay);
        signalBus.Subscribe<ReplayReachedServerSignal>(postReachedServer);
        signalBus.Subscribe<ServerReplaySignal>(getOpponentMove);
    }

    private void Start()
    {
        multyplayerService.findNewGame();
        firstRequestTime = Time.time + waitingTime;
        lastRequestTime = Time.time + timeInterval;
        mode = 1;
    }

    private void Update()
    {
        if(Time.time >= lastRequestTime)
        {
            lastRequestTime = Time.time + timeInterval;
            switch (mode)
            {
                case 1:
                    multyplayerService.findNewGame();

                    break;
                case 2:
                    multyplayerService.checkGameStatus();

                    break;
                case 4:
                    multyplayerService.getOpponentReplay();

                    break;
                default: break;
            }
            checkServerResponseDelay();
        }
    }

    private void gamefound(IsGameReadySignal signal)
    {
        if (signal.status)
        {
            mode = 3;
        }
        else
        {
            mode = 2;
        }
        firstRequestTime = Time.time + waitingTime;

        Debug.Log("Game mode-" + mode);
    }

    private void postNewReplay(ClientReplaySignal signal)
    {
        multyplayerService.postNewReplay(signal);

        lastRequestTime = Time.time + timeInterval;
    }
    private void postReachedServer(ReplayReachedServerSignal signal)
    {
        if (signal.isReached)
        {
            multyplayerService.getOpponentReplay();
            mode = 4;
        }
        else multyplayerService.postNewReplay(multyplayerService.lastPlayerReplay);

        lastRequestTime = Time.time + timeInterval;
    }

    private void getOpponentMove()
    {
        firstRequestTime = Time.time + waitingTime;
        mode = 3;
    }

    private void checkServerResponseDelay()
    {
        if(firstRequestTime <= Time.time && mode == 4)
        {
            signalBus.Fire<ServerNotRespondingSignal>();
            //TODO: действия если сервер не отвечает
        }
    }
}
