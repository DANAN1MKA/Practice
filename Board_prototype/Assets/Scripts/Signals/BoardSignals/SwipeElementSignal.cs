

using UnityEngine;

public class SwipeElementSignal
{
    public int posX { get; private set; }
    public int posY { get; private set; }

    public Vector2 direction { get; private set; }

    public SwipeElementSignal(int _posX, int _posY, Vector2 _direction)
    {
        posX = _posX;
        posY = _posY;
        direction = _direction;
    }
}
