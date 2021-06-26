using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClientUI : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    [SerializeField] private GameObject messageObj;
    [SerializeField] private Text message;
    [SerializeField] private GameObject shadow; 

    private string gameSearch = "Поиск игры";
    private string waitingOpponent = "Ждем соперника";
    private string waitingOpponentMove = "Ждем ход соперника";
    private string error = "Сервер не отвечает";

    private bool wait = false;

    private void Awake()
    {
        signalBus.Subscribe<IsGameReadySignal>(gameFound);
        //signalBus.Subscribe<ClientReplaySignal>(clientReplay);
        signalBus.Subscribe<SwipeDamageSignal>(waitingOpponetMove);
        signalBus.Subscribe<ServerReplaySignal>(gotReplay);
        signalBus.Subscribe<ServerNotRespondingSignal>(ServerNotResponding);
    }

    private void gameFound(IsGameReadySignal signal)
    {
        if (signal.status)
        {
            message.text = waitingOpponentMove;


            messageObj.SetActive(false);
            shadow.SetActive(false);
        }
        else
        {
            message.text = waitingOpponent;
        }
    }

    private void waitingOpponetMove()
    {
        message.text = waitingOpponentMove;
        messageObj.SetActive(true);
    }

    private void gotReplay()
    {
        message.text = waitingOpponentMove;

        messageObj.SetActive(false);
    }

    private void ServerNotResponding()
    {
        message.text = error;
        messageObj.SetActive(true);
    }
}
