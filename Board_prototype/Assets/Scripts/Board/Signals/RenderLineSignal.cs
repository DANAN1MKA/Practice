using UnityEngine;

public class RenderLineSignal
{

    public Vector3[] points { get; private set; }

    public RenderLineSignal nextLine { get; private set; }

    public RenderLineSignal(Vector3[] _points, RenderLineSignal _nextLine)
    {
        points = _points;
        nextLine = _nextLine;
    }
}
