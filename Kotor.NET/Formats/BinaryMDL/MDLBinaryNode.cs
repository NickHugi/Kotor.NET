using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryNode
{
    public MDLBinaryNodeHeader NodeHeader { get; set; } = new();
    public MDLBinaryLightHeader? LightHeader { get; set; }
    public MDLBinaryEmitterHeader? EmitterHeader { get; set; }
    public MDLBinaryReferenceHeader? ReferenceHeader { get; set; }
    public MDLBinaryTrimeshHeader? TrimeshHeader { get; set; }
    public MDLBinarySkinmeshHeader? SkinmeshHeader { get; set; }
    public MDLBinaryDanglyHeader? DanglymeshHeader { get; set; }
    public MDLBinaryWalkmeshHeader? WalkmeshHeader { get; set; }
    public MDLBinarySabermeshHeader? SabermeshHeader { get; set; }

    public List<int> ChildrenOffsets { get; set; } = new();
    public List<MDLBinaryNode> Children { get; set; } = new();
    public List<MDLBinaryControllerHeader> ControllerHeaders { get; set; } = new();
    public List<byte[]> ControllerData { get; set; } = new();

    public MDLBinaryLight? Light { get; set; }
    public MDLBinaryTrimesh? Trimesh { get; set; }
    public MDLBinarySkinmesh? Skinmesh { get; set; }
    public MDLBinaryDanglymesh? Danglymesh { get; set; }
    public MDLBinarySabermesh? Sabermesh { get; set; }
    public MDLBinaryAABBNode? RootAABBNode { get; set; }

    public List<MDXBinaryVertex> MDXVertices { get; set; } = new();

    public MDLBinaryNode()
    {
    }
    public MDLBinaryNode(MDLBinaryReader reader, BinaryReader mdxReader)
    {
        NodeHeader = new(reader);

        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.LightFlag) > 0)
        {
            LightHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.EmitterFlag) > 0)
        {
            EmitterHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.ReferenceFlag) > 0)
        {
            ReferenceHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.TrimeshFlag) > 0)
        {
            TrimeshHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.SkinFlag) > 0)
        {
            SkinmeshHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.DanglyFlag) > 0)
        {
            DanglymeshHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.AABBFlag) > 0)
        {
            WalkmeshHeader = new(reader);
        }
        if ((NodeHeader.NodeType & (ushort)MDLBinaryNodeType.SaberFlag) > 0)
        {
            SabermeshHeader = new(reader);
        }

        if (LightHeader is not null)
        {
            Light = new(LightHeader, reader);
        }
        if (EmitterHeader is not null)
        {
            // No extra data.
        }
        if (ReferenceHeader is not null)
        {
            // No extra data.
        }
        if (TrimeshHeader is not null)
        {
            Trimesh = new(TrimeshHeader, reader);
        }
        if (SkinmeshHeader is not null)
        {
            Skinmesh = new(SkinmeshHeader, reader);
        }
        if (DanglymeshHeader is not null)
        {
            Danglymesh = new(DanglymeshHeader, reader);
        }
        if (WalkmeshHeader is not null)
        {
            reader.SetStreamPosition(WalkmeshHeader.OffsetToRootAABBNode);
            RootAABBNode = new(reader);
        }
        if (SabermeshHeader is not null && TrimeshHeader is not null)
        {
            Sabermesh = new(TrimeshHeader, SabermeshHeader, reader);
        }

        reader.SetStreamPosition(NodeHeader.OffsetToChildOffsetArray);
        for (int i = 0; i < NodeHeader.ChildArrayCount; i++)
        {
            var offset = reader.ReadInt32();
            ChildrenOffsets.Add(offset);
        }
        foreach (var childOffset in ChildrenOffsets)
        {
            reader.SetStreamPosition(childOffset);
            var child = new MDLBinaryNode(reader, mdxReader);
            Children.Add(child);
        }

        reader.SetStreamPosition(NodeHeader.OffsetToControllerArray);
        for (int i = 0; i < NodeHeader.ControllerArrayCount; i++)
        {
            var controllerHeader = new MDLBinaryControllerHeader(reader);
            ControllerHeaders.Add(controllerHeader);
        }

        reader.SetStreamPosition(NodeHeader.OffsetToControllerData);
        for (int i = 0; i < NodeHeader.ControllerDataCount; i++)
        {
            ControllerData.Add(reader.ReadBytes(4));
        }

        if (TrimeshHeader is not null)
        {
            for (int i = 0; i < TrimeshHeader.VertexCount+1; i++)
            {
                MDXVertices.Add(new());

                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Vertices) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXPositionStride;
                    MDXVertices[i].Position = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Normals) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXNormalStride;
                    MDXVertices[i].Normal = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV1) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture1Stride;
                    MDXVertices[i].UV1 = mdxReader.ReadVector2();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV2) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture2Stride;
                    MDXVertices[i].UV2 = mdxReader.ReadVector2();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV3) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture3Stride;
                    MDXVertices[i].UV3 = mdxReader.ReadVector2();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV4) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture4Stride;
                    MDXVertices[i].UV4 = mdxReader.ReadVector2();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Colours) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXColourStride;
                    MDXVertices[i].Colour = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent1) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent1Stride;
                    MDXVertices[i].Tangent1 = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent2) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent2Stride;
                    MDXVertices[i].Tangent2 = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent3) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent3Stride;
                    MDXVertices[i].Tangent3 = mdxReader.ReadVector3();
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent4) != 0)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent4Stride;
                    MDXVertices[i].Tangent4 = mdxReader.ReadVector3();
                }

                if (SkinmeshHeader is not null)
                {
                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + SkinmeshHeader.MDXWeightValueStride;
                    MDXVertices[i].BoneWeight1 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneWeight2 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneWeight3 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneWeight4 = mdxReader.ReadSingle();

                    mdxReader.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + SkinmeshHeader.MDXWeightIndexStride;
                    MDXVertices[i].BoneIndex1 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneIndex2 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneIndex3 = mdxReader.ReadSingle();
                    MDXVertices[i].BoneIndex4 = mdxReader.ReadSingle();
                }
            }
        }
    }

    public void Write(MDLBinaryWriter writer, BinaryWriter mdxWriter)
    {
        NodeHeader.Write(writer);
        if (LightHeader is not null)
        {
            LightHeader.Write(writer);
        }
        if (EmitterHeader is not null)
        {
            EmitterHeader.Write(writer);
        }
        if (ReferenceHeader is not null)
        {
            ReferenceHeader.Write(writer);
        }
        if (TrimeshHeader is not null)
        {
            TrimeshHeader.Write(writer);
        }
        if (SkinmeshHeader is not null)
        {
            SkinmeshHeader.Write(writer);
        }
        if (DanglymeshHeader is not null)
        {
            DanglymeshHeader.Write(writer);
        }
        if (WalkmeshHeader is not null)
        {
            WalkmeshHeader.Writer(writer);
        }
        if (SabermeshHeader is not null)
        {
            SabermeshHeader.Write(writer);
        }

        if (LightHeader is not null && Light is not null)
        {
            Light.Write(LightHeader, writer);
        }
        if (EmitterHeader is not null)
        {
            // No extra data.
        }
        if (ReferenceHeader is not null)
        {
            // No extra data
        }
        if (TrimeshHeader is not null && Trimesh is not null)
        {
            Trimesh.Write(TrimeshHeader, writer);
        }
        if (SkinmeshHeader is not null && Skinmesh is not null)
        {
            Skinmesh.Write(SkinmeshHeader, writer);
        }
        if (DanglymeshHeader is not null && Danglymesh is not null)
        {
            Danglymesh.Write(DanglymeshHeader, writer);
        }
        if (WalkmeshHeader is not null && RootAABBNode is not null)
        {
            writer.SetStreamPosition(WalkmeshHeader.OffsetToRootAABBNode);
            RootAABBNode.Write(writer);
        }
        if (SabermeshHeader is not null && Sabermesh is not null)
        {
            Sabermesh.Write(SabermeshHeader, writer);
        }

        writer.SetStreamPosition(NodeHeader.OffsetToChildOffsetArray);
        foreach (var offset in ChildrenOffsets)
        {
            writer.Write(offset);
        }
        for (int i = 0; i < ChildrenOffsets.Count(); i++)
        {
            writer.SetStreamPosition(ChildrenOffsets[i]);
            Children[i].Write(writer, mdxWriter);
        }

        writer.SetStreamPosition(NodeHeader.OffsetToControllerArray);
        foreach (var controllerHeader in ControllerHeaders)
        {
            controllerHeader.Write(writer);
        }

        writer.SetStreamPosition(NodeHeader.OffsetToControllerData);
        foreach (var controllerDataBlock in ControllerData)
        {
            writer.Write(controllerDataBlock);
        }

        if (TrimeshHeader is not null)
        {
            for (int i = 0; i < MDXVertices.Count; i++)
            {
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Vertices) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXPositionStride;
                    mdxWriter.Write(MDXVertices[i].Position!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Normals) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXNormalStride;
                    mdxWriter.Write(MDXVertices[i].Normal!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV1) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture1Stride;
                    mdxWriter.Write(MDXVertices[i].UV1!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV2) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture2Stride;
                    mdxWriter.Write(MDXVertices[i].UV2!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV3) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture3Stride;
                    mdxWriter.Write(MDXVertices[i].UV3!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.UV4) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTexture4Stride;
                    mdxWriter.Write(MDXVertices[i].UV4!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Colours) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXColourStride;
                    mdxWriter.Write(MDXVertices[i].Colour!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent1) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent1Stride;
                    mdxWriter.Write(MDXVertices[i].Tangent1!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent2) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent2Stride;
                    mdxWriter.Write(MDXVertices[i].Tangent2!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent3) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent3Stride;
                    mdxWriter.Write(MDXVertices[i].Tangent3!);
                }
                if ((TrimeshHeader.MDXDataBitmap & (uint)MDLBinaryMDXVertexBitmask.Tangent4) != 0)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + TrimeshHeader.MDXTangent4Stride;
                    mdxWriter.Write(MDXVertices[i].Tangent4!);
                }

                if (SkinmeshHeader is not null)
                {
                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + SkinmeshHeader.MDXWeightValueStride;
                    mdxWriter.Write((float)MDXVertices[i].BoneWeight1!);
                    mdxWriter.Write((float)MDXVertices[i].BoneWeight2!);
                    mdxWriter.Write((float)MDXVertices[i].BoneWeight3!);
                    mdxWriter.Write((float)MDXVertices[i].BoneWeight4!);

                    mdxWriter.BaseStream.Position = TrimeshHeader.MDXOffsetToData + TrimeshHeader.MDXDataSize * i + SkinmeshHeader.MDXWeightIndexStride;
                    mdxWriter.Write((float)MDXVertices[i].BoneIndex1!);
                    mdxWriter.Write((float)MDXVertices[i].BoneIndex2!);
                    mdxWriter.Write((float)MDXVertices[i].BoneIndex3!);
                    mdxWriter.Write((float)MDXVertices[i].BoneIndex4!);
                }
            }

            writer.SetStreamPosition(TrimeshHeader.OffsetToFaceArray);
            foreach (var face in Trimesh.Faces)
            {
                face.Write(writer);
            }
        }
    }
}
