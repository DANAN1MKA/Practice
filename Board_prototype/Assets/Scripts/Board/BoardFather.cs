using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFather : MonoBehaviour, IBoard, IBoardTimer, IBoardUIEvents
{
    [SerializeField] public int width;
    [SerializeField] public int heigth;

    [SerializeField] public Transform _thisTransform;
    public bool isBoardBlocked { get; set; }

    public virtual void dropIsOver()
    {
        throw new System.NotImplementedException();
    }

    public virtual Element getElementFromPoint(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool swipeElements(Element element1, Element element2)
    {
        throw new System.NotImplementedException();
    }

    public virtual void timerHandler()
    {
        throw new System.NotImplementedException();
    }
}

public interface IBoard
{
    Element getElementFromPoint(int x, int y);

    bool swipeElements(Element element1, Element element2);
}

public interface IBoardTimer
{
    void timerHandler();
}

public interface IBoardUIEvents
{
    void dropIsOver();
}