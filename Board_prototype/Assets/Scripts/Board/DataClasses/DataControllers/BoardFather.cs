using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFather : MonoBehaviour, IBoardElements, IBoardTimerEvents, IBoardUIEvents
{
    [SerializeField] public int width;
    [SerializeField] public int heigth;

    [SerializeField] public Transform _thisTransform;

    protected bool isBlocked { get; set; }
    public bool getState() { return isBlocked; }

    public virtual void animationCompleted()
    {
        throw new System.NotImplementedException();
    }

    //TDOD: выпилить если одобрят
    public virtual Element getElementFromPoint(int x, int y)
    {
        throw new System.NotImplementedException();
    }


    //TDOD: выпилить если одобрят
    public virtual bool swipeElements(Element element1, Element element2)
    {
        throw new System.NotImplementedException();
    }

    public virtual void timerHandler()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool grabElement(int _x, int _y)
    {
        throw new System.NotImplementedException();
    }

    public virtual void swipeElement(int _x, int _y, Vector2 _direction)
    {
        throw new System.NotImplementedException();
    }
}
