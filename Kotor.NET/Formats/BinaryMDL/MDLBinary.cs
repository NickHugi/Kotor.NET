using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        FileHeader.MDLSize = offset;
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
            var hasTangent1 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent1 is not null;
            var hasTangent2 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent2 is not null;
            var hasTangent3 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent3 is not null;
            var hasTangent4 = node.MDXVertices.ElementAtOrDefault(0)?.Tangent4 is not null;

            node.TrimeshHeader.MDXOffsetToData = mdxOffset;

            var hasPosition = node.MDXVertices.ElementAtOrDefault(0)?.Position is not null;
            node.TrimeshHeader.MDXDataSize += hasPosition ? 12 : 0;
            node.TrimeshHeader.MDXPositionStride = hasPosition ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasPosition ? (uint)MDLBinaryMDXVertexBitmask.Position : 0;

            var hasNormal = node.MDXVertices.ElementAtOrDefault(0)?.Normal is not null;
            node.TrimeshHeader.MDXDataSize += hasNormal ? 12 : 0;
            node.TrimeshHeader.MDXNormalStride = hasNormal ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasNormal ? (uint)MDLBinaryMDXVertexBitmask.Normals : 0;

            var hasUV1 = node.MDXVertices.ElementAtOrDefault(0)?.UV1 is not null;
            node.TrimeshHeader.MDXDataSize += hasUV1 ? 8 : 0;
            node.TrimeshHeader.MDXTexture1Stride = hasUV1 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasUV1 ? (uint)MDLBinaryMDXVertexBitmask.UV1 : 0;

            var hasUV2 = node.MDXVertices.ElementAtOrDefault(0)?.UV2 is not null;
            node.TrimeshHeader.MDXDataSize += hasUV2 ? 8 : 0;
            node.TrimeshHeader.MDXTexture2Stride = hasUV2 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasUV2 ? (uint)MDLBinaryMDXVertexBitmask.UV2 : 0;

            var hasUV3 = node.MDXVertices.ElementAtOrDefault(0)?.UV3 is not null;
            node.TrimeshHeader.MDXDataSize += hasUV3 ? 8 : 0;
            node.TrimeshHeader.MDXTexture3Stride = hasUV3 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasUV3 ? (uint)MDLBinaryMDXVertexBitmask.UV3 : 0;

            var hasUV4 = node.MDXVertices.ElementAtOrDefault(0)?.UV4 is not null;
            node.TrimeshHeader.MDXDataSize += hasUV4 ? 8 : 0;
            node.TrimeshHeader.MDXTexture4Stride = hasUV4 ? node.TrimeshHeader.MDXDataSize : -1;
            node.TrimeshHeader.MDXDataBitmap |= hasUV4 ? (uint)MDLBinaryMDXVertexBitmask.UV4 : 0;

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

    public MDL Parse()
    {
        var mdl = new MDL();

        mdl.Name = ModelHeader.GeometryHeader.Name;
        mdl.ModelType = ModelHeader.ModelType;
        mdl.AffectedByFog = ModelHeader.Fog > 0;
        mdl.AnimationScale = ModelHeader.AnimationScale;
        mdl.SupermodelName = ModelHeader.SupermodelName;

        mdl.Animations = Animations.Select(x => ParseAnimation(x)).ToList();
        mdl.Root = ParseNode(RootNode);

        return mdl;
    }
    private MDLAnimation ParseAnimation(MDLBinaryAnimation binaryAnim)
    {
        var animation = new MDLAnimation();
        animation.Name = binaryAnim.AnimationHeader.GeometryHeader.Name;
        animation.AnimationRoot = binaryAnim.AnimationHeader.AnimationRoot;
        animation.AnimationLength = binaryAnim.AnimationHeader.AnimationLength;
        animation.TransitionTime = binaryAnim.AnimationHeader.TransitionTime;
        animation.RootNode = ParseNode(binaryAnim.RootNode);
        animation.Events = binaryAnim.Events.Select(x => new MDLAnimationEvent()
        {
            Name = x.Name,
            ActivationTime = x.ActivationTime,
        }).ToList();
        return animation;
    }
    private MDLNode ParseNode(MDLBinaryNode binaryNode)
    {
        var name = Names[binaryNode.NodeHeader.NameIndex];
        var type = (MDLBinaryNodeType)binaryNode.NodeHeader.NodeType;
        MDLNode node;

        if (type.HasFlag(MDLBinaryNodeType.AABBFlag))
        {
            var walkmeshNode = new MDLWalkmeshNode(name);
            node = walkmeshNode;
        }
        else if (type.HasFlag(MDLBinaryNodeType.SaberFlag))
        {
            var saberNode = new MDLSaberNode(name);
            node = saberNode;

            saberNode.Unknown1 = binaryNode.SabermeshHeader.Unknown1;
            saberNode.Unknown2 = binaryNode.SabermeshHeader.Unknown2;
        }
        else if (type.HasFlag(MDLBinaryNodeType.DanglyFlag))
        {
            var danglyNode = new MDLDanglyNode(name);
            node = danglyNode;

            danglyNode.Displacement = binaryNode.DanglymeshHeader.Displacement;
            danglyNode.Tightness = binaryNode.DanglymeshHeader.Tightness;
            danglyNode.Period = binaryNode.DanglymeshHeader.Period;
        }
        else if (type.HasFlag(MDLBinaryNodeType.SkinFlag))
        {
            var skinNode = new MDLSkinNode(name);
            node = skinNode;

            skinNode.BoneMap = Enumerable.Range(0, binaryNode.Skinmesh.Bonemap.Count()).Select((x, index) => new MDLBone
            {
                Bonemap = x,
                TBone = binaryNode.Skinmesh.TBones[index],
                QBone = binaryNode.Skinmesh.QBones[index],
            }).ToList();
        }
        else if (type.HasFlag(MDLBinaryNodeType.ReferenceFlag))
        {
            var referenceNode = new MDLReferenceNode(name);
            node = referenceNode;

            referenceNode.ModelResRef = binaryNode.ReferenceHeader.ModelResRef;
            referenceNode.Reattachable = binaryNode.ReferenceHeader.Reattachable;
        }
        else if (type.HasFlag(MDLBinaryNodeType.EmitterFlag))
        {
            var emitterNode = new MDLEmitterNode(name);
            node = emitterNode;

            emitterNode.DeadSpace = binaryNode.EmitterHeader.DeadSpace;
            emitterNode.BlastRadius = binaryNode.EmitterHeader.BlastRadius;
            emitterNode.BlastLength = binaryNode.EmitterHeader.BlastLength;
            emitterNode.BranchCount = binaryNode.EmitterHeader.BranchCount;
            emitterNode.ControlPointSmoothing = binaryNode.EmitterHeader.ControlPointSmoothing;
            emitterNode.XGrid = binaryNode.EmitterHeader.XGrid;
            emitterNode.YGrid = binaryNode.EmitterHeader.YGrid;
            emitterNode.SpawnType = binaryNode.EmitterHeader.SpawnType;
            emitterNode.Update = binaryNode.EmitterHeader.Update;
            emitterNode.Render = binaryNode.EmitterHeader.Render;
            emitterNode.Blend = binaryNode.EmitterHeader.Blend;
            emitterNode.Texture = binaryNode.EmitterHeader.Texture;
            emitterNode.ChunkName = binaryNode.EmitterHeader.ChunkName;
            emitterNode.TwoSidedTexture = binaryNode.EmitterHeader.TwoSidedTexture;
            emitterNode.Loop = binaryNode.EmitterHeader.Loop;
            emitterNode.RenderOrder = binaryNode.EmitterHeader.RenderOrder;
            emitterNode.FrameBlending = binaryNode.EmitterHeader.FrameBlending;
            emitterNode.DepthTextureName = binaryNode.EmitterHeader.DepthTextureName;
            var flags = (MDLBinaryEmitterFlags)binaryNode.EmitterHeader.Flags;
            emitterNode.P2P = flags.HasFlag(MDLBinaryEmitterFlags.P2P);
            emitterNode.P2P_SEL = flags.HasFlag(MDLBinaryEmitterFlags.P2P_SEL);
            emitterNode.AffectedByWind = flags.HasFlag(MDLBinaryEmitterFlags.AffectedByWind);
            emitterNode.Tinted = flags.HasFlag(MDLBinaryEmitterFlags.Tinted);
            emitterNode.Bounce = flags.HasFlag(MDLBinaryEmitterFlags.Bounce);
            emitterNode.Random = flags.HasFlag(MDLBinaryEmitterFlags.Random);
            emitterNode.Inherit = flags.HasFlag(MDLBinaryEmitterFlags.Inherit);
            emitterNode.InheritVelocity = flags.HasFlag(MDLBinaryEmitterFlags.InheritVelocity);
            emitterNode.InheritLocal = flags.HasFlag(MDLBinaryEmitterFlags.InheritLocal);
            emitterNode.Splat = flags.HasFlag(MDLBinaryEmitterFlags.Splat);
            emitterNode.InheritPart = flags.HasFlag(MDLBinaryEmitterFlags.InheritPart);
            emitterNode.DepthTexture = flags.HasFlag(MDLBinaryEmitterFlags.DepthTexture);
            emitterNode.Flag13 = flags.HasFlag(MDLBinaryEmitterFlags.Flag13);
        }
        else if (type.HasFlag(MDLBinaryNodeType.LightFlag))
        {
            var lightNode = new MDLLightNode(name);
            node = lightNode;

            lightNode.FlareRadius = binaryNode.LightHeader.FlareRadius;
            lightNode.LightPriority = binaryNode.LightHeader.LightPriority;
            lightNode.AmbientOnly = binaryNode.LightHeader.AmbientOnly;
            lightNode.DynamicType = binaryNode.LightHeader.DynamicType;
            lightNode.AffectDynamic = binaryNode.LightHeader.AffectDynamic;
            lightNode.Shadow = binaryNode.LightHeader.Shadow;
            lightNode.Flare = binaryNode.LightHeader.Flare;
            lightNode.FadingLight = binaryNode.LightHeader.FadingLight;

            for (int i = 0; i < binaryNode.LightHeader.FlareColorShiftArrayCount; i++)
            {
                lightNode.Flares.Add(new MDLFlare
                {
                    Position = binaryNode.Light.FlarePositions[i],
                    Size = binaryNode.Light.FlareSizes[i],
                    ColourShift = binaryNode.Light.FlareColourShifts[i],
                    TextureName = binaryNode.Light.FlareTextures[i],
                });
            }
        }
        else if (type.HasFlag(MDLBinaryNodeType.TrimeshFlag))
        {
            node = new MDLTrimeshNode(name);
        }
        else
        {
            node = new MDLNode(name);
        }

        if (node is MDLTrimeshNode trimeshNode)
        {
            trimeshNode.TotalArea = binaryNode.TrimeshHeader.TotalArea;
            trimeshNode.Radius = binaryNode.TrimeshHeader.Radius;
            trimeshNode.BoundingBox = new(binaryNode.TrimeshHeader.BoundingBoxMin, binaryNode.TrimeshHeader.BoundingBoxMax);
            trimeshNode.AveragePoint = binaryNode.TrimeshHeader.AveragePoint;
            trimeshNode.IsBackgroundGeometry = binaryNode.TrimeshHeader.BackgroundGeometry > 0;
            trimeshNode.DiffuseTexture = binaryNode.TrimeshHeader.Texture;
            trimeshNode.LightmapTexture = binaryNode.TrimeshHeader.Lightmap;
            trimeshNode.Renders = binaryNode.TrimeshHeader.DoesRender > 0;
            trimeshNode.CastsShadow = binaryNode.TrimeshHeader.HasShadow > 0;
            trimeshNode.Beaming = binaryNode.TrimeshHeader.Beaming > 0;
            trimeshNode.RotateTexture = binaryNode.TrimeshHeader.RotateTexture > 0;
            trimeshNode.Diffuse = binaryNode.TrimeshHeader.Diffuse;
            trimeshNode.Ambient = binaryNode.TrimeshHeader.Ambient;
            trimeshNode.TransparencyHint = binaryNode.TrimeshHeader.TransparencyHint;
            trimeshNode.UVSpeed = binaryNode.TrimeshHeader.UVSpeed;
            trimeshNode.UVJitterSpeed = binaryNode.TrimeshHeader.UVJitterSpeed;
            trimeshNode.UVDirection = binaryNode.TrimeshHeader.UVDirection;
            trimeshNode.HideInHologram = binaryNode.TrimeshHeader.HideInHolograms > 0;
            trimeshNode.DirtEnabled = binaryNode.TrimeshHeader.DirtEnabled > 0;
            trimeshNode.DirtTexture = binaryNode.TrimeshHeader.DirtTexture;
            trimeshNode.DirtCoordinateSpace = binaryNode.TrimeshHeader.DirtCoordinateSpace;

            var vertexBitmap = (MDLBinaryMDXVertexBitmask)binaryNode.TrimeshHeader.MDXDataBitmap;

            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.Position))
            {
                trimeshNode.EnableVertices();
            }
            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.Normals))
            {
                trimeshNode.EnableNormals();
            }
            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.UV1))
            {
                trimeshNode.EnableDiffuseUVs();
            }
            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.UV2))
            {
                trimeshNode.EnableLightmapUVs();
            }
            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.Tangent1))
            {
                trimeshNode.EnableTangents();
            }

            var vertices = binaryNode.MDXVertices.Select((x, index) => new MDLVertex
            {
                _position = x.Position,
                _normal = x.Normal,
                _diffuseUV = x.UV1,
                _lightmapUV = x.UV2,
                _tangent1 = x.Tangent1,
                _tangent2 = x.Tangent2,
                _tangent3 = x.Tangent3,
                _tangent4 = x.Tangent4,
                _dangly = (binaryNode.Danglymesh is null) ? null : new MDLDanglyVertexData
                {
                    Constraint = binaryNode.Danglymesh.Constraints.ElementAtOrDefault(index),
                    Unknown = binaryNode.Danglymesh.Data.ElementAtOrDefault(index) ?? new(),
                },
                _saber = (trimeshNode is not MDLSaberNode) ? null : new MDLSaberVertexData
                {
                    Position = x.Position,
                    Normal = x.Normal,
                    UV = x.UV1,
                },
                _skin = (trimeshNode is not MDLSkinNode) ? null : new MDLSkinVertexData
                {
                    WeightIndex1 = x.BoneIndex1 ?? -1,
                    WeightIndex2 = x.BoneIndex2 ?? -1,
                    WeightIndex3 = x.BoneIndex3 ?? -1,
                    WeightIndex4 = x.BoneIndex4 ?? -1,
                    WeightValue1 = x.BoneWeight1 ?? 0,
                    WeightValue2 = x.BoneWeight2 ?? 0,
                    WeightValue3 = x.BoneWeight3 ?? 0,
                    WeightValue4 = x.BoneWeight4 ?? 0,
                },
            }).ToList();

            binaryNode.Trimesh.Faces.ForEach(x =>
            {
                var vertex1 = vertices[x.Vertex1];
                var vertex2 = vertices[x.Vertex2];
                var vertex3 = vertices[x.Vertex3];
                trimeshNode.Faces.Add(vertex1, vertex2, vertex3);
            });

            if (trimeshNode is MDLWalkmeshNode walkmeshNode)
            {
                walkmeshNode.RootNode = ParseAABB(walkmeshNode, binaryNode.RootAABBNode);
            }
        }

        node.Controllers = binaryNode.ControllerHeaders.Select(x => ParseController(binaryNode, x)).ToList();
        node.Children = binaryNode.Children.Select(x => ParseNode(x)).ToList();
        node.NodeIndex = binaryNode.NodeHeader.NodeIndex;
        return node;
    }
    private MDLController<BaseMDLControllerRow<BaseMDLControllerData>> ParseController(MDLBinaryNode binaryNode, MDLBinaryControllerHeader binaryController)
    {
        var nodeType = (MDLBinaryNodeType)binaryNode.NodeHeader.NodeType;
        var controllerType = (MDLBinaryControllerType)binaryController.ControllerType;
        var columnCount = binaryController.ColumnCount & 0x0F;
        var bezier = (binaryController.ColumnCount & 0x10) != 0;

        var data = binaryNode.ControllerData
            .Skip(binaryController.FirstDataOffset)
            .Take(binaryController.ColumnCount)
            .ToList();

        var times = binaryNode.ControllerData
            .Skip(binaryController.FirstKeyOffset)
            .Take(binaryController.RowCount * binaryController.ColumnCount)
            .Select(x => BitConverter.ToSingle(x))
            .ToList();

        var controller = new MDLController<BaseMDLControllerRow<BaseMDLControllerData>>();
        controller.Rows = Enumerable
            .Range(0, binaryController.RowCount)
            .ToList()
            .Select(x => ParseControllerRow(nodeType, controllerType, columnCount, bezier, times[x], data.Take(columnCount * binaryController.RowCount)))
            .ToList();
        return controller;
    }
    private BaseMDLControllerRow<BaseMDLControllerData> ParseControllerRow(MDLBinaryNodeType nodeType, MDLBinaryControllerType controllerType, int columnCount, bool bezier, float timeStart, IEnumerable<byte[]> data)
    {
        if (bezier)
        {
            var cdata1 = ParseControllerData(nodeType, controllerType, data.Skip(columnCount * 0).Take(columnCount));
            var cdata2 = ParseControllerData(nodeType, controllerType, data.Skip(columnCount * 1).Take(columnCount));
            var cdata3 = ParseControllerData(nodeType, controllerType, data.Skip(columnCount * 2).Take(columnCount));
            return new MDLControllerRowBezier<BaseMDLControllerData>(timeStart, cdata1, cdata2, cdata3);
        }
        else
        {
            var cdata = ParseControllerData(nodeType, controllerType, data);
            return new MDLControllerRow<BaseMDLControllerData>(timeStart, cdata);
        }
    }
    private BaseMDLControllerData ParseControllerData(MDLBinaryNodeType nodeType, MDLBinaryControllerType controllerType, IEnumerable<byte[]> data)
    {
        var columnCount = data.Count();
        var floatData = data.Select(x => BitConverter.ToSingle(x)).ToList();

        if ((nodeType & MDLBinaryNodeType.TrimeshFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerDataPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerDataOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.Scale => new MDLControllerDataScale(floatData[0]),
                MDLBinaryControllerType.SelfIlluminationColour => new MDLControllerDataMeshSelfIllumination(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Alpha => new MDLControllerDataAlpha(floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
        else if ((nodeType & MDLBinaryNodeType.LightFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerDataPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerDataOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.Colour => new MDLControllerDataLightColour(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Radius => new MDLControllerDataLightRadius(floatData[0]),
                MDLBinaryControllerType.ShadowRadius => new MDLControllerDataLightShadowRadius(floatData[0]),
                MDLBinaryControllerType.VerticalDisplacement => new MDLControllerDataLightVerticalDisplacement(floatData[0]),
                MDLBinaryControllerType.Multiplier => new MDLControllerDataLightMultiplier(floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
        else if ((nodeType & MDLBinaryNodeType.EmitterFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerDataPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerDataOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.AlphaEnd => new MDLControllerDataEmitterAlphaEnd(floatData[0]),
                MDLBinaryControllerType.AlphaMid => new MDLControllerDataEmitterAlphaMiddle(floatData[0]),
                MDLBinaryControllerType.AlphaStart => new MDLControllerDataEmitterAlphaStart(floatData[0]),
                MDLBinaryControllerType.BirthRate => new MDLControllerDataEmitterBirthRate(floatData[0]),
                MDLBinaryControllerType.BounceCo => new MDLControllerDataEmitterBounceCo(floatData[0]),
                MDLBinaryControllerType.CombineTime => new MDLControllerDataEmitterCombineTime(floatData[0]),
                MDLBinaryControllerType.Drag => new MDLControllerDataEmitterDrag(floatData[0]),
                MDLBinaryControllerType.FPS => new MDLControllerDataEmitterFPS(floatData[0]),
                MDLBinaryControllerType.FrameStart => new MDLControllerDataEmitterFrameStart(floatData[0]),
                MDLBinaryControllerType.FrameEnd => new MDLControllerDataEmitterFrameEnd(floatData[0]),
                MDLBinaryControllerType.Gravity => new MDLControllerDataEmitterGravity(floatData[0]),
                MDLBinaryControllerType.LifeExpectancy => new MDLControllerDataEmitterLifeExpectancy(floatData[0]),
                MDLBinaryControllerType.Mass => new MDLControllerDataEmitterMass(floatData[0]),
                MDLBinaryControllerType.P2PBezier2 => new MDLControllerDataEmitterP2PBezier2(floatData[0]),
                MDLBinaryControllerType.P2PBezier3 => new MDLControllerDataEmitterP2PBezier3(floatData[0]),
                MDLBinaryControllerType.ParticleRotation => new MDLControllerDataEmitterParticleRotation(floatData[0]),
                MDLBinaryControllerType.RandomVelocity => new MDLControllerDataEmitterRandomVelocity(floatData[0]),
                MDLBinaryControllerType.SizeStart => new MDLControllerDataEmitterSizeStart(floatData[0]),
                MDLBinaryControllerType.SizeStartY => new MDLControllerEmitterSizeYStart(floatData[0]),
                MDLBinaryControllerType.SizeMid => new MDLControllerDataEmitterSizeMiddle(floatData[0]),
                MDLBinaryControllerType.SizeMidY => new MDLControllerDataEmitterSizeYMiddle(floatData[0]),
                MDLBinaryControllerType.SizeEnd => new MDLControllerDataEmitterSizeEnd(floatData[0]),
                MDLBinaryControllerType.SizeEndY => new MDLControllerDataEmitterSizeYEnd(floatData[0]),
                MDLBinaryControllerType.Spread => new MDLControllerDataEmitterSpread(floatData[0]),
                MDLBinaryControllerType.Threshold => new MDLControllerDataEmitterThreshold(floatData[0]),
                MDLBinaryControllerType.Velocity => new MDLControllerDataEmitterVelocity(floatData[0]),
                MDLBinaryControllerType.XSize => new MDLControllerDataEmitterSizeX(floatData[0]),
                MDLBinaryControllerType.YSize => new MDLControllerDataEmitterSizeY(floatData[0]),
                MDLBinaryControllerType.BlurLength => new MDLControllerDataEmitterBlurLength(floatData[0]),
                MDLBinaryControllerType.LightningDelay => new MDLControllerDataEmitterLightningDelay(floatData[0]),
                MDLBinaryControllerType.LightningRadius => new MDLControllerDataEmitterLightningRadius(floatData[0]),
                MDLBinaryControllerType.LightningScale => new MDLControllerDataEmitterLightningScale(floatData[0]),
                MDLBinaryControllerType.LightningSubdivide => new MDLControllerDataEmitterLightningSubDiv(floatData[0]),
                MDLBinaryControllerType.LightningZigZag => new MDLControllerDataEmitterLightningZigZag(floatData[0]),
                MDLBinaryControllerType.PercentStart => new MDLControllerEmitterPercentStart(floatData[0]),
                MDLBinaryControllerType.PercentMid => new MDLControllerDataEmitterPercentMiddle(floatData[0]),
                MDLBinaryControllerType.PercentEnd => new MDLControllerDataEmitterPercentEnd(floatData[0]),
                MDLBinaryControllerType.RandomBirthRate => new MDLControllerDataEmitterRandomBirthRate(floatData[0]),
                MDLBinaryControllerType.TargetSize => new MDLControllerDataEmitterTargetSize(floatData[0]),
                MDLBinaryControllerType.NumberOfControlPoints => new MDLControllerDataEmitterControlPointsCount(floatData[0]),
                MDLBinaryControllerType.ControlPointRadius => new MDLControllerDataEmitterControlPointsRadius(floatData[0]),
                MDLBinaryControllerType.ControlPointDelay => new MDLControllerDataEmitterControlPointsDelay(floatData[0]),
                MDLBinaryControllerType.TangentSpread => new MDLControllerDataEmitterTangentSpread(floatData[0]),
                MDLBinaryControllerType.TangentLength => new MDLControllerDataEmitterTangentLength(floatData[0]),
                MDLBinaryControllerType.ColorStart => new MDLControllerDataEmitterColourStart(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.ColorMid => new MDLControllerDataEmitterColourMiddle(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.ColorEnd => new MDLControllerDataEmitterColourEnd(floatData[0], floatData[1], floatData[2]),
                //MDLBinaryControllerType.EmitterDetonate => new (floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }

        return controllerType switch
        {
            MDLBinaryControllerType.Position => new MDLControllerDataPosition(floatData[0], floatData[1], floatData[2]),
            MDLBinaryControllerType.Orientation => ParseControllerOrientationData(data),
            MDLBinaryControllerType.Scale => new MDLControllerDataScale(floatData[0]),
            _ => throw new ArgumentException("Unknown Controller Type"),
        };
    }
    private MDLWalkmeshAABBNode ParseAABB(MDLTrimeshNode node, MDLBinaryAABBNode binaryAABB)
    {
        var boundingBox = binaryAABB.BoundingBox; ;
        var face = node.Faces.ElementAtOrDefault(binaryAABB.FaceIndex);
        var leftChild = (binaryAABB.FaceIndex == -1) ? ParseAABB(node, binaryAABB.LeftNode) : null;
        var rightChild = (binaryAABB.FaceIndex == -1) ? ParseAABB(node, binaryAABB.RightNode) : null;

        return new MDLWalkmeshAABBNode(boundingBox, face, leftChild, rightChild);
    }
    private MDLControllerDataOrientation ParseControllerOrientationData(IEnumerable<byte[]> data)
    {
        if (data.Count() == 2)
        {
            // TODO - seems to be inverse of actual value eg. x=1 -> x=-1
            var intData = BitConverter.ToInt32(data.First());
            var x = ((intData & 0x7FF) / 1023.0f) - 1.0f;
            var y = (((intData >> 11) & 0x7FF) / 1023.0f) - 1.0f;
            var z = ((intData >> 22) / 511.0f) - 1.0f;
            var w = 0f;
            float mag2 = x * x + y * y + z * z;
            if (mag2 < 1.0f)
                w = MathF.Sqrt(1.0f - mag2);
            else
                w = 0.0f;
            return new MDLControllerDataOrientation(x, y, z, w);
        }
        else if (data.Count() == 4)
        {
            var floatData = data.Select(x => BitConverter.ToSingle(x)).ToList();
            return new MDLControllerDataOrientation(floatData[0], floatData[1], floatData[2], floatData[3]);
        }
        else
        {
            throw new ArgumentException("Illegal number of columns in orientation controller.");
        }
    }

    public void Unparse(MDL mdl)
    {
        ModelHeader.GeometryHeader.Name = mdl.Name;
        ModelHeader.GeometryHeader.GeometryType = 2;
        ModelHeader.ModelType = mdl.ModelType;
        ModelHeader.Fog = Convert.ToByte(mdl.AffectedByFog);
        ModelHeader.BoundingBoxMin = mdl.BoundingBox.Min;
        ModelHeader.BoundingBoxMax = mdl.BoundingBox.Max;
        ModelHeader.Radius = mdl.Radius;
        ModelHeader.AnimationScale = mdl.AnimationScale;
        ModelHeader.SupermodelName = mdl.SupermodelName;

        Names =
        [
            mdl.Root.Name,
            .. mdl.Root.GetAllAncestors().Select(x => x.Name).ToList(),
            .. mdl.Animations.SelectMany(x => x.RootNode.GetAllAncestors().Select(x => x.Name)),
            .. mdl.Animations.Select(x => x.RootNode.Name),
        ];
        Names = Names.Distinct().ToList();

        Animations = mdl.Animations.Select(x => UnparseAnimation(x)).ToList();

        RootNode = UnparseNode(mdl.Root);

        Recalculate();
    }
    private MDLBinaryAnimation UnparseAnimation(MDLAnimation animation)
    {
        var binaryAnimation = new MDLBinaryAnimation();
        binaryAnimation.AnimationHeader.GeometryHeader.Name = animation.Name;
        binaryAnimation.AnimationHeader.GeometryHeader.GeometryType = 5;
        binaryAnimation.AnimationHeader.AnimationLength = animation.AnimationLength;
        binaryAnimation.AnimationHeader.TransitionTime = animation.TransitionTime;
        binaryAnimation.AnimationHeader.AnimationRoot = animation.AnimationRoot;
        binaryAnimation.RootNode = UnparseNode(animation.RootNode, true);
        binaryAnimation.Events = animation.Events.Select(x => new MDLBinaryAnimationEvent
        {
            ActivationTime = x.ActivationTime,
            Name = x.Name,
        }).ToList();
        return binaryAnimation;
    }
    private MDLBinaryNode UnparseNode(MDLNode node, bool animation = false)
    {
        var binaryNode = new MDLBinaryNode();

        //var positionController = node.Controllers.OfType<MDLControllerDataPosition>().OrderBy(x => x.StartTime).FirstOrDefault();
        //var orientationController = node.Controllers.OfType<MDLControllerDataOrientation>().OrderBy(x => x.StartTime).FirstOrDefault();

        binaryNode.NodeHeader.NodeType = (ushort)MDLBinaryNodeType.NodeFlag;
        binaryNode.NodeHeader.NodeIndex = node.NodeIndex;
        binaryNode.NodeHeader.NameIndex = (ushort)Names.IndexOf(node.Name);
        binaryNode.NodeHeader.Position = new();//new(positionController?.X ?? 0, positionController?.Y ?? 0, positionController?.Z ?? 0);
        binaryNode.NodeHeader.Rotation = new();//new(orientationController?.X ?? 0, orientationController?.Y ?? 0, orientationController?.Z ?? 0, orientationController?.W ?? 0);

        if (node is MDLTrimeshNode trimeshNode)
        {
            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.TrimeshFlag;

            var vertices = trimeshNode.AllVertices().ToList();

            binaryNode.Trimesh = new()
            {
                Faces = trimeshNode.Faces.Select(x => new MDLBinaryTrimeshFace
                {
                    Vertex1 = (short)vertices.IndexOf(x.Vertex1),
                    Vertex2 = (short)vertices.IndexOf(x.Vertex2),
                    Vertex3 = (short)vertices.IndexOf(x.Vertex3),
                }).ToList(),
                VertexIndices = new()
                {
                    trimeshNode.Faces.SelectMany(x => new List<ushort>()
                    {
                        (ushort)vertices.IndexOf(x.Vertex1),
                        (ushort)vertices.IndexOf(x.Vertex2),
                        (ushort)vertices.IndexOf(x.Vertex3),
                    }).ToList(),
                },
                Vertices = vertices.Select(x => x.Position ?? new()).ToList(),
            };

            binaryNode.TrimeshHeader = new()
            {
                FunctionPointer1 = IsTSL ? MDLBinaryTrimeshHeader.K2_NORMAL_FP1 : MDLBinaryTrimeshHeader.K1_NORMAL_FP1,
                FunctionPointer2 = IsTSL ? MDLBinaryTrimeshHeader.K2_NORMAL_FP2 : MDLBinaryTrimeshHeader.K1_NORMAL_FP2,
                BoundingBoxMin = trimeshNode.BoundingBox.Min,
                BoundingBoxMax = trimeshNode.BoundingBox.Max,
                Radius = trimeshNode.Radius,
                AveragePoint = trimeshNode.AveragePoint,
                Diffuse = trimeshNode.Diffuse,
                Ambient = trimeshNode.Ambient,
                TransparencyHint = trimeshNode.TransparencyHint,
                Texture = trimeshNode.DiffuseTexture ?? "",
                Lightmap = trimeshNode.LightmapTexture ?? "",
                AnimateUV = Convert.ToByte(trimeshNode.UVSpeed == 0),
                UVSpeed = trimeshNode.UVSpeed,
                UVJitterSpeed = trimeshNode.UVJitterSpeed,
                UVDirection = trimeshNode.UVDirection,
                VertexCount = (ushort)vertices.Count(),
                TextureCount = (ushort)(Convert.ToUInt16(trimeshNode.DiffuseTexture != "") + Convert.ToUInt16(trimeshNode.LightmapTexture != "")),
                HasLightmap = Convert.ToByte(trimeshNode.LightmapTexture != ""),
                RotateTexture = Convert.ToByte(trimeshNode.RotateTexture),
                BackgroundGeometry = Convert.ToByte(trimeshNode.IsBackgroundGeometry),
                HasShadow = Convert.ToByte(trimeshNode.CastsShadow),
                DoesRender = Convert.ToByte(trimeshNode.Renders),
                TotalArea = trimeshNode.TotalArea,
                DirtEnabled = Convert.ToByte(trimeshNode.DirtEnabled),
                DirtTexture = trimeshNode.DirtTexture,
                DirtCoordinateSpace = trimeshNode.DirtCoordinateSpace,
                HideInHolograms = Convert.ToByte(trimeshNode.HideInHologram),
            };

            binaryNode.MDXVertices = vertices.Select(x => new MDXBinaryVertex
            {
                Position = x.Position,
                Normal = x.Normal,
                UV1 = x.DiffuseUV,
                UV2 = x.LightmapUV,
                Tangent1 = x.Tangent1,
                Tangent2 = x.Tangent2,
                Tangent3 = x.Tangent3,
                Tangent4 = x.Tangent4,
                BoneIndex1 = x.Skin?.WeightIndex1,
                BoneIndex2 = x.Skin?.WeightIndex2,
                BoneIndex3 = x.Skin?.WeightIndex3,
                BoneIndex4 = x.Skin?.WeightIndex4,
                BoneWeight1 = x.Skin?.WeightValue1,
                BoneWeight2 = x.Skin?.WeightValue2,
                BoneWeight3 = x.Skin?.WeightValue3,
                BoneWeight4 = x.Skin?.WeightValue4,
            }).ToList();

            binaryNode.MDXVertices.Add(new()
            {
                Position = trimeshNode.HasPositions() ? new() : null,
                Normal = trimeshNode.HasNormals() ? new() : null,
                UV1 = trimeshNode.HasDiffuseUVs() ? new() : null,
                UV2 = trimeshNode.HasLightmapUVs() ? new() : null,
                Tangent1 = trimeshNode.HasTangents() ? new() : null,
                Tangent2 = trimeshNode.HasTangents() ? new() : null,
                Tangent3 = trimeshNode.HasTangents() ? new() : null,
                Tangent4 = trimeshNode.HasTangents() ? new() : null,
                BoneIndex1 = (node is MDLSkinNode) ? new() : null,
                BoneIndex2 = (node is MDLSkinNode) ? new() : null,
                BoneIndex3 = (node is MDLSkinNode) ? new() : null,
                BoneIndex4 = (node is MDLSkinNode) ? new() : null,
                BoneWeight1 = (node is MDLSkinNode) ? new() : null,
                BoneWeight2 = (node is MDLSkinNode) ? new() : null,
                BoneWeight3 = (node is MDLSkinNode) ? new() : null,
                BoneWeight4 = (node is MDLSkinNode) ? new() : null,
            });
        }
        if (node is MDLDanglyNode danglyNode)
        {
            //binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.DanglyFlag;
        }
        if (node is MDLEmitterNode emitterNode)
        {
            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.EmitterFlag;

            binaryNode.EmitterHeader = new MDLBinaryEmitterHeader
            {
                DeadSpace = emitterNode.DeadSpace,
                BlastRadius = emitterNode.BlastRadius,
                BranchCount = emitterNode.BranchCount,
                ControlPointSmoothing = emitterNode.ControlPointSmoothing,
                XGrid = emitterNode.XGrid,
                YGrid = emitterNode.YGrid,
                SpawnType = emitterNode.SpawnType,
                Update = emitterNode.Update,
                Render = emitterNode.Render,
                Blend = emitterNode.Blend,
                Texture = emitterNode.Texture,
                ChunkName = emitterNode.ChunkName,
                TwoSidedTexture = emitterNode.TwoSidedTexture,
                Loop = emitterNode.Loop,
                RenderOrder = emitterNode.RenderOrder,
                FrameBlending = emitterNode.FrameBlending,
                DepthTextureName = emitterNode.DepthTextureName,
            };

            binaryNode.EmitterHeader.Flags |= emitterNode.P2P ? (uint)MDLBinaryEmitterFlags.P2P : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.P2P_SEL ? (uint)MDLBinaryEmitterFlags.P2P_SEL : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.AffectedByWind ? (uint)MDLBinaryEmitterFlags.AffectedByWind : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Tinted ? (uint)MDLBinaryEmitterFlags.Tinted : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Bounce ? (uint)MDLBinaryEmitterFlags.Bounce : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Random ? (uint)MDLBinaryEmitterFlags.Random : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Inherit ? (uint)MDLBinaryEmitterFlags.Inherit : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.InheritVelocity ? (uint)MDLBinaryEmitterFlags.InheritVelocity : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.InheritLocal ? (uint)MDLBinaryEmitterFlags.InheritLocal : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Splat ? (uint)MDLBinaryEmitterFlags.Splat : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.InheritPart ? (uint)MDLBinaryEmitterFlags.InheritPart : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.DepthTexture ? (uint)MDLBinaryEmitterFlags.DepthTexture : 0;
            binaryNode.EmitterHeader.Flags |= emitterNode.Flag13 ? (uint)MDLBinaryEmitterFlags.Flag13 : 0;
        }
        if (node is MDLLightNode lightNode)
        {
            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.LightFlag;
            binaryNode.LightHeader = new()
            {
                FlareRadius = lightNode.FlareRadius,
                LightPriority = lightNode.LightPriority,
                AmbientOnly = lightNode.AmbientOnly,
                DynamicType = lightNode.DynamicType,
                AffectDynamic = lightNode.AffectDynamic,
                Shadow = lightNode.Shadow,
                Flare = lightNode.Flare,
                FadingLight = lightNode.FadingLight,
            };

            binaryNode.Light = new();
            lightNode.Flares.ForEach(x =>
            {
                binaryNode.Light.FlareSizes.Add(x.Size);
                binaryNode.Light.FlarePositions.Add(x.Position);
                binaryNode.Light.FlareColourShifts.Add(x.ColourShift);
                binaryNode.Light.FlareTextures.Add(x.TextureName);
            });
        }
        if (node is MDLReferenceNode referenceNode)
        {
            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.ReferenceFlag;

            binaryNode.ReferenceHeader = new MDLBinaryReferenceHeader
            {
                ModelResRef = referenceNode.ModelResRef,
                Reattachable = referenceNode.Reattachable,
            };
        }
        if (node is MDLSkinNode skinNode)
        {
            //binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.SkinFlag;
        }
        if (node is MDLWalkmeshNode walkmeshNode)
        {
            //binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.AABBFlag;
        }

        binaryNode.Children = node.Children.Select(x => UnparseNode(x, animation)).ToList();
        binaryNode.ControllerHeaders = node.Controllers.Select(x => UnparseController(x, binaryNode.ControllerData, animation)).ToList();

        return binaryNode;
    }
    private MDLBinaryControllerHeader UnparseController(MDLController<BaseMDLControllerRow<BaseMDLControllerData>> controller, List<byte[]> binaryControllerData, bool animation)
    {
        var binaryControllerHeader = new MDLBinaryControllerHeader();
        binaryControllerHeader.FirstKeyOffset = (short)binaryControllerData.Count();
        binaryControllerHeader.Unknown = -1;
        binaryControllerHeader.RowCount = (short)controller.Rows.Count();
        binaryControllerData.AddRange(controller.Rows.Select(x => BitConverter.GetBytes(x.StartTime)));

        binaryControllerHeader.FirstDataOffset = (short)binaryControllerData.Count();

        Type dataType = null;

        foreach (var row in controller.Rows)
        {
            if (row is MDLControllerRow<BaseMDLControllerData> notBezier)
            {
                UnparseControllerData(notBezier.Data, binaryControllerData);
                dataType = notBezier.Data.GetType();
            }
            else if (row is MDLControllerRowBezier<BaseMDLControllerData> bezier)
            {
                UnparseControllerData(bezier.Data[0], binaryControllerData);
                UnparseControllerData(bezier.Data[1], binaryControllerData);
                UnparseControllerData(bezier.Data[2], binaryControllerData);
                dataType = bezier.Data[0].GetType();
            }
        }

        if (dataType.IsAssignableFrom(typeof(MDLControllerDataAlpha)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Alpha;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterAlphaEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaEnd;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterAlphaMiddle)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaMid;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterAlphaStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaStart;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterBirthRate)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.BirthRate;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterColourEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorEnd;
            binaryControllerHeader.ColumnCount = 3;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterColourMiddle)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorMid;
            binaryControllerHeader.ColumnCount = 3;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterColourStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorStart;
            binaryControllerHeader.ColumnCount = 3;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterCombineTime)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.CombineTime;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterControlPointsCount)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.NumberOfControlPoints;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterControlPointsDelay)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ControlPointDelay;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterControlPointsRadius)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ControlPointRadius;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterDrag)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Drag;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterFPS)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FPS;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterFrameEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FrameEnd;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterFrameStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FrameStart;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterGravity)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Gravity;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLifeExpectancy)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LifeExpectancy;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLightningDelay)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningDelay;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLightningRadius)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningRadius;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLightningScale)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningScale;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLightningSubDiv)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningSubdivide;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterLightningZigZag)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningZigZag;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterMass)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Mass;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterP2PBezier2)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.P2PBezier2;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterP2PBezier3)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.P2PBezier3;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterParticleRotation)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ParticleRotation;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterPercentEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentEnd;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterPercentMiddle)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentMid;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerEmitterPercentStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentStart;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterRandomBirthRate)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.BirthRate;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterRandomVelocity)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.RandomVelocity;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeEnd;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeMiddle)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeMid;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeStart;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeX)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.XSize;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeY)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.YSize;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeYEnd)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeEndY;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSizeYMiddle)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeMidY;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerEmitterSizeYStart)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeStartY;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterSpread)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Spread;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterTangentLength)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TangentLength;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterTangentSpread)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TangentSpread;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterTargetSize)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TargetSize;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterThreshold)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Threshold;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataEmitterVelocity)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Velocity;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataLightColour)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Colour;
            binaryControllerHeader.ColumnCount = 3;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataLightMultiplier)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Multiplier;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataLightRadius)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Radius;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataLightShadowRadius)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ShadowRadius;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataLightVerticalDisplacement)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.VerticalDisplacement;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataOrientation)))
        {
            binaryControllerHeader.Unknown = animation ? (short)28 : (short)-1;
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Orientation;
            binaryControllerHeader.ColumnCount = 4;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataPosition)))
        {
            binaryControllerHeader.Unknown = animation ? (short)16 : (short)-1;
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Position;
            binaryControllerHeader.ColumnCount = 3;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataScale)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Scale;
            binaryControllerHeader.ColumnCount = 1;
        }
        else if (dataType.IsAssignableFrom(typeof(MDLControllerDataMeshSelfIllumination)))
        {
            binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SelfIlluminationColour;
            binaryControllerHeader.ColumnCount = 3;
        }

        return binaryControllerHeader;
    }
    private void UnparseControllerData(BaseMDLControllerData data, List<byte[]> binaryControllerData)
    {
        if (data is MDLControllerDataAlpha alpha)
        {
            binaryControllerData.Add(BitConverter.GetBytes(alpha.Alpha));
        }
        else if (data is MDLControllerDataEmitterAlphaEnd alphaEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(alphaEnd.Value));
        }
        else if (data is MDLControllerDataEmitterAlphaMiddle alphaMiddle)
        {
            binaryControllerData.Add(BitConverter.GetBytes(alphaMiddle.Value));
        }
        else if (data is MDLControllerDataEmitterAlphaStart alphaStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(alphaStart.Value));
        }
        else if (data is MDLControllerDataEmitterBirthRate birthRate)
        {
            binaryControllerData.Add(BitConverter.GetBytes(birthRate.Value));
        }
        else if (data is MDLControllerDataEmitterColourEnd colourEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Red));
            binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Green));
            binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Blue));
        }
        else if (data is MDLControllerDataEmitterColourMiddle colourMiddle)
        {
            binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Red));
            binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Green));
            binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Blue));
        }
        else if (data is MDLControllerDataEmitterColourStart colourStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(colourStart.Red));
            binaryControllerData.Add(BitConverter.GetBytes(colourStart.Green));
            binaryControllerData.Add(BitConverter.GetBytes(colourStart.Blue));
        }
        else if (data is MDLControllerDataEmitterCombineTime combineTime)
        {
            binaryControllerData.Add(BitConverter.GetBytes(combineTime.Value));
        }
        else if (data is MDLControllerDataEmitterControlPointsCount controlPointsCount)
        {
            binaryControllerData.Add(BitConverter.GetBytes(controlPointsCount.Value));
        }
        else if (data is MDLControllerDataEmitterControlPointsDelay controlPointsDelay)
        {
            binaryControllerData.Add(BitConverter.GetBytes(controlPointsDelay.Value));
        }
        else if (data is MDLControllerDataEmitterControlPointsRadius controlPointsRadius)
        {
            binaryControllerData.Add(BitConverter.GetBytes(controlPointsRadius.Value));
        }
        else if (data is MDLControllerDataEmitterDrag drag)
        {
            binaryControllerData.Add(BitConverter.GetBytes(drag.Value));
        }
        else if (data is MDLControllerDataEmitterFPS fps)
        {
            binaryControllerData.Add(BitConverter.GetBytes(fps.FramesPerSecond));
        }
        else if (data is MDLControllerDataEmitterFrameEnd frameEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(frameEnd.Value));
        }
        else if (data is MDLControllerDataEmitterFrameStart frameStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(frameStart.Value));
        }
        else if (data is MDLControllerDataEmitterGravity gravity)
        {
            binaryControllerData.Add(BitConverter.GetBytes(gravity.Value));
        }
        else if (data is MDLControllerDataEmitterLifeExpectancy lifeExpectancy)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lifeExpectancy.Time));
        }
        else if (data is MDLControllerDataEmitterLightningDelay lightningDelay)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightningDelay.Value));
        }
        else if (data is MDLControllerDataEmitterLightningRadius lightningRadius)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightningRadius.Value));
        }
        else if (data is MDLControllerDataEmitterLightningScale lightningScale)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightningScale.Value));
        }
        else if (data is MDLControllerDataEmitterLightningSubDiv lightningSubDiv)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightningSubDiv.Value));
        }
        else if (data is MDLControllerDataEmitterLightningZigZag lightningZigZag)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightningZigZag.Value));
        }
        else if (data is MDLControllerDataEmitterMass mass)
        {
            binaryControllerData.Add(BitConverter.GetBytes(mass.Speed));
        }
        else if (data is MDLControllerDataEmitterP2PBezier2 bezier2)
        {
            binaryControllerData.Add(BitConverter.GetBytes(bezier2.Value));
        }
        else if (data is MDLControllerDataEmitterP2PBezier3 bezier3)
        {
            binaryControllerData.Add(BitConverter.GetBytes(bezier3.Value));
        }
        else if (data is MDLControllerDataEmitterParticleRotation rotation)
        {
            binaryControllerData.Add(BitConverter.GetBytes(rotation.Rotation));
        }
        else if (data is MDLControllerDataEmitterPercentEnd percentEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(percentEnd.Value));
        }
        else if (data is MDLControllerDataEmitterPercentMiddle percentMiddle)
        {
            binaryControllerData.Add(BitConverter.GetBytes(percentMiddle.Value));
        }
        else if (data is MDLControllerEmitterPercentStart percentStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(percentStart.Value));
        }
        else if (data is MDLControllerDataEmitterRandomBirthRate emitterBirthRate)
        {
            binaryControllerData.Add(BitConverter.GetBytes(emitterBirthRate.Value));
        }
        else if (data is MDLControllerDataEmitterRandomVelocity emitterRandomVelocity)
        {
            binaryControllerData.Add(BitConverter.GetBytes(emitterRandomVelocity.Speed));
        }
        else if (data is MDLControllerDataEmitterSizeEnd sizeEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeEnd.Value));
        }
        else if (data is MDLControllerDataEmitterSizeMiddle sizeMiddle)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeMiddle.Value));
        }
        else if (data is MDLControllerDataEmitterSizeStart sizeStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeStart.Value));
        }
        else if (data is MDLControllerDataEmitterSizeX sizeX)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeX.SizeX));
        }
        else if (data is MDLControllerDataEmitterSizeY sizeY)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeY.SizeY));
        }
        else if (data is MDLControllerDataEmitterSizeYEnd sizeYEnd)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeYEnd.Value));
        }
        else if (data is MDLControllerDataEmitterSizeYMiddle sizeYMiddle)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeYMiddle.Value));
        }
        else if (data is MDLControllerEmitterSizeYStart sizeYStart)
        {
            binaryControllerData.Add(BitConverter.GetBytes(sizeYStart.Value));
        }
        else if (data is MDLControllerDataEmitterSpread spread)
        {
            binaryControllerData.Add(BitConverter.GetBytes(spread.Spread));
        }
        else if (data is MDLControllerDataEmitterTangentLength tangentLength)
        {
            binaryControllerData.Add(BitConverter.GetBytes(tangentLength.Value));
        }
        else if (data is MDLControllerDataEmitterTangentSpread tangentSpread)
        {
            binaryControllerData.Add(BitConverter.GetBytes(tangentSpread.Value));
        }
        else if (data is MDLControllerDataEmitterTargetSize targetSize)
        {
            binaryControllerData.Add(BitConverter.GetBytes(targetSize.Value));
        }
        else if (data is MDLControllerDataEmitterThreshold threshold)
        {
            binaryControllerData.Add(BitConverter.GetBytes(threshold.Value));
        }
        else if (data is MDLControllerDataEmitterVelocity emitterVelocity)
        {
            binaryControllerData.Add(BitConverter.GetBytes(emitterVelocity.Speed));
        }
        else if (data is MDLControllerDataLightColour lightColour)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightColour.Red));
            binaryControllerData.Add(BitConverter.GetBytes(lightColour.Green));
            binaryControllerData.Add(BitConverter.GetBytes(lightColour.Blue));
        }
        else if (data is MDLControllerDataLightMultiplier lightMultiplier)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightMultiplier.Multiplier));
        }
        else if (data is MDLControllerDataLightRadius lightRadius)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightRadius.Radius));
        }
        else if (data is MDLControllerDataLightShadowRadius lightShadowRadius)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightShadowRadius.Radius));
        }
        else if (data is MDLControllerDataLightVerticalDisplacement lightVerticalDisplacement)
        {
            binaryControllerData.Add(BitConverter.GetBytes(lightVerticalDisplacement.Displacement));
        }
        else if (data is MDLControllerDataOrientation orientation)
        {
            binaryControllerData.Add(BitConverter.GetBytes(orientation.X));
            binaryControllerData.Add(BitConverter.GetBytes(orientation.Y));
            binaryControllerData.Add(BitConverter.GetBytes(orientation.Z));
            binaryControllerData.Add(BitConverter.GetBytes(orientation.W));
        }
        else if (data is MDLControllerDataPosition position)
        {
            binaryControllerData.Add(BitConverter.GetBytes(position.X));
            binaryControllerData.Add(BitConverter.GetBytes(position.Y));
            binaryControllerData.Add(BitConverter.GetBytes(position.Z));
        }
        else if (data is MDLControllerDataScale scale)
        {
            binaryControllerData.Add(BitConverter.GetBytes(scale.Scale));
        }
        else if (data is MDLControllerDataMeshSelfIllumination selfIllumination)
        {
            binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Red));
            binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Green));
            binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Blue));
        }
    }
}
