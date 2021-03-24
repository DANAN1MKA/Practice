
public class MoveManagerSwipeSignal
{
    public MovingElement element1 { get; private set; }
    public MovingElement element2 { get; private set; }

    public MoveManagerSwipeSignal(MovingElement _element1, MovingElement _element2)
    {
        element1 = _element1;
        element2 = _element2;
    }
}