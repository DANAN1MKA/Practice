using System;
using System.Collections.Generic;

[Serializable]
public class BoardHistory
{
    public LiteElemet[,] board;

    public List<SwipeData> swipeHistory;

    public List<int> newGemsType;
}
