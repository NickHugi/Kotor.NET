using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Common.Data;

public class BoundingBox
{
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }

    public float XLength => Math.Abs(Max.X - Min.X);
    public float YLength => Math.Abs(Max.Y - Min.Y);
    public float ZLength => Math.Abs(Max.Z - Min.Z);

    public Vector3 Centre
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
