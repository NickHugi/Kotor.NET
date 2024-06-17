using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Geometry;
using Kotor.NET.Formats.KotorBWM;
using Kotor.NET.Formats.KotorMDL;
using Kotor.NET.Graphics.SceneObjects;
using MapBuilder.Render;

namespace MapBuilder.Data;

public class TerrainData : Placement
{
    public uint Width => _width;
    public uint Length => _length;
    public float[,] Height => _height;

    private uint _width;
    private uint _length;
    private float[,] _height;

    public TerrainData(uint width, uint length)
    {
        _width = width;
        _length = length;
        _height = new float[width, length];

        AssignRandomElevation();
    }

    public void AssignRandomElevation()
        {
            var height = new float[Width, Length];
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Length; y++)
            {
            _height[x, y] = (float)new Random().NextDouble() / 2;
        }
    }

    public override int GetHashCode()
    {
        int hash = 0;
        
        hash ^= _width.GetHashCode();
        hash ^= _height.GetHashCode();

        foreach (var item in _height)
    {
            hash ^= item.GetHashCode();
        }

        return hash;
    }

    public override BWM GenerateWalkmesh() => throw new NotImplementedException();

    public override MDL GenerateModel()
    {
        var mdl = new MDL();
        mdl.AnimationScale = 1;
        mdl.ModelName = "test";
        mdl.Supermodel = "NULL";

        var node = mdl.Root = new Node();
        node.Name = "test";

        var child = new Node();
        node.Children.Add(child);

        child.Name = "terrain";
        child.Controllers = new()
        {
            new Controller()
            {
                ControllerType = 8,
                Rows = new() { new(0.0f, BitConverter.GetBytes(0.0f), BitConverter.GetBytes(0.0f), BitConverter.GetBytes(0.0f)), }
            },
            new Controller()
            {
                ControllerType = 20,
                Rows = new() { new(0.0f, BitConverter.GetBytes(0.0f), BitConverter.GetBytes(0.0f), BitConverter.GetBytes(0.0f), BitConverter.GetBytes(1.0f)), }
            },
        };
        child.Rotation = new() { W = 1 };
        child.Trimesh = new()
        {
            DiffuseTexture = "lda_grass01.tga",
            Faces = Enumerable.Range(0, (int)_width).SelectMany(x =>
            {
                return Enumerable.Range(0, (int)_length).Select(y =>
                {
                    return new Face()
                    {
                        Vertex1 = new Vertex()
                        {
                            Position = new Vector3(x, y, _height[x, y]),
                            Normal = new Vector3(0, 0, 1),
                            DiffuseUV = new Vector2(x, y),
                        }
                    };
                });
            }).ToList(),
        };

        return mdl;
    }
}
