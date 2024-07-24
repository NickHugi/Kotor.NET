using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Resources.KotorMDL.VertexData;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLVertex
{
    public Vector3? Position => _position;
    public Vector3? Normal => _normal;
    public Vector2? DiffuseUV => _diffuseUV;
    public Vector2? LightmapUV => _lightmapUV;
    public Vector3? Tangent1 => _tangent1;
    public Vector3? Tangent2 => _tangent2;
    public Vector3? Tangent3 => _tangent3;
    public Vector3? Tangent4 => _tangent4;
    public MDLSaberVertexData? Saber => _saber;
    public MDLDanglyVertexData? Dangly => _dangly;
    public MDLSkinVertexData? Skin => _skin;

    internal Vector3? _position;
    internal Vector3? _normal;
    internal Vector2? _diffuseUV;
    internal Vector2? _lightmapUV;
    internal Vector3? _tangent1;
    internal Vector3? _tangent2;
    internal Vector3? _tangent3;
    internal Vector3? _tangent4;
    internal MDLSaberVertexData? _saber;
    internal MDLDanglyVertexData? _dangly;
    internal MDLSkinVertexData? _skin;

    public MDLVertex Clone()
    {
        return new MDLVertex()
        {
            _position = Position,
            _normal = Normal,
            _diffuseUV = DiffuseUV,
            _lightmapUV = LightmapUV,
            _tangent1 = Tangent1,
            _tangent2 = Tangent2,
            _tangent3 = Tangent3,
            _tangent4 = Tangent4,
            _saber = Saber,
            _dangly = Dangly,
            _skin = Skin,
        };
    }
}
