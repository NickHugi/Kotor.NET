using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryTrimesh
{
    public List<MDLBinaryTrimeshFace> Faces { get; set; } = new();
    public List<int> VertexIndicesOffsets { get; set; } = new();
    public List<int> VertexIndiciesCounts { get; set; } = new();
    public List<int> InvertedCounters { get; set; } = new();
    public List<List<ushort>> VertexIndices {  get; set; } = new();
    public List<Vector3> Vertices {  get; set; } = new();

    public MDLBinaryTrimesh()
    {
    }
    public MDLBinaryTrimesh(MDLBinaryTrimeshHeader trimeshHeader, MDLBinaryReader reader)
    {
        reader.SetStreamPosition(trimeshHeader.OffsetToFaceArray);
        for (int i = 0; i < trimeshHeader.FaceArrayCount; i++)
        {
            var face = new MDLBinaryTrimeshFace(reader);
            Faces.Add(face);
        }

        reader.SetStreamPosition(trimeshHeader.OffsetToVertexIndicesCountArray);
        for (int i = 0; i < trimeshHeader.VertexIndicesCountArrayCount; i++)
        {
            var index = reader.ReadInt32();
            VertexIndiciesCounts.Add(index);
        }

        reader.SetStreamPosition(trimeshHeader.OffsetToVertexIndicesOffsetArray);
        for (int i = 0; i < trimeshHeader.VertexIndicesOffsetArrayCount; i++)
        {
            var offset = reader.ReadInt32();
            VertexIndicesOffsets.Add(offset);
        }

        reader.SetStreamPosition(trimeshHeader.OffsetToInvertedCounterArray);
        for (int i = 0; i < trimeshHeader.InvertedCounterArrayCount; i++)
        {
            var counter = reader.ReadInt32();
            InvertedCounters.Add(counter);
        }

        for (int i = 0; i < trimeshHeader.VertexIndicesCountArrayCount; i++)
        {
            var offset = VertexIndicesOffsets[i];
            var count = VertexIndiciesCounts[i];

            var indicies = new List<ushort>();
            VertexIndices.Add(indicies);

            reader.SetStreamPosition(offset);
            for (int j = 0; j < count; j++)
            {
                var index = reader.ReadUInt16();
                indicies.Add(index);
            }
        }

        reader.SetStreamPosition(trimeshHeader.OffsetToVertexArray);
        for (int i = 0; i < trimeshHeader.VertexCount; i++)
        {
            var vertex = reader.ReadVector3();
            Vertices.Add(vertex);
        }
    }

    public void Write(MDLBinaryTrimeshHeader trimeshHeader, MDLBinaryWriter writer)
    {
        writer.SetStreamPosition(trimeshHeader.OffsetToFaceArray);
        foreach (var face in Faces)
        {
            face.Write(writer);
        }

        writer.SetStreamPosition(trimeshHeader.OffsetToVertexIndicesCountArray);
        foreach (var indicesCount in VertexIndiciesCounts)
        {
            writer.Write(indicesCount);
        }

        writer.SetStreamPosition(trimeshHeader.OffsetToVertexIndicesOffsetArray);
        foreach (var indicesOffset in VertexIndicesOffsets)
        {
            writer.Write(indicesOffset);
        }

        writer.SetStreamPosition(trimeshHeader.OffsetToInvertedCounterArray);
        foreach (var invertedCounter in InvertedCounters)
        {
            writer.Write(invertedCounter);
        }

        for (int i = 0; i < trimeshHeader.VertexIndicesCountArrayCount; i++)
        {
            var offset = VertexIndicesOffsets[i];
            writer.SetStreamPosition(offset);
            foreach (var index in VertexIndices[i])
            {
                writer.Write(index);
            }
        }

        writer.SetStreamPosition(trimeshHeader.OffsetToVertexArray);
        for (int i = 0; i < trimeshHeader.VertexCount; i++)
        {
            writer.Write(Vertices[i]);
        }
    }
}
