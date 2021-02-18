using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Timer : ITickable, Itimer
{
    private bool isActive = false;
    private float time;

    [Inject] private IProgressBar progressUI;

    public void Tick()
    {       
        if (isActive)
        {
            if(time < Time.time)
            {               
                isActive = false;
                progressUI.dropProgress();
            }

            progressUI.updateProgress(time - Time.time);
        }
    }

    public void setTimer(float _time)
    {
        if (isActive)
        {
            time += _time;
        }
        else
        {
            isActive = true;
            time = _time + Time.time;
        }      
    }
}

public interface Itimer
{
    void setTimer(float _time);
}
