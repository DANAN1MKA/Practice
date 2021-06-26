
public class ReplayReachedServerSignal
{
    public bool isReached { get; private set; }

    public ReplayReachedServerSignal(bool _isReached)
    {
        isReached = _isReached;
    }
}
