using UnityEngine;

public class MovingElement
{
    public Element elem { get; private set; }
    public Vector2 endPosition { get; private set; }

    public MovingElement nextPosition{ get; private set; }

    public MovingElement(Element _elem, Vector2 _endPosition, MovingElement next)
    {
        elem = _elem;
        endPosition = _endPosition;
        nextPosition = next;
    }
}
