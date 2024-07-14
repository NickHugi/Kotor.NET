using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDXBinaryVertex
{
    public Vector3? Position { get; set; }
    public Vector3? Normal { get; set; }
    public Vector2? UV1 { get; set; }
    public Vector2? UV2 { get; set; }
    public Vector2? UV3 { get; set; }
    public Vector2? UV4 { get; set; }
    public Vector3? Colour { get; set; }
    public Vector3? Tangent1 { get; set; }
    public Vector3? Tangent2 { get; set; }
    public Vector3? Tangent3 { get; set; }
    public Vector3? Tangent4 { get; set; }
    public float? BoneWeight1 { get; set; }
    public float? BoneWeight2 { get; set; }
    public float? BoneWeight3 { get; set; }
    public float? BoneWeight4 { get; set; }
    public float? BoneIndex1 { get; set; }
    public float? BoneIndex2 { get; set; }
    public float? BoneIndex3 { get; set; }
    public float? BoneIndex4 { get; set; }
}
