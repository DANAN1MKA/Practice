
public class AddScoreSignal
{
    public System.UInt64 score { get; private set; }
    public System.UInt64 money { get; private set; }

    public AddScoreSignal(System.UInt64 _score, System.UInt64 _money)
    {
        score = _score;
        money = _money;
    }
}
