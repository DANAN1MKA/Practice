
public class StartBoardStateSignal
{
    public Element[,] startState { get; private set; }

    public StartBoardStateSignal(Element[,] board)
    {
        startState = board;
    }
}
