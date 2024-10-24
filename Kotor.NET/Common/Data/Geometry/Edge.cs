namespace Kotor.NET.Common.Data.Geometry;

public class Edge
{
    public const int NoTransition = -1;

    public Vector3 Point1 => _index switch
    {
        0 => _face.Point1,
        1 => _face.Point2,
        2 => _face.Point3,
    };
    public Vector3 Point2 => _index switch
    {
        0 => _face.Point2,
        1 => _face.Point3,
        2 => _face.Point1,
    };
    public int Transition
    {
        get => _face._transition[_index];
        set => _face._transition[_index] = value;
    }
    public Face? Adjacent => _face._collection.FindAdjacentFace(this);

    internal readonly Face _face;
    internal readonly int _index;

    internal Edge(Face face, int index)
    {
        _face = face;
        _index = index;
    }

    public override bool Equals(object? obj)
    {
        return (obj is Edge edge) ? Equals(edge) : false;
    }
    public bool Equals(Edge other)
    {
        var match1 = Point1.Equals(other.Point1, 0.01f) || Point1.Equals(other.Point2, 0.01f);
        var match2 = Point2.Equals(other.Point1, 0.01f) || Point2.Equals(other.Point2, 0.01f);
        return match1 && match2;
    }
}
