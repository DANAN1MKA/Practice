using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class ItemsScoreUI : MonoBehaviour
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
        text.text = "Очки:" + playerData.score;
    }

    private void updateUI()
    {
        text.text = "Очки:" + playerData.score;
    }
}
