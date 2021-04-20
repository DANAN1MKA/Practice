using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemsMoneyUI : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    [Inject] PlayerData playerData;

    private Text text;

    private void Awake()
    {
        signalBus.Subscribe<UpdateTextUISignal>(updateUI);
        text = GetComponent<Text>();
    }

    void Start()
    {
        text.text = "Валежник:" + playerData.money;
    }

    private void updateUI()
    {
        text.text = "Валежник:" + playerData.money;
    }
}
