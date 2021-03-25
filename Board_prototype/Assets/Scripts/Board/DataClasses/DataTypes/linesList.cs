using UnityEngine;

public class LiensList
{
    public Vector3[] points { get; private set; }

    public int type { get; private set; }

    public LiensList nextLine { get; private set; }

    public LiensList(Vector3[] _points, int _type, LiensList _nextLine)
    {
        points = _points;
        type = _type;
        nextLine = _nextLine;
    }
}
