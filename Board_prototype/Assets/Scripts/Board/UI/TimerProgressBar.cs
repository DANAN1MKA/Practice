using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimerProgressBar : MonoBehaviour
{
    [Inject] ITimeController timer;

    [Inject]private BoardProperties config;

    private Slider slider;
    private float configTime;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        configTime = config.time;
    }

    private void Start()
    {
        timer.setUIDisplay(updateProgress);
    }

    public void updateProgress(float newProgress)
    {
        slider.value = 1 - (newProgress / configTime);
    }
}