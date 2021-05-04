using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveElementsManager : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    private List<MovingElement> movingElemenets;

    public void addElement(MoveManagerSwipeSignal _data)
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

    private void Awake()
    {
        signalBus.Subscribe<MoveManagerSwipeSignal>(addElement);
        signalBus.Subscribe<MoveManagerDropSignal>(dropElements);
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

                if (!movingElemenets[i].elem.isMoving) 
                {
                    if (movingElemenets[i].nextPosition != null)
                    {
                        movingElemenets[i] = movingElemenets[i].nextPosition;
                        movingElemenets[i].elem.move(movingElemenets[i].endPosition);
                    }
                    else
                    {
                        movingElemenets.Remove(movingElemenets[i]);
                        i--;
                    }
                }
            }
            if (movingElemenets.Count == 0) signalBus.Fire<AnimationCompletedSignal>();
        }
    }
}
