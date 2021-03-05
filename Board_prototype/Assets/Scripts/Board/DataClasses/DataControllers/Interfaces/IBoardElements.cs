using UnityEngine;

public interface IBoardElements
{
    Element getElementFromPoint(int x, int y);

    bool swipeElements(Element element1, Element element2);

    bool grabElement(int _x, int _y);

    void swipeElement(int _x, int _y, Vector2 _direction);
}
