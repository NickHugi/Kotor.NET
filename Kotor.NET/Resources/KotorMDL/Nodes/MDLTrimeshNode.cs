using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLTrimeshNode : MDLNode
{
    public MDLFaceCollection Faces { get; }

    public float TotalArea { get; set; }
    public float Radius { get; set; }
    public Vector3 AveragePoint { get; set; }
    public BoundingBox BoundingBox { get; set; }

    public string? DiffuseTexture { get; set; }
    public string? LightmapTexture { get; set; }

    public bool Renders { get; set; }
    public bool CastsShadow { get; set; }
    public bool Beaming { get; set; }
    public bool RotateTexture { get; set; }
    public bool IsBackgroundGeometry { get; set; }

    public Vector3 Diffuse { get; set; } = new();
    public Vector3 Ambient { get; set; } = new();
    public uint TransparencyHint { get; set; }

    public float UVSpeed { get; set; }
    public float UVJitterSpeed { get; set; }
    public Vector2 UVDirection { get; set; } = new();

    public int InvertedCounter = 0;

    // K2 Only
    public bool HideInHologram { get; set; }
    public bool DirtEnabled { get; set; }
    public short DirtTexture { get; set; }
    public short DirtCoordinateSpace { get; set; }

    private bool _positionsEnabled = false;
    private bool _normalsEnabled = false;
    private bool _diffuseUVsEnabled = false;
    private bool _lightmapUVsEnabled = false;
    private bool _tangentsEnabled = false;

    public MDLTrimeshNode(string name) : base(name)
    {
        Faces = new(this);
    }

    public bool HasPositions()
    {
        return _positionsEnabled;
    }
    public void EnableVertices()
    {
        if (_positionsEnabled)
            return;

        _positionsEnabled = true;
        AllVertices().ToList().ForEach(x => x._position = new());
    }
    public void DisableVertices()
    {
        if (!_positionsEnabled)
            return;

        _positionsEnabled = false;
        AllVertices().ToList().ForEach(x => x._position = null);
    }

    public bool HasNormals()
    {
        return _normalsEnabled;
    }
    public void EnableNormals()
    {
        if (_normalsEnabled)
            return;

        _normalsEnabled = true;
        AllVertices().ToList().ForEach(x => x._normal = new());
    }
    public void DisableNormals()
    {
        if (!_normalsEnabled)
            return;

        _normalsEnabled = false;
        AllVertices().ToList().ForEach(x => x._normal = null);
    }

    public bool HasDiffuseUVs()
    {
        return _diffuseUVsEnabled;
    }
    public void EnableDiffuseUVs()
    {
        if (_diffuseUVsEnabled)
            return;

        _diffuseUVsEnabled = true;
        AllVertices().ToList().ForEach(x => x._diffuseUV = new());
    }
    public void DisableDiffuseUVs()
    {
        if (!_diffuseUVsEnabled)
            return;

        _diffuseUVsEnabled = false;
        AllVertices().ToList().ForEach(x => x._diffuseUV = null);
    }

    public bool HasLightmapUVs()
    {
        return _lightmapUVsEnabled;
    }
    public void EnableLightmapUVs()
    {
        if (_lightmapUVsEnabled)
            return;

        _lightmapUVsEnabled = true;
        AllVertices().ToList().ForEach(x => x._lightmapUV = new());
    }
    public void DisableLightmapUVs()
    {
        if (!_lightmapUVsEnabled)
            return;

        _lightmapUVsEnabled = false;
        AllVertices().ToList().ForEach(x => x._lightmapUV = null);
    }

    public bool HasTangents()
    {
        return _tangentsEnabled;
    }
    public void EnableTangents()
    {
        if (_tangentsEnabled)
            return;

        _tangentsEnabled = true;
        AllVertices().ToList().ForEach(x =>
        {
            x._tangent1 = new();
            x._tangent2 = new();
            x._tangent3 = new();
            x._tangent4 = new();
        });
    }
    public void DisableTangents()
    {
        if (!_tangentsEnabled)
            return;

        _tangentsEnabled = false;
        AllVertices().ToList().ForEach(x =>
        {
            x._tangent1 = new();
            x._tangent2 = new();
            x._tangent3 = new();
            x._tangent4 = new();
        });
    }

    public IEnumerable<MDLVertex> AllVertices()
    {
        var vertices = Faces.SelectMany(x => new[] { x.Vertex1, x.Vertex2, x.Vertex3 });
        var x = new List<MDLVertex>();
        foreach (var vertex in vertices)
        {
            if (!x.Contains(vertex))
                x.Add(vertex);
        }
        return x;
    }

    public override string ToString()
    {
        return $"MDLTrimeshNode '{Name}'";
    }
}
