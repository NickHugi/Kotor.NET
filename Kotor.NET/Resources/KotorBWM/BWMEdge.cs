using System.Numerics;
using Kotor.NET.Extensions;

namespace Kotor.NET.Resources.KotorBWM;

public class BWMEdge
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

    public BWMFace Face => _face;

    public BWMFace? AdjacentFace => _face._collection.FindAdjacentFace(this);
    public BWMEdge? AdjacentEdge
    {
        get
        {
            var adjacentFace = AdjacentFace;
            if (adjacentFace is null)
                return null;

            var adjacentEdge = adjacentFace.Edge1;
            if (Equals(adjacentFace.Edge1))
                return adjacentEdge;

            adjacentEdge = adjacentFace.Edge2;
            if (Equals(adjacentEdge))
                return adjacentEdge;

            adjacentEdge = adjacentFace.Edge3;
            if (Equals(adjacentEdge))
                return adjacentEdge;

            return null;
        }
    }

    internal readonly BWMFace _face;
    internal readonly int _index;

    internal BWMEdge(BWMFace face, int index)
    {
        _face = face;
        _index = index;
    }

    public override bool Equals(object? obj)
    {
        return obj is BWMEdge edge ? Equals(edge) : false;
    }
    public bool Equals(BWMEdge other)
    {
        var match1 = Point1.ApproximatelyEquals(other.Point1) || Point1.ApproximatelyEquals(other.Point2);
        var match2 = Point2.ApproximatelyEquals(other.Point1) || Point2.ApproximatelyEquals(other.Point2);
        return match1 && match2;
    }
}
