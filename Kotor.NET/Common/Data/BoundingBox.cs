using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data;

public class BoundingBox
{
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }

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
}
