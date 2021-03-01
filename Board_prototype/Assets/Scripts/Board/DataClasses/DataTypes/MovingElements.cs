using UnityEngine;

public class MovingElements
{
    public Element elem;
    public Vector2 endPosition;

    public MovingElements(Element _elem, Vector2 _endPosition)
    {
        elem = _elem;
        endPosition = _endPosition;
    }
}
