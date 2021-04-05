using UnityEngine;
using Zenject;

public class ScoreGenerator: MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;

    [SerializeField] GameObject scoreTextPrefab;
    [SerializeField] GameObject moneyTextPrefab;
    Vector2 newPositionScore;
    Vector2 newPositionMoney;

    private void Awake()
    {
        signalBus.Subscribe<AddScoreSignal>(generateScore);
    }

    private void Start()
    {
        newPositionScore = new Vector2(config.characterPosition.x * config.scale + 1,
                                          config.boardPositionFromResolution.y + (config.height + 2.6f) * config.scale);

        newPositionMoney = new Vector2(config.characterPosition.x * config.scale + 5,
                                          config.boardPositionFromResolution.y + (config.height + 2.6f) * config.scale);

        newPositionScore = RectTransformUtility.WorldToScreenPoint(GetComponentInParent<Camera>(), newPositionScore);

        newPositionMoney = RectTransformUtility.WorldToScreenPoint(GetComponentInParent<Camera>(), newPositionMoney);
    }

    private void generateScore(AddScoreSignal signal)
    {
        GameObject newTextScore =  Instantiate(scoreTextPrefab, this.transform);
        newTextScore.transform.position = newPositionScore;
        TextMover scriptScore = newTextScore.GetComponent<TextMover>();
        scriptScore.setup(signal.score);

        Destroy(newTextScore, 3f);


        GameObject newTextMoney = Instantiate(moneyTextPrefab, this.transform);
        newTextMoney.transform.position = newPositionMoney;
        TextMover scriptMoney = newTextMoney.GetComponent<TextMover>();
        scriptMoney.setup(signal.money);

        Destroy(newTextMoney, 3f);

    }

}
