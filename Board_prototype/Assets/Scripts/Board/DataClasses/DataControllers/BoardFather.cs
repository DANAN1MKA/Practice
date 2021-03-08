using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFather : MonoBehaviour, IBoardElements, IBoardTimerEvents, IBoardUIEvents
{
    [SerializeField] protected Transform _thisTransform;

    [SerializeField] protected BoardConfig config;

    public virtual void animationCompleted()
    {
        throw new System.NotImplementedException();
    }

    public virtual void timerHandler()
    {
        throw new System.NotImplementedException();
    }
    
    public virtual void grabElement(GrabElemetnSignal _grabElemetnSignal)
    {
        throw new System.NotImplementedException();
    }

    public virtual void swipeElement(SwipeElementSignal swipeElementSignal)
    {
        throw new System.NotImplementedException();
    }
}
