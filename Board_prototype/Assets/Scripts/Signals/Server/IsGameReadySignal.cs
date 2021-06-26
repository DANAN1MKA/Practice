using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGameReadySignal
{
    public bool status { get; private set; }
    public int heroID { get; private set; }

    public IsGameReadySignal(bool _status, int _heroId)
    {
        status = _status;
        heroID = _heroId;
    }
}
