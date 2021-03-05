using UnityEngine;

public class MovingElement
{
    public Element elem;
    public Vector2 endPosition;

    public MovingElement(Element _elem, Vector2 _endPosition)
    {
        elem = _elem;
        endPosition = _endPosition;
    }
}
