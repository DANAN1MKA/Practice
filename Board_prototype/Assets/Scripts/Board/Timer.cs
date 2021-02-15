using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Timer : ITickable, Itimer
{
    private bool isActive = false;
    private float time;

    public void Tick()
    {
        
        if (isActive)
        {
            if(time < Time.time)
            {
                Debug.Log("Timer alert!");
                isActive = false;
            }

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
            Debug.Log("Timer start!");
        }      
    }
}

public interface Itimer
{
    void setTimer(float _time);
}
