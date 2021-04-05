using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScoreTextUI : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [Inject] PlayerData playerData;

    private Text text;
    int currentScore;

    private void Awake()
    {
        text = GetComponent<Text>();
        signalBus.Subscribe<AddScoreSignal>(addScore);
    }

    void Start()
    {
        currentScore = playerData.score;
        text.text = "Score:" + currentScore;
    }

    private void addScore(AddScoreSignal signal)
    {
        currentScore += signal.score;
        text.text = "Score:" + currentScore;
    }

}
