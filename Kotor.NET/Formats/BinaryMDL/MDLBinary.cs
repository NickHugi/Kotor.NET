using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

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
    public MDLBinary(BinaryReader reader, BinaryReader mdxReader) :this(new(reader.BaseStream), mdxReader)
    {
    }
    public MDLBinary(MDLBinaryReader reader, BinaryReader mdxReader)
    {
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

    public void Write(BinaryWriter writer, BinaryWriter mdxWriter)
    {
        writer = new MDLBinaryWriter(writer.BaseStream);
        Write((MDLBinaryWriter)writer, mdxWriter);
    }
    public void Write(MDLBinaryWriter writer, BinaryWriter mdxWriter)
    {
        FileHeader.Write(writer);
        ModelHeader.Write(writer);

        writer.SetStreamPosition(ModelHeader.OffsetToNameOffsetArray);
        foreach (var nameOffset in NamesOffset)
        {
            writer.Write(nameOffset);
        }

        for (int i = 0; i < Names.Count(); i ++)
        {
            writer.SetStreamPosition(NamesOffset[i]);
            writer.Write(Names[i] + "\0", 0);
        }

        writer.SetStreamPosition(ModelHeader.AnimationOffsetArrayOffset);
        foreach (var animationOffset in AnimationOffsets)
        {
            writer.Write(animationOffset);
        }
        for (int i = 0; i < AnimationOffsets.Count(); i ++)
        {
            writer.SetStreamPosition(AnimationOffsets[i]);
            Animations[i].Write(writer, mdxWriter);
        }

        writer.SetStreamPosition(ModelHeader.OffsetToRootNode);
        RootNode.Write(writer, mdxWriter);
    }

    public void Recalculate()
    {
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

        offset += 4 * AnimationOffsets.Count();
        AnimationOffsets = new();
        foreach (var animation in Animations)
        {
            AnimationOffsets.Add(offset);
            Recalculate(animation, ref offset, ref mdxOffset);
        }

        ModelHeader.OffsetToRootNode = offset;
        ModelHeader.GeometryHeader.RootNodeOffset = offset;
        Recalculate(RootNode, ref offset, ref mdxOffset, 0, 0);

        FileHeader.MDLSize = offset;
        FileHeader.MDXSize = mdxOffset;
    }
    private void Recalculate(MDLBinaryAnimation animation, ref int offset, ref int mdxOffset)
    {
        var animationOffset = offset;
        animation.AnimationHeader.EventCount = animation.Events.Count;
        animation.AnimationHeader.EventCount2 = animation.Events.Count;

        offset += MDLBinaryAnimationHeader.SIZE;
        animation.AnimationHeader.GeometryHeader.RootNodeOffset = offset;

        Recalculate(animation.RootNode, ref offset, ref mdxOffset, animationOffset, 0);
        animation.AnimationHeader.OffsetToEventArray = offset;

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
            node.LightHeader.FlareTextureNameCount = node.Light.FlareTextureOffsets.Count;

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
                node.Light.FlareTextureOffsets.Add(offset);
                offset += name.Length + 1;
            }
        }

        if (node.TrimeshHeader is not null)
        {
            node.TrimeshHeader.FaceArrayCount = node.Trimesh.Faces.Count;
            node.TrimeshHeader.FaceArrayCount2 = node.Trimesh.Faces.Count;
            node.TrimeshHeader.InvertedCounterArrayCount = node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.InvertedCounterArrayCount2 = node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.VertexIndicesCountArrayCount = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexIndicesCountArrayCount2 = node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.VertexIndicesOffsetArrayCount = node.Trimesh.VertexIndicesOffsets.Count;
            node.TrimeshHeader.VertexIndicesOffsetArrayCount2 = node.Trimesh.VertexIndicesOffsets.Count;
            node.TrimeshHeader.VertexCount = (ushort)node.Trimesh.Vertices.Count;
            node.Trimesh.VertexIndiciesCounts = node.Trimesh.VertexIndices.Select(x => x.Count()).ToList();

            node.TrimeshHeader.OffsetToFaceArray = offset;

            offset += MDLBinaryTrimeshFace.SIZE * node.Trimesh.Faces.Count;
            node.TrimeshHeader.OffsetToInvertedCounterArray = offset;

            offset += 4 * node.Trimesh.InvertedCounters.Count;
            node.TrimeshHeader.OffsetToVertexIndicesCountArray = offset;

            offset += 4 * node.Trimesh.VertexIndiciesCounts.Count;
            node.TrimeshHeader.OffsetToVertexIndicesOffsetArray = offset;

            offset += 4 * node.Trimesh.VertexIndicesOffsets.Count;
            node.TrimeshHeader.OffsetToVertexArray = offset;

            offset += 12 * node.Trimesh.Vertices.Count;
            for (int i = 0; i < node.Trimesh.VertexIndicesOffsets.Count; i++)
            {
                node.Trimesh.VertexIndicesOffsets[i] = offset;

                offset += 2 * node.Trimesh.VertexIndiciesCounts[i];
            }

            node.TrimeshHeader.MDXOffsetToData = mdxOffset;
            mdxOffset += node.TrimeshHeader.MDXDataSize * (node.Trimesh.Vertices.Count() + 1);
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
}
