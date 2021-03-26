using UnityEngine;

public class RenderLineSignal
{
    public LiensList list { get; private set; }

    public RenderLineSignal(LiensList _list)
    {
        list = _list;
    }
}
