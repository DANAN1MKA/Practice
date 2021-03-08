using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour, ITimerProgressBar
{
    public BoardConfig config;

    private Slider slider;
    private float configTime;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        configTime = config.time;
    }

    public void updateProgress(float newProgress)
    {
        slider.value = 1 - (newProgress / configTime);
    }
}