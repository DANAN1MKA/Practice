public class NewReplaySignal
{
    public BoardHistory history { get; private set; }

    public NewReplaySignal(BoardHistory _history)
    {
        history = _history;
    }
}

