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

    public void dropProgress()
    {
        // TODO: Выяснить почему не работает
        slider.value = 0.1f;
    }

    public void setConfig(float _time)
    {
        configTime = _time;
    }
}