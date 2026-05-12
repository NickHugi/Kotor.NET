using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Common.Data;

public class BoundingBox
{
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }

    public float XLength => Math.Abs(Max.X - Min.X);
    public float YLength => Math.Abs(Max.Y - Min.Y);
    public float ZLength => Math.Abs(Max.Z - Min.Z);

    public Vector3 Center
    {
        get
        {
            var x = (Min.X + Max.X) / 2;
            var y = (Min.Y + Max.Y) / 2;
            var z = (Min.Z + Max.Z) / 2;
            return new(x, y, z);
        }
    }

    public BoundingBox()
    {
        Min = new();
        Max = new();
    }
    public BoundingBox(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }
    public BoundingBox(IEnumerable<IFace> faces, float padding = 0)
    {
        List<Vector3> points =
        [
            ..faces.Select(x => x.Point1),
            ..faces.Select(x => x.Point2),
            ..faces.Select(x => x.Point3),
        ];

        Min = new Vector3
        {
            X = points.DefaultIfEmpty().Min(point => point.X) - padding,
            Y = points.DefaultIfEmpty().Min(point => point.Y) - padding,
            Z = points.DefaultIfEmpty().Min(point => point.Z) - padding,
        };
        Max = new Vector3
        {
            X = points.DefaultIfEmpty().Max(point => point.X) + padding,
            Y = points.DefaultIfEmpty().Max(point => point.Y) + padding,
            Z = points.DefaultIfEmpty().Max(point => point.Z) + padding,
        };
    }

    public Axis GetLongestAxis()
    {
        if (XLength > YLength && XLength > ZLength)
        {
            return Axis.X;
        }
        else if (YLength > ZLength)
        {
            return Axis.Y;
        }
        else
        {
            return Axis.Z;
        }
    }

    public Axis GetSecondLongestAxis()
    {
        if (XLength > YLength && XLength > ZLength)
        {
            return YLength > ZLength ? Axis.Y : Axis.Z;
        }
        else if (YLength > XLength && YLength > ZLength)
        {
            return XLength > ZLength ? Axis.X : Axis.Z;
        }
        else
        {
            return YLength > XLength ? Axis.Y : Axis.X;
        }
    }
}
