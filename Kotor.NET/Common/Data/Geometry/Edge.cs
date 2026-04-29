using System.Numerics;
using Kotor.NET.Extensions;

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
        var match1 = Point1.ApproximatelyEquals(other.Point1) || Point1.ApproximatelyEquals(other.Point2);
        var match2 = Point2.ApproximatelyEquals(other.Point1) || Point2.ApproximatelyEquals(other.Point2);
        return match1 && match2;
    }
}
