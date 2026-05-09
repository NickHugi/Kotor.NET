using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Common.Data.Geometry;

public class FaceCollection : IEnumerable<Face>
{
    private readonly List<Face> _faces = new();
    private List<Face> _sortedFaces => _faces.OrderByDescending(x => x.Material.IsWalkable() ? 1 : 0).ToList();

    public int Count => _faces.Count;
    public bool IsReadOnly => false;

    public Face this[int index]
    {
        get => _sortedFaces[index];
    }

    public void Add(Vector3 point1, Vector3 point2, Vector3 point3, SurfaceMaterial material) => _faces.Add(new(this) { Point1=point1, Point2=point2, Point3=point3, Material=material });
    public void Clear() => _faces.Clear();
    public bool Contains(Face item) => _faces.Contains(item);
    public IEnumerator<Face> GetEnumerator() => _sortedFaces.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _sortedFaces.GetEnumerator();

    public int IndexOf(Face face)
    {
        return _sortedFaces.IndexOf(face);
    }

    public Face? FindAdjacentFace(Face face)
    {
        foreach (var checking in _faces)
        {
            if (face == checking)
                continue;

            var matching1 = false;
            var matching2 = false;

            if (face.Point1.ApproximatelyEquals(checking.Point1))
                matching1 = true;
            if (face.Point1.ApproximatelyEquals(checking.Point2))
                matching1 = true;
            if (face.Point1.ApproximatelyEquals(checking.Point3))
                matching1 = true;

            if (face.Point2.ApproximatelyEquals(checking.Point1))
                matching2 = true;
            if (face.Point2.ApproximatelyEquals(checking.Point2))
                matching2 = true;
            if (face.Point2.ApproximatelyEquals(checking.Point3))
                matching2 = true;

            if (matching1 && matching2)
                return checking;
        }

        return null;
    }
    public Face? FindAdjacentFace(Edge edge)
    {
        foreach (var checking in _faces)
        {
            if (edge._face == checking)
                continue;
            if (edge.Point1.Equals(edge.Point2))
                break;

            var matching1 = false;
            var matching2 = false;

            if (edge.Point1.ApproximatelyEquals(checking.Point1))
                matching1 = true;
            if (edge.Point1.ApproximatelyEquals(checking.Point2))
                matching1 = true;
            if (edge.Point1.ApproximatelyEquals(checking.Point3))
                matching1 = true;

            if (edge.Point2.ApproximatelyEquals(checking.Point1))
                matching2 = true;
            if (edge.Point2.ApproximatelyEquals(checking.Point2))
                matching2 = true;
            if (edge.Point2.ApproximatelyEquals(checking.Point3))
                matching2 = true;

            if (matching1 && matching2)
                return checking;
        }

        return null;
    }
}
