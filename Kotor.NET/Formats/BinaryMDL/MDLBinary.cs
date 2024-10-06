using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Resources.KotorMDL.VertexData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinary
{
    public MDLBinaryFileHeader FileHeader { get; set; } = new();
    public MDLBinaryModelHeader ModelHeader { get; set; } = new();
    public List<int> NamesOffset { get; set; } = new();
    public List<string> Names { get; set; } = new();
    public List<int> AnimationOffsets { get; set; } = new();
    public List<MDLBinaryAnimation> Animations { get; set; } = new();
    public MDLBinaryNode RootNode { get; set; } = new();

    public bool IsTSL => false;

    public MDLBinary()
    {
    }
    public MDLBinary(Stream stream, Stream mdxStream)
    {
        var reader = new MDLBinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);

        FileHeader = new(reader);
        ModelHeader = new(reader);

        reader.SetStreamPosition(ModelHeader.OffsetToNameOffsetArray);
        for (int i = 0; i < ModelHeader.NamesArrayCount; i++)
        {
            var offset = reader.ReadInt32();
            NamesOffset.Add(offset);
        }

        foreach (var nameOffset in NamesOffset)
        {
            reader.SetStreamPosition(nameOffset);
            var name = reader.ReadTerminatedString('\0');
            Names.Add(name);
        }

        reader.SetStreamPosition(ModelHeader.AnimationOffsetArrayOffset);
        for (int i = 0; i < ModelHeader.AnimationCount; i++)
        {
            var offset = reader.ReadInt32();
            AnimationOffsets.Add(offset);
        }

        foreach (var offset in AnimationOffsets)
        {
            reader.SetStreamPosition(offset);
            var animation = new MDLBinaryAnimation(reader, mdxReader);
            Animations.Add(animation);
        }

        reader.SetStreamPosition(ModelHeader.OffsetToRootNode);
        RootNode = new(reader, mdxReader);
    }

    public void Write(Stream stream, Stream mdxStream)
    {
        var writer = new MDLBinaryWriter(stream);
        var mdxWriter = new BinaryWriter(mdxStream);

        FileHeader.Write(writer);
        ModelHeader.Write(writer);

        writer.SetStreamPosition(ModelHeader.OffsetToNameOffsetArray);
        foreach (var nameOffset in NamesOffset)
        {
            writer.Write(nameOffset);
        }
        for (int i = 0; i < Names.Count(); i++)
        {
            writer.SetStreamPosition(NamesOffset[i]);
            writer.Write(Names[i] + "\0", 0);
        }

        writer.SetStreamPosition(ModelHeader.AnimationOffsetArrayOffset);
        foreach (var animationOffset in AnimationOffsets)
        {
            writer.Write(animationOffset);
        }
        for (int i = 0; i < AnimationOffsets.Count(); i++)
        {
            writer.SetStreamPosition(AnimationOffsets[i]);
            Animations[i].Write(writer, mdxWriter);
        }

        writer.SetStreamPosition(ModelHeader.OffsetToRootNode);
        RootNode.Write(writer, mdxWriter);
    }

    public void Recalculate()
    {
        // TODO: function pointers?
        // TODO: node indexes - these might be linked to super models?

        ModelHeader.GeometryHeader.FunctionPointer1 = IsTSL ? MDLBinaryGeometryHeader.K2_NORMAL_FP1 : MDLBinaryGeometryHeader.K1_NORMAL_FP1;
        ModelHeader.GeometryHeader.FunctionPointer2 = IsTSL ? MDLBinaryGeometryHeader.K2_NORMAL_FP2 : MDLBinaryGeometryHeader.K1_NORMAL_FP2;
        ModelHeader.GeometryHeader.NodeCount = RecalculateNodeCount(RootNode);
        ModelHeader.GeometryHeader.GeometryType = 2;
        //ModelHeader.ChildModelCount = ModelHeader.GeometryHeader.NodeCount; // TODO - plus supermodel?
        ModelHeader.NamesArrayCount = Names.Count();
        ModelHeader.NamesArrayCount2 = Names.Count();
        ModelHeader.AnimationCount = Animations.Count();
        ModelHeader.AnimationCount2 = Animations.Count();

        var mdxOffset = 0;

        var offset = MDLBinaryModelHeader.SIZE;
        ModelHeader.OffsetToNameOffsetArray = offset;

        NamesOffset = new();
        offset += 4 * Names.Count;
        foreach (var name in Names)
        {
            NamesOffset.Add(offset);
            offset += name.Length + 1;
        }

        ModelHeader.AnimationOffsetArrayOffset = offset;

        offset += 4 * Animations.Count();
        AnimationOffsets = new();
        foreach (var animation in Animations)
        {
            AnimationOffsets.Add(offset);
            Recalculate(animation, ref offset, ref mdxOffset);
        }

        ModelHeader.OffsetToRootNode = offset;
        ModelHeader.GeometryHeader.RootNodeOffset = offset;
        Recalculate(RootNode, ref offset, ref mdxOffset, 0, 0);

        FileHeader.MDLSize = offset+12;
        FileHeader.MDXSize = mdxOffset;
        ModelHeader.MDXSize = mdxOffset;
    }
    private void Recalculate(MDLBinaryAnimation animation, ref int offset, ref int mdxOffset)
    {
        animation.AnimationHeader.EventCount = animation.Events.Count;
        animation.AnimationHeader.EventCount2 = animation.Events.Count;
        animation.AnimationHeader.GeometryHeader.NodeCount = RecalculateNodeCount(animation.RootNode);
        animation.AnimationHeader.GeometryHeader.FunctionPointer1 = IsTSL ? MDLBinaryGeometryHeader.K2_ANIM_FP1 : MDLBinaryGeometryHeader.K1_ANIM_FP1;
        animation.AnimationHeader.GeometryHeader.FunctionPointer2 = IsTSL ? MDLBinaryGeometryHeader.K2_ANIM_FP1 : MDLBinaryGeometryHeader.K1_ANIM_FP1;

        var animationOffset = offset;

        offset += MDLBinaryAnimationHeader.SIZE;
        animation.AnimationHeader.GeometryHeader.RootNodeOffset = offset;

        Recalculate(animation.RootNode, ref offset, ref mdxOffset, animationOffset, 0);
        animation.AnimationHeader.OffsetToEventArray = (animation.Events.Count() == 0) ? 0 : offset;

        offset += MDLBinaryAnimationEvent.SIZE * animation.Events.Count;
    }
    private void Recalculate(MDLBinaryNode node, ref int offset, ref int mdxOffset, int offsetToRoot, int offsetToParent)
    {
        var nodeOffset = offset;

        if (node.NodeHeader is not null)
            offset += MDLBinaryNodeHeader.SIZE;
        if (node.LightHeader is not null)
            offset += MDLBinaryLightHeader.SIZE;
        if (node.EmitterHeader is not null)
            offset += MDLBinaryEmitterHeader.SIZE;
        if (node.ReferenceHeader is not null)
            offset += MDLBinaryReferenceHeader.SIZE;
        if (node.TrimeshHeader is not null)
            offset += IsTSL ? MDLBinaryTrimeshHeader.K2_SIZE : MDLBinaryTrimeshHeader.K1_SIZE;
        if (node.SkinmeshHeader is not null)
            offset += MDLBinarySkinmeshHeader.SIZE;
        if (node.DanglymeshHeader is not null)
            offset += MDLBinaryDanglyHeader.SIZE;
        if (node.SabermeshHeader is not null)
            offset += MDLBinarySabermeshHeader.SIZE;
        if (node.WalkmeshHeader is not null)
            offset += MDLBinaryWalkmeshHeader.SIZE;

        if (node.NodeHeader is not null)
        {
            node.NodeHeader.ChildArrayCount = node.Children.Count;
            node.NodeHeader.ChildArrayCount2 = node.Children.Count;
            node.NodeHeader.ControllerArrayCount = node.ControllerHeaders.Count;
            node.NodeHeader.ControllerArrayCount2 = node.ControllerHeaders.Count;
            node.NodeHeader.ControllerDataCount = node.ControllerData.Count;
            node.NodeHeader.ControllerDataCount2 = node.ControllerData.Count;

            node.NodeHeader.OffsetToRootNode = offsetToRoot;
            node.NodeHeader.OffsetToParentNode = offsetToParent;

            node.NodeHeader.OffsetToChildOffsetArray = offset;

            offset += 4 * node.Children.Count;
            node.NodeHeader.OffsetToControllerArray = offset;

            offset += MDLBinaryControllerHeader.SIZE * node.ControllerHeaders.Count;
            node.NodeHeader.OffsetToControllerData = offset;

            offset += 4 * node.ControllerData.Count;
        }

        if (node.LightHeader is not null)
        {
            node.LightHeader.UnknownArrayCount = 0;
            node.LightHeader.FlareSizeArrayCount = node.Light.FlareSizes.Count;
            node.LightHeader.FlarePositionArrayCount = node.Light.FlarePositions.Count;
            node.LightHeader.FlareColorShiftArrayCount = node.Light.FlareColourShifts.Count;
            node.LightHeader.FlareTextureNameCount = node.Light.FlareTextureNameOffsets.Count;

            node.LightHeader.OffsetToUnknownArray = offset;

            offset += 0;
            node.LightHeader.OffsetToFlareSizeArray = offset;

            offset += 4 * node.Light.FlareSizes.Count;
            node.LightHeader.OffsetToFlarePositionArray = offset;

            offset += 4 * node.Light.FlarePositions.Count;
            node.LightHeader.OffsetToFlareColorShiftArray = offset;

            offset += 12 * node.Light.FlareColourShifts.Count;
            node.LightHeader.OffsetToFlareTextureNameOffsetsArray = offset;

            offset += 4 * node.Light.FlareTextures.Count;
            foreach (var name in node.Light.FlareTextures)
            {
                node.Light.FlareTextureNameOffsets.Add(offset);
                offset += name.Length + 1;
            }
        }

        if (node.TrimeshHeader is not null)
        {
            // MDX
            node.TrimeshHeader.MDXDataSize = 0;

            node.TrimeshHeader.MDXOffsetToData = mdxOffset;

            var hasPosition = node.MDXVertices.ElementAtOrDefault(0)?.Position is not null;
            node.TrimeshHeader.MDXPositionStride = hasPosition ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasPosition ? 12 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasPosition ? (uint)MDLBinaryMDXVertexBitmask.Position : 0;

            var hasNormal = node.MDXVertices.ElementAtOrDefault(0)?.Normal is not null;
            node.TrimeshHeader.MDXNormalStride = hasNormal ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasNormal ? 12 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasNormal ? (uint)MDLBinaryMDXVertexBitmask.Normals : 0;

            node.TrimeshHeader.MDXColourStride = -1;

            var hasUV1 = node.MDXVertices.ElementAtOrDefault(0)?.UV1 is not null;
            node.TrimeshHeader.MDXTexture1Stride = hasUV1 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasUV1 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasUV1 ? (uint)MDLBinaryMDXVertexBitmask.UV1 : 0;

            var hasUV2 = node.MDXVertices.ElementAtOrDefault(0)?.UV2 is not null;
            node.TrimeshHeader.MDXTexture2Stride = hasUV2 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasUV2 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasUV2 ? (uint)MDLBinaryMDXVertexBitmask.UV2 : 0;

            var hasUV3 = node.MDXVertices.ElementAtOrDefault(0)?.UV3 is not null;
            node.TrimeshHeader.MDXTexture3Stride = hasUV3 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasUV3 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasUV3 ? (uint)MDLBinaryMDXVertexBitmask.UV3 : 0;

            var hasUV4 = node.MDXVertices.ElementAtOrDefault(0)?.UV4 is not null;
            node.TrimeshHeader.MDXTexture4Stride = hasUV4 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasUV4 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasUV4 ? (uint)MDLBinaryMDXVertexBitmask.UV4 : 0;

            var hasTangent1 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent1 is not null;
            node.TrimeshHeader.MDXTangent1Stride = hasTangent1 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasTangent1 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasTangent1 ? (uint)MDLBinaryMDXVertexBitmask.Tangent1 : 0;

            var hasTangent2 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent2 is not null;
            node.TrimeshHeader.MDXTangent2Stride = hasTangent2 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasTangent2 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasTangent2 ? (uint)MDLBinaryMDXVertexBitmask.Tangent2 : 0;

            var hasTangent3 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent3 is not null;
            node.TrimeshHeader.MDXTangent3Stride = hasTangent3 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasTangent3 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasTangent3 ? (uint)MDLBinaryMDXVertexBitmask.Tangent3 : 0;

            var hasTangent4 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent4 is not null;
            node.TrimeshHeader.MDXTangent4Stride = hasTangent4 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataSize += hasTangent4 ? 8 : 0;
            node.TrimeshHeader.MDXDataBitmap |= hasTangent4 ? (uint)MDLBinaryMDXVertexBitmask.Tangent4 : 0;

            mdxOffset += node.TrimeshHeader.MDXDataSize * node.MDXVertices.Count();

            // MDL
            node.Trimesh.VertexIndiciesCounts = node.Trimesh.VertexIndices.Select(x => x.Count()).ToList();
            node.TrimeshHeader.FaceArrayCount = node.Trimesh.Faces.Count;
            node.TrimeshHeader.FaceArrayCount2 = node.Trimesh.Faces.Count;
            node.TrimeshHeader.InvertedCounterArrayCount = node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.InvertedCounterArrayCount2 = node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.VertexIndicesCountArrayCount = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexIndicesCountArrayCount2 = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexIndicesOffsetArrayCount = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexIndicesOffsetArrayCount2 = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexCount = (ushort)node.Trimesh.Vertices.Count;

            node.TrimeshHeader.OffsetToFaceArray = offset;

            offset += MDLBinaryTrimeshFace.SIZE * node.Trimesh.Faces.Count;
            node.TrimeshHeader.OffsetToInvertedCounterArray = offset;

            offset += 4 * node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.OffsetToVertexIndicesCountArray = offset;

            offset += 4 * node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.OffsetToVertexIndicesOffsetArray = offset;

            offset += 4 * node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.OffsetToVertexArray = offset;

            offset += 12 * node.Trimesh.Vertices.Count;
            node.Trimesh.VertexIndicesOffsets = Enumerable.Range(0, node.Trimesh.VertexIndiciesCounts.Count).ToList();
            for (int i = 0; i < node.Trimesh.VertexIndicesOffsets.Count; i++)
            {
                node.Trimesh.VertexIndicesOffsets[i] = offset;

                offset += 2 * node.Trimesh.VertexIndiciesCounts[i];
            }
        }

        if (node.SkinmeshHeader is not null)
        {
            node.SkinmeshHeader.BonemapCount = node.Skinmesh.Bonemap.Count;
            node.SkinmeshHeader.TBonesCount = node.Skinmesh.TBones.Count;
            node.SkinmeshHeader.TBonesCount2 = node.Skinmesh.TBones.Count;
            node.SkinmeshHeader.QBonesCount = node.Skinmesh.QBones.Count;
            node.SkinmeshHeader.QBonesCount2 = node.Skinmesh.QBones.Count;
            node.SkinmeshHeader.Array8Count = node.Skinmesh.Array8.Count;
            node.SkinmeshHeader.Array8Count2 = node.Skinmesh.Array8.Count;

            node.SkinmeshHeader.BonemapOffset = offset;
            offset += 4 * node.Skinmesh.Bonemap.Count;

            node.SkinmeshHeader.TBonesOffset = offset;
            offset += 12 * node.Skinmesh.TBones.Count;

            node.SkinmeshHeader.QBonesOffset = offset;
            offset += 16 * node.Skinmesh.QBones.Count;

            node.SkinmeshHeader.Array8Offset = offset;
            offset += 4 * node.Skinmesh.Array8.Count;
        }

        if (node.DanglymeshHeader is not null)
        {
            node.DanglymeshHeader.ContraintArrayCount = node.Danglymesh.Constraints.Count();
            node.DanglymeshHeader.ContraintArrayCount2 = node.Danglymesh.Constraints.Count();

            node.DanglymeshHeader.OffsetToContraintArray = offset;

            offset += 4 * node.Danglymesh.Constraints.Count();
            node.DanglymeshHeader.OffsetToDataArray = offset;

            offset += 12 * node.Danglymesh.Constraints.Count();
        }

        if (node.SabermeshHeader is not null)
        {
            node.SabermeshHeader.OffsetToNormalArray = offset;

            offset += 12 * node.Trimesh.Vertices.Count();
            node.SabermeshHeader.OffsetToTexCoordArray = offset;

            offset += 12 * node.Trimesh.Vertices.Count();
            node.SabermeshHeader.OffsetToVertexArray = offset;

            offset += 8 * node.Trimesh.Vertices.Count();
        }

        if (node.WalkmeshHeader is not null)
        {
            node.WalkmeshHeader.OffsetToRootAABBNode = offset;
            Recalculate(node.RootAABBNode, ref offset);
        }

        node.ChildrenOffsets = Enumerable.Range(0, node.Children.Count).Select(x => 0).ToList();
        for (int i = 0; i < node.Children.Count; i++)
        {
            node.ChildrenOffsets[i] = offset;
            Recalculate(node.Children[i], ref offset, ref mdxOffset, offsetToRoot, nodeOffset);
        }
    }
    private void Recalculate(MDLBinaryAABBNode aabb, ref int offset)
    {
        offset += MDLBinaryAABBNode.SIZE;

        aabb.LeftChildOffset = (aabb.LeftNode is null) ? 0 : offset;
        if (aabb.LeftNode is not null)
            Recalculate(aabb.LeftNode, ref offset);

        aabb.RightChildOffset = (aabb.RightNode is null) ? 0 : offset;
        if (aabb.RightNode is not null)
            Recalculate(aabb.RightNode, ref offset);
    }
    private int RecalculateNodeCount(MDLBinaryNode node)
    {
        int count = 1;
        node.Children.ForEach(x => RecalculateNodeCount(x));
        return count;
    }

}
