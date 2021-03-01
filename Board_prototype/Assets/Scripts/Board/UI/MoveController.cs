using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveController : MonoBehaviour, IMoveManager
{
    [Inject] IBoardUIEvents boardUIEvents;

    private List<MovingElements> movingElemenets;

    public void addElement(MovingElements _newElement)
    {
        movingElemenets.Add(_newElement);
    }

    public void addElement(MovingElements newElement1, MovingElements newElement2)
    {
        movingElemenets.Add(newElement1);
        movingElemenets.Add(newElement2);
    }


    private bool dropElementsFlag = false;
    private List<MovingElements> newList;
    public void dropElements(List<MovingElements> _newList)
    {
        dropElementsFlag = true;
        newList = _newList;
    }
    public void dropElementsHandler(List<MovingElements> newList)
    {
        movingElemenets.Clear();
        movingElemenets.AddRange(newList);
    }


    private bool removeElementFlag = false;
    private Element element;
    public void removeElement(Element _element)
    {
        removeElementFlag = true;
        element = _element;
    }

    public void removeElementHandler(Element _element)
    {
        foreach (MovingElements elem in movingElemenets)
        {
            if (elem.elem.Equals(_element))
            {
                movingElemenets.Remove(elem);

                break;
            }
        }
    }



    void Start()
    {
        movingElemenets = new List<MovingElements>();
    }

    void Update()
    {
        if (movingElemenets.Count > 0)
        {
            for (int i = 0; i < movingElemenets.Count; i++)
            {
                movingElemenets[i].elem.hardMoveElementTo(movingElemenets[i].endPosition);

                //if reached target position - remove element
                if (movingElemenets[i].elem.piece.transform.position.x == movingElemenets[i].endPosition.x &&
                    movingElemenets[i].elem.piece.transform.position.y == movingElemenets[i].endPosition.y)
                    movingElemenets.Remove(movingElemenets[i]);
            }
            if(movingElemenets.Count == 0) boardUIEvents.dropIsOver();
        }
    }
    //Так сказать синхронизация
    private void LateUpdate()
    {
        if (dropElementsFlag)
        {
            dropElementsHandler(newList);
            dropElementsFlag = false;
        }
    }

}
