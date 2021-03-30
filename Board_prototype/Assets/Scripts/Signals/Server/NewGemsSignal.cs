using System.Collections.Generic;

public class NewGemsSignal
{
    public List<int> newGemsType { get; private set; }

    public NewGemsSignal(List<int> _newGems)
    {
        newGemsType = _newGems;
    }
}
