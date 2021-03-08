using System.Collections.Generic;
using UnityEngine;

public class MovingElement
{
    public Element elem;
    public List<Vector2> endPosition;

    public MovingElement(Element _elem, List<Vector2> _endPosition)
    {
        elem = _elem;
        endPosition = _endPosition;
    }
}
