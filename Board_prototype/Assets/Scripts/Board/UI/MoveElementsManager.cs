using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveElementsManager : MonoBehaviour, IMoveElementsManager
{
    [Inject] private SignalBus signalBus;

    private List<MovingElement> movingElemenets;

    public void addElement(MovingElement newElement1, MovingElement newElement2)
    {
        removeElement(newElement1.elem);
        removeElement(newElement2.elem);
        movingElemenets.Add(newElement1);
        movingElemenets.Add(newElement2);
    }

    public void dropElements(List<MovingElement> newList)
    {
        movingElemenets.Clear();
        movingElemenets.AddRange(newList);
    }

    private void removeElement(Element _element)
    {
        foreach (MovingElement elem in movingElemenets)
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
        movingElemenets = new List<MovingElement>();
    }

    void Update()
    {
        if (movingElemenets.Count > 0)
        {
            for (int i = 0; i < movingElemenets.Count; i++)
            {
                movingElemenets[i].elem.moveHard(movingElemenets[i].endPosition[0]);

                //if reached target position - remove element
                if (movingElemenets[i].elem.piece.transform.position.x == movingElemenets[i].endPosition[0].x &&
                    movingElemenets[i].elem.piece.transform.position.y == movingElemenets[i].endPosition[0].y)

                    if (movingElemenets[i].endPosition.Count > 1) movingElemenets[i].endPosition.RemoveAt(0);
                    else movingElemenets.Remove(movingElemenets[i]);
            }
            if (movingElemenets.Count == 0) signalBus.Fire<AnimationCompletedSignal>();
        }
    }

}
