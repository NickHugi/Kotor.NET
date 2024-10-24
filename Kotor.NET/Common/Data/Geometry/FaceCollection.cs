using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public class FaceCollection : IEnumerable<Face>
{
    private readonly List<Face> _faces = new();

    public int Count => _faces.Count;
    public bool IsReadOnly => false;

    public Face this[int index]
    {
        get => _faces[index];
    }

    public void Add(Vector3 point1, Vector3 point2, Vector3 point3, SurfaceMaterial material) => _faces.Add(new(this) { Point1=point1, Point2=point2, Point3=point3, Material=material });
    public void Clear() => throw new NotImplementedException();
    public bool Contains(Face item) => throw new NotImplementedException();
    public IEnumerator<Face> GetEnumerator() => _faces.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _faces.GetEnumerator();

    public int IndexOf(Face face)
    {
        return _faces.IndexOf(face);
    }

    public Face? FindAdjacentFace(Face face)
    {
        foreach (var checking in _faces)
        {
            if (face == checking)
                continue;

            var matching1 = false;
            var matching2 = false;

            if (face.Point1.Equals(checking.Point1, 0.01f))
                matching1 = true;
            if (face.Point1.Equals(checking.Point2, 0.01f))
                matching1 = true;
            if (face.Point1.Equals(checking.Point3, 0.01f))
                matching1 = true;

            if (face.Point2.Equals(checking.Point1, 0.01f))
                matching2 = true;
            if (face.Point2.Equals(checking.Point2, 0.01f))
                matching2 = true;
            if (face.Point2.Equals(checking.Point3, 0.01f))
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

            if (edge.Point1.Equals(checking.Point1, 0.01f))
                matching1 = true;
            if (edge.Point1.Equals(checking.Point2, 0.01f))
                matching1 = true;
            if (edge.Point1.Equals(checking.Point3, 0.01f))
                matching1 = true;

            if (edge.Point2.Equals(checking.Point1, 0.01f))
                matching2 = true;
            if (edge.Point2.Equals(checking.Point2, 0.01f))
                matching2 = true;
            if (edge.Point2.Equals(checking.Point3, 0.01f))
                matching2 = true;

            if (matching1 && matching2)
                return checking;
        }

        return null;
    }
}
