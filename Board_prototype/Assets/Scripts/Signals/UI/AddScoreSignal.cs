
public class AddScoreSignal
{
    public int score { get; private set; }
    public int money { get; private set; }

    public AddScoreSignal(int _score, int _money)
    {
        score = _score;
        money = _money;
    }
}
