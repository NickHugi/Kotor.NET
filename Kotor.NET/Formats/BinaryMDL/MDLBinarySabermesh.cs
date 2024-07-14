using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinarySabermesh
{
    public List<Vector3> Vertices { get; set; } = new();
    public List<Vector3> Normals { get; set; } = new();
    public List<Vector2> UVs { get; set; } = new();

    public MDLBinarySabermesh()
    {
    }
    public MDLBinarySabermesh(MDLBinaryTrimeshHeader trimeshHeader, MDLBinarySabermeshHeader sabermeshHeader, MDLBinaryReader reader)
    {
        reader.SetStreamPosition(sabermeshHeader.OffsetToVertexArray);
        for (int i = 0; i < trimeshHeader.VertexCount; i++)
        {
            var vertex = reader.ReadVector3();
            Vertices.Add(vertex);
        }

        reader.SetStreamPosition(sabermeshHeader.OffsetToNormalArray);
        for (int i = 0; i < trimeshHeader.VertexCount; i++)
        {
            var normal = reader.ReadVector3();
            Normals.Add(normal);
        }

        reader.SetStreamPosition(sabermeshHeader.OffsetToTexCoordArray);
        for (int i = 0; i < trimeshHeader.VertexCount; i++)
        {
            var uv = reader.ReadVector2();
            UVs.Add(uv);
        }
    }

    public void Write(MDLBinarySabermeshHeader sabermeshHeader, MDLBinaryWriter writer)
    {
        writer.SetStreamPosition(sabermeshHeader.OffsetToVertexArray);
        foreach (var vertex in Vertices)
        {
            writer.Write(vertex);
        }

        writer.SetStreamPosition(sabermeshHeader.OffsetToNormalArray);
        foreach (var normal in Normals)
        {
            writer.Write(normal);
        }

        writer.SetStreamPosition(sabermeshHeader.OffsetToTexCoordArray);
        foreach (var uv in UVs)
        {
            writer.Write(uv);
        }
    }
}
