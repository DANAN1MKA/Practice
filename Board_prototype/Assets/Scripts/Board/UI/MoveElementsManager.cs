using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveElementsManager : MonoBehaviour, IMoveElementsManager
{
    [Inject] private SignalBus signalBus;

    private List<MovingElement> movingElemenets;

    public void addElement(MoveManagerAddSignal _data)
    {
        removeElement(_data.element1.elem);
        removeElement(_data.element2.elem);
        movingElemenets.Add(_data.element1);
        movingElemenets.Add(_data.element2);
    }

    public void dropElements(MoveManagerDropSignal newList)
    {
        movingElemenets.Clear();
        movingElemenets.AddRange(newList.board);
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
        signalBus.Subscribe<MoveManagerAddSignal>(addElement);
        signalBus.Subscribe<MoveManagerDropSignal>(dropElements);

        movingElemenets = new List<MovingElement>();
    }

    void Update()
    {
        if (movingElemenets.Count > 0)
        {
            for (int i = 0; i < movingElemenets.Count; i++)
            {
                movingElemenets[i].elem.moveHard(movingElemenets[i].endPosition);

                //if reached target position - remove element
                if (movingElemenets[i].elem.piece.transform.position.x == movingElemenets[i].endPosition.x &&
                    movingElemenets[i].elem.piece.transform.position.y == movingElemenets[i].endPosition.y)

                    if (movingElemenets[i].nextPosition != null) movingElemenets[i] = movingElemenets[i].nextPosition;
                    else
                    { 
                        movingElemenets.Remove(movingElemenets[i]);
                        i--;
                    }
            }
            if (movingElemenets.Count == 0) signalBus.Fire<AnimationCompletedSignal>();
        }
    }

}
