using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLFace : IFace
{
    public MDLVertex Vertex1 { get; set; } = new();
    public MDLVertex Vertex2 { get; set; } = new();
    public MDLVertex Vertex3 { get; set; } = new();

    public Vector3 Point1 => Vertex1.Position.HasValue ? Vertex1.Position.Value : throw new InvalidOperationException();
    public Vector3 Point2 => Vertex2.Position.HasValue ? Vertex2.Position.Value : throw new InvalidOperationException();
    public Vector3 Point3 => Vertex3.Position.HasValue ? Vertex3.Position.Value : throw new InvalidOperationException();

    public SurfaceMaterial Material { get; set; }
    public Vector3 Normal { get; set; } = new();
    public Vector3 Center { get; set; } = new();
    public float PlaneDistance { get; set; }
}
