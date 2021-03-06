using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour, ITimerProgressBar
{
    private Slider slider;
    private float configTime;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void updateProgress(float newProgress)
    {
        slider.value = 1 - (newProgress / configTime);
    }

    public void setConfig(float _time)
    {
        configTime = _time;
    }
}