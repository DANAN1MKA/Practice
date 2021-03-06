using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardTimeController : ITickable, IBoardTimeController
{
    private bool isActive = false;
    private float time;

    [Inject] private ITimerProgressBar progressUI;
    [Inject] private IBoardTimerEvents board;

    public void setConfigProgresBar(float _time)
    {
        progressUI.setConfig(_time);
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

    public void Tick()
    {
        if (isActive)
        {
            if (time < Time.time)
            {
                isActive = false;
                board.timerHandler();
            }

            progressUI.updateProgress(time - Time.time);
        }
    }
}
