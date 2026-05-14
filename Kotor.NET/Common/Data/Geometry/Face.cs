using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public interface IFace
{
    public Vector3 Point1 { get; }
    public Vector3 Point2 { get; }
    public Vector3 Point3 { get; }

    public SurfaceMaterial Material { get; set; }
    public float PlaneDistance { get; }
    public Vector3 Normal { get; }
    public Vector3 Center { get; }
}
