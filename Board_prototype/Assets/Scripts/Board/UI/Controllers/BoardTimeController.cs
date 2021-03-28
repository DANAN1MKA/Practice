using UnityEngine;
using Zenject;

public class BoardTimeController : ITickable, IInitializable, ITimeController
{
    [Inject]private BoardProperties config;
    private bool isActive = false;
    private float time;
    private float timeScale;

    [Inject] private SignalBus signalBus;

    public delegate void UIDisplay(float _leftTime);
    private UIDisplay display;

    public void Initialize()
    {
        signalBus.Subscribe<SetTimerSignal>(setTimer);
    }

    public void setTimer(SetTimerSignal _time)
    {
        if (isActive)
        {
            if (time + _time.time - Time.time < config.time)
            {
                time += _time.time * timeScale;
                timeScale *= 0.7f;
            }
            else time = Time.time + config.time;
        }
        else
        {
            isActive = true;
            timeScale = 1;
            time = _time.time + Time.time;
        }
    }

    public void setUIDisplay(UIDisplay callback)
    {
        display = callback;
    }

    public void Tick()
    {
        if (isActive)
        {
            if (time < Time.time)
            {
                isActive = false;
                signalBus.Fire<TimerHandlerSignal>();
            }

            display(time - Time.time); 
        }
    }
}
