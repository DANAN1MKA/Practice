using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MoneyTextUI : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [Inject] PlayerData playerData;

    private Text text;
    int currentMoney;

    private void Awake()
    {
        text = GetComponent<Text>();
        signalBus.Subscribe<AddScoreSignal>(addScore);
    }

    void Start()
    {
        currentMoney = playerData.money;
        text.text = "Валежник:" + currentMoney;
    }

    private void addScore(AddScoreSignal signal)
    {
        currentMoney += signal.money;
        text.text = "Валежник:" + currentMoney;
    }
}
