using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics.GPU;
using Vector3 = System.Numerics.Vector3;

namespace Kotor.NET.Graphics.Model;

public class MeshDescriptor
{
    public IMesh Mesh { get; init; }
    public BoundingBox? BoundingBox { get; init; }
    public BoundingSphere? BoundingSphere { get; init; }
    public string Texture1 { get; init; }
    public string Texture2 { get; init; }
    public bool DoShadow { get; init; }
    public bool DoRender { get; init; }
    public Matrix4x4 Transform { get; init; }
    public Matrix4x4[] BoneTransforms { get; init; }
    public uint EntityID { get; init; }
}


