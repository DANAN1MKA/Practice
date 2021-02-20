using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour, IMoveManager
{
    private List<MovingElements> movingElemenets;

    private bool addElementFlag = false;
    private MovingElements newElement;
    public void addElement(MovingElements _newElement)
    {
        addElementFlag = true;
        newElement = _newElement;
    }
    public void addElementHandler(MovingElements newElement)
    {
        if (movingElemenets.Contains(newElement))
        {
            foreach (MovingElements elem in movingElemenets)
            {
                if (elem.Equals(newElement))
                {
                    elem.endPosition = newElement.endPosition;
                    break;
                }
            }
        }
        else
        {
            movingElemenets.Add(newElement);
        }
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
    public void removeElementHandler(Element element)
    {
        foreach (MovingElements elem in movingElemenets)
        {
            if (elem.elem.Equals(element))
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
                movingElemenets[i].elem.MoveElementTo(movingElemenets[i].endPosition);
                //if reached target position - remove element
                if (movingElemenets[i].elem.piece.transform.position.x == movingElemenets[i].endPosition.x &&
                    movingElemenets[i].elem.piece.transform.position.y == movingElemenets[i].endPosition.y)
                    movingElemenets.Remove(movingElemenets[i]);
            }
        }
    }
    //Так сказать синхронизация
    private void LateUpdate()
    {
        if (addElementFlag)
        {
            addElementHandler(newElement);
            addElementFlag = false;
        }
        if (dropElementsFlag)
        {
            dropElementsHandler(newList);
            dropElementsFlag = false;
        }
        if (removeElementFlag)
        {
            removeElementHandler(element);
            removeElementFlag = false;
        }
    }
}
 
public interface IMoveManager 
{
    void dropElements(List<MovingElements> newList);

    void addElement(MovingElements newElement);

    void removeElement(Element element);
}

