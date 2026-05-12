using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public interface IFace
{
    public abstract Vector3 Point1 { get; }
    public abstract Vector3 Point2 { get; }
    public abstract Vector3 Point3 { get; }

    public abstract SurfaceMaterial Material { get; }
    public abstract Vector3 Normal { get; }
    public abstract float PlaneDistance { get; }
}
