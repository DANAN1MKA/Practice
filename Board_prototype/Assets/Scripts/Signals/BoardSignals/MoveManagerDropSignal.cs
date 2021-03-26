using System.Collections.Generic;

public class MoveManagerDropSignal
{
    public List<MovingElement> board { get; private set; }

    public MoveManagerDropSignal(List<MovingElement> _board)
    {
        board = _board;
    }

}
