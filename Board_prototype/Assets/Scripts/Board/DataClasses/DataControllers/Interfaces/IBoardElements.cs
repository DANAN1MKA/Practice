using UnityEngine;

public interface IBoardElements
{
    bool grabElement(int _x, int _y);

    void swipeElement(int _x, int _y, Vector2 _direction);
}
