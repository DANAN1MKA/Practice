using UnityEngine;
using Zenject;

public class BoardTimeController : ITickable, IInitializable, IBoardTimeController
{
    private bool isActive = false;
    private float time;

    [Inject] private SignalBus signalBus;

    //TODO: удалить
    [Inject] private ITimerProgressBar progressUI;

    public void Initialize()
    {
        signalBus.Subscribe<SetTimerSignal>(setTimer);
    }

    public void setTimer(SetTimerSignal _time)
    {
        if (isActive)
        {
            time += _time.time;
        }
        else
        {
            isActive = true;
            time = _time.time + Time.time;
        }
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

            progressUI.updateProgress(time - Time.time);
        }
    }
}
