using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Graphics.OpenGL.Model;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL;

public class ModelLoader
{
    public KModel LoadModel(GL gl, byte[] data, byte[] mdxData)
    {
        var stream = new MemoryStream(data);
        var reader = new MDLBinaryReader(stream);

        var mdxStream = new MemoryStream(mdxData);
        var mdxReader = new BinaryReader(mdxStream);

        reader.SetStreamPosition(0);
        var modelHeader = new MDLBinaryModelHeader(reader);

        return new KModel()
        {
            Root = LoadNode(gl, reader, mdxReader, modelHeader.OffsetToRootNode, null)
        };
    }

    private BaseNode LoadNode(GL gl, MDLBinaryReader reader, BinaryReader mdxReader, int nodeOffset, BaseNode? parent)
    {
        reader.SetStreamPosition(nodeOffset);
        var dummyHeader = new MDLBinaryNodeHeader(reader);
        BaseNode node;

        var position = new Vector3(dummyHeader.Position.X, dummyHeader.Position.Y, dummyHeader.Position.Z);
        var orientation = new Quaternion(dummyHeader.Rotation.Y, dummyHeader.Rotation.Z, dummyHeader.Rotation.W, dummyHeader.Rotation.X);

        if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.TrimeshFlag))
        {
            var trimeshHeader = new MDLBinaryTrimeshHeader(reader);

            reader.SetStreamPosition(trimeshHeader.OffsetToVertexIndicesOffsetArray);
            var offsetToIndices = reader.ReadUInt32();
            reader.SetStreamPosition((int)offsetToIndices);
            var indicesData = reader.ReadBytes(trimeshHeader.FaceArrayCount * 3 * 2);

            mdxReader.BaseStream.Position = trimeshHeader.MDXOffsetToData;
            var mdxData = mdxReader.ReadBytes(trimeshHeader.MDXDataSize * trimeshHeader.VertexCount); 

            var vao = new VertexArrayObjectFactory().FromBinary(
                gl,
                mdxData,
                indicesData,
                (uint)trimeshHeader.MDXPositionStride,
                (uint)trimeshHeader.MDXNormalStride,
                (uint)trimeshHeader.MDXTexture1Stride,
                (uint)trimeshHeader.MDXTexture2Stride,
                (uint)trimeshHeader.MDXDataSize,
                (uint)trimeshHeader.MDXDataBitmap);

            if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.DanglyFlag))
            {
                var danglyHeader = new MDLBinaryDanglyHeader(reader);
                node = new DanglymeshNode()
                {
                    Parent = parent,
                    Mesh = vao,
                    Texture1 = trimeshHeader.Texture,
                    Texture2 = trimeshHeader.Lightmap,
                    Position = position,
                    Orientation = orientation
                };
            }
            else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.SkinFlag))
            {
                var skinHeader = new MDLBinarySkinmeshHeader(reader);
                node = new SkinmeshNode()
                {
                    Parent = parent,
                    Mesh = vao,
                    Texture1 = trimeshHeader.Texture,
                    Texture2 = trimeshHeader.Lightmap,
                    Position = position,
                    Orientation = orientation
                };
            }
            else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.SaberFlag))
            {
                var saberHeader = new MDLBinarySabermeshHeader(reader);
                node = new SabermeshNode();
            }
            else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.AABBFlag))
            {
                var walkmeshHeader = new MDLBinaryWalkmeshHeader(reader);
                node = new WalkmeshNode()
                {
                    Parent = parent,
                    Mesh = vao,
                    Texture1 = trimeshHeader.Texture,
                    Texture2 = trimeshHeader.Lightmap,
                    Position = position,
                    Orientation = orientation
                };
            }
            else
            {
                node = new MeshNode()
                {
                    Parent = parent,
                    Mesh = vao,
                    Texture1 = trimeshHeader.Texture,
                    Texture2 = trimeshHeader.Lightmap,
                    Position = position,
                    Orientation = orientation
                };
            }
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.LightFlag))
        {
            node = new LightNode();
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.EmitterFlag))
        {
            node = new EmitterNode();
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.ReferenceFlag))
        {
            node = new ReferenceNode();
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.NodeFlag))
        {
            node = new DummyNode();
        }
        else
        {
            throw new Exception();
        }

        var childOffsets = new List<int>(dummyHeader.ChildArrayCount);
        reader.SetStreamPosition(dummyHeader.OffsetToChildOffsetArray);
        for (int i = 0; i < dummyHeader.ChildArrayCount; i++)
        {
            childOffsets.Add(reader.ReadInt32());
        }
        foreach (var childOffset in childOffsets)
        {
            node.Nodes.Add(LoadNode(gl, reader, mdxReader, childOffset, node));
        }

        return node;
    }
}
