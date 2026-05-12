using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model;

public class MeshDescriptor
{
    public object Tag { get; set; }
    public uint PickerID { get; set; } = 0xFFFFFFFF;

    public IMesh Mesh { get; init; }
    public BoundingBox? BoundingBox { get; init; }
    public BoundingSphere? BoundingSphere { get; init; }
    public string Texture1 { get; set; }
    public string Texture2 { get; set; }
    public bool DoShadow { get; set; }
    public bool DoRender { get; set; }
    public Matrix4x4 Transform { get; set; }
    public Matrix4x4[] BoneTransforms { get; set; }
    public Vector3 DiffuseColor { get; set; }
    public Vector3 AmbientColor { get; set; }
}


