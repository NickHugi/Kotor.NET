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
        ModelHeader.ChildModelCount = ModelHeader.GeometryHeader.NodeCount; // TODO - plus supermodel?
        ModelHeader.NamesArrayCount = Names.Count();
        ModelHeader.NamesArrayCount2 = Names.Count();
        //ModelHeader.AnimationCount = Animations.Count();
        //ModelHeader.AnimationCount2 = Animations.Count();

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
        mdl.Supermodel = ModelHeader.SupermodelName;

        Animations.Select(x => ParseAnimation(x));
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

            if (vertexBitmap.HasFlag(MDLBinaryMDXVertexBitmask.Vertices))
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

            binaryNode.TrimeshFaces.ForEach(x =>
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

        node.Controllers = binaryNode.ControllerHeaders.SelectMany(x => ParseController(binaryNode, x)).ToList();
        node.Children = binaryNode.Children.Select(x => ParseNode(x)).ToList();
        return node;
    }
    private IEnumerable<BaseMDLController> ParseController(MDLBinaryNode binaryNode, MDLBinaryControllerHeader binaryController)
    {
        var nodeType = (MDLBinaryNodeType)binaryNode.NodeHeader.NodeType;
        var controllerType = (MDLBinaryControllerType)binaryController.ControllerType;

        var data = binaryNode.ControllerData
            .Skip(binaryController.FirstDataOffset)
            .Take(binaryController.ColumnCount)
            .ToList();

        var times = binaryNode.ControllerData
            .Skip(binaryController.FirstKeyOffset)
            .Take(binaryController.RowCount * binaryController.ColumnCount)
            .Select(x => BitConverter.ToSingle(x))
            .ToList();

        return Enumerable.Range(0, binaryController.RowCount)
            .Select((x, index) => ParseControllerData(nodeType, controllerType, times, data.Skip(binaryController.RowCount * index).ToList()));
    }
    private BaseMDLController ParseControllerData(MDLBinaryNodeType nodeType, MDLBinaryControllerType controllerType, List<float> times, List<byte[]> data)
    {
        var floatData = data.Select(x => BitConverter.ToSingle(x)).ToList();

        if ((nodeType & MDLBinaryNodeType.TrimeshFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.Scale => new MDLControllerScale(floatData[0]),
                MDLBinaryControllerType.SelfIlluminationColour => new MDLControllerSelfIllumination(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Alpha => new MDLControllerAlpha(floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
        else if ((nodeType & MDLBinaryNodeType.LightFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.Colour => new MDLControllerLightColour(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Radius => new MDLControllerLightRadius(floatData[0]),
                MDLBinaryControllerType.ShadowRadius => new MDLControllerLightShadowRadius(floatData[0]),
                MDLBinaryControllerType.VerticalDisplacement => new MDLControllerLightVerticalDisplacement(floatData[0]),
                MDLBinaryControllerType.Multiplier => new MDLControllerLightMultiplier(floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
        else if ((nodeType & MDLBinaryNodeType.EmitterFlag) > 0)
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.AlphaEnd => new MDLControllerEmitterAlphaEnd(floatData[0]),
                MDLBinaryControllerType.AlphaMid => new MDLControllerEmitterAlphaMiddle(floatData[0]),
                MDLBinaryControllerType.AlphaStart => new MDLControllerEmitterAlphaStart(floatData[0]),
                MDLBinaryControllerType.BirthRate => new MDLControllerEmitterBirthRate(floatData[0]),
                MDLBinaryControllerType.BounceCo => new MDLControllerEmitterBounceCo(floatData[0]),
                MDLBinaryControllerType.CombineTime => new MDLControllerEmitterCombineTime(floatData[0]),
                MDLBinaryControllerType.Drag => new MDLControllerEmitterDrag(floatData[0]),
                MDLBinaryControllerType.FPS => new MDLControllerEmitterFPS(floatData[0]),
                MDLBinaryControllerType.FrameStart => new MDLControllerEmitterFrameStart(floatData[0]),
                MDLBinaryControllerType.FrameEnd => new MDLControllerEmitterFrameEnd(floatData[0]),
                MDLBinaryControllerType.Gravity => new MDLControllerEmitterGravity(floatData[0]),
                MDLBinaryControllerType.LifeExpectancy => new MDLControllerEmitterLifeExpectancy(floatData[0]),
                MDLBinaryControllerType.Mass => new MDLControllerEmitterMass(floatData[0]),
                MDLBinaryControllerType.P2PBezier2 => new MDLControllerEmitterP2PBezier2(floatData[0]),
                MDLBinaryControllerType.P2PBezier3 => new MDLControllerEmitterP2PBezier3(floatData[0]),
                MDLBinaryControllerType.ParticleRotation => new MDLControllerEmitterParticleRotation(floatData[0]),
                MDLBinaryControllerType.RandomVelocity => new MDLControllerEmitterRandomVelocity(floatData[0]),
                MDLBinaryControllerType.SizeStart => new MDLControllerEmitterSizeStart(floatData[0]),
                MDLBinaryControllerType.SizeStartY => new MDLControllerEmitterSizeYStart(floatData[0]),
                MDLBinaryControllerType.SizeMid => new MDLControllerEmitterSizeMiddle(floatData[0]),
                MDLBinaryControllerType.SizeMidY => new MDLControllerEmitterSizeYMiddle(floatData[0]),
                MDLBinaryControllerType.SizeEnd => new MDLControllerEmitterSizeEnd(floatData[0]),
                MDLBinaryControllerType.SizeEndY => new MDLControllerEmitterSizeYEnd(floatData[0]),
                MDLBinaryControllerType.Spread => new MDLControllerEmitterSpread(floatData[0]),
                MDLBinaryControllerType.Threshold => new MDLControllerEmitterThreshold(floatData[0]),
                MDLBinaryControllerType.Velocity => new MDLControllerEmitterVelocity(floatData[0]),
                MDLBinaryControllerType.XSize => new MDLControllerEmitterSizeX(floatData[0]),
                MDLBinaryControllerType.YSize => new MDLControllerEmitterSizeY(floatData[0]),
                MDLBinaryControllerType.BlurLength => new MDLControllerEmitterBlurLength(floatData[0]),
                MDLBinaryControllerType.LightningDelay => new MDLControllerEmitterLightningDelay(floatData[0]),
                MDLBinaryControllerType.LightningRadius => new MDLControllerEmitterLightningRadius(floatData[0]),
                MDLBinaryControllerType.LightningScale => new MDLControllerEmitterLightningScale(floatData[0]),
                MDLBinaryControllerType.LightningSubdivide => new MDLControllerEmitterLightningSubDiv(floatData[0]),
                MDLBinaryControllerType.LightningZigZag => new MDLControllerEmitterLightningZigZag(floatData[0]),
                MDLBinaryControllerType.PercentStart => new MDLControllerEmitterPercentStart(floatData[0]),
                MDLBinaryControllerType.PercentMid => new MDLControllerEmitterPercentMiddle(floatData[0]),
                MDLBinaryControllerType.PercentEnd => new MDLControllerEmitterPercentEnd(floatData[0]),
                MDLBinaryControllerType.RandomBirthRate => new MDLControllerEmitterRandomBirthRate(floatData[0]),
                MDLBinaryControllerType.TargetSize => new MDLControllerEmitterTargetSize(floatData[0]),
                MDLBinaryControllerType.NumberOfControlPoints => new MDLControllerEmitterControlPointsCount(floatData[0]),
                MDLBinaryControllerType.ControlPointRadius => new MDLControllerEmitterControlPointsRadius(floatData[0]),
                MDLBinaryControllerType.ControlPointDelay => new MDLControllerEmitterControlPointsDelay(floatData[0]),
                MDLBinaryControllerType.TangentSpread => new MDLControllerEmitterTangentSpread(floatData[0]),
                MDLBinaryControllerType.TangentLength => new MDLControllerEmitterTangentLength(floatData[0]),
                MDLBinaryControllerType.ColorStart => new MDLControllerEmitterColourStart(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.ColorMid => new MDLControllerEmitterColourMiddle(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.ColorEnd => new MDLControllerEmitterColourEnd(floatData[0], floatData[1], floatData[2]),
                //MDLBinaryControllerType.EmitterDetonate => new (floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
        else
        {
            return controllerType switch
            {
                MDLBinaryControllerType.Position => new MDLControllerPosition(floatData[0], floatData[1], floatData[2]),
                MDLBinaryControllerType.Orientation => new MDLControllerOrientation(floatData[0], floatData[1], floatData[2], floatData[3]),
                MDLBinaryControllerType.Scale => new MDLControllerScale(floatData[0]),
                _ => throw new ArgumentException("Unknown Controller Type"),
            };
        }
    }
    private MDLWalkmeshAABBNode ParseAABB(MDLTrimeshNode node, MDLBinaryAABBNode binaryAABB)
    {
        var boundingBox = binaryAABB.BoundingBox; ;
        var face = node.Faces.ElementAtOrDefault(binaryAABB.FaceIndex);
        var leftChild = (binaryAABB.FaceIndex == -1) ? ParseAABB(node, binaryAABB.LeftNode) : null;
        var rightChild = (binaryAABB.FaceIndex == -1) ? ParseAABB(node, binaryAABB.RightNode) : null;

        return new MDLWalkmeshAABBNode(boundingBox, face, leftChild, rightChild);
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
        ModelHeader.SupermodelName = mdl.Supermodel;

        Names = mdl.Root.GetAllAncestors().Select(x => x.Name).ToList();
        Names.Add(mdl.Root.Name);
        Names.AddRange(mdl.Animations.SelectMany(x => x.RootNode.GetAllAncestors().Select(x => x.Name)));
        Names.AddRange(mdl.Animations.Select(x => x.RootNode.Name));
        Names = Names.Distinct().ToList();

        //Animations = mdl.Animations.Select(x => UnparseAnimation(x)).ToList();

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
        binaryAnimation.RootNode = UnparseNode(animation.RootNode);
        binaryAnimation.Events = animation.Events.Select(x => new MDLBinaryAnimationEvent
        {
            ActivationTime = x.ActivationTime,
            Name = x.Name,
        }).ToList();
        return binaryAnimation;
    }
    private MDLBinaryNode UnparseNode(MDLNode node)
    {
        var binaryNode = new MDLBinaryNode();

        var positionController = node.Controllers.OfType<MDLControllerPosition>().OrderBy(x => x.StartTime).FirstOrDefault();
        var orientationController = node.Controllers.OfType<MDLControllerOrientation>().OrderBy(x => x.StartTime).FirstOrDefault();

        binaryNode.NodeHeader.NodeType = (ushort)MDLBinaryNodeType.NodeFlag;
        binaryNode.NodeHeader.NodeIndex = (ushort)Names.IndexOf(node.Name); // TODO
        binaryNode.NodeHeader.NameIndex = (ushort)Names.IndexOf(node.Name);
        binaryNode.NodeHeader.Position = new(positionController?.X ?? 0, positionController?.Y ?? 0, positionController?.Z ?? 0);
        binaryNode.NodeHeader.Rotation = new(orientationController?.X ?? 0, orientationController?.Y ?? 0, orientationController?.Z ?? 0, orientationController?.W ?? 0);

        binaryNode.Children = node.Children.Select(UnparseNode).ToList();
        binaryNode.ControllerHeaders = node.Controllers.GroupBy(x => x.GetType()).Select(x => UnparseController(x.ToList(), binaryNode.ControllerData)).ToList();

        return binaryNode;
    }
    private MDLBinaryControllerHeader UnparseController(List<BaseMDLController> controllers, List<byte[]> binaryControllerData)
    {
        var binaryControllerHeader = new MDLBinaryControllerHeader();
        binaryControllerHeader.FirstKeyOffset = (short)binaryControllerData.Count();
        binaryControllerHeader.Unknown = -1;
        binaryControllerHeader.RowCount = (short)controllers.Count();

        binaryControllerData.AddRange(controllers.Select(x => BitConverter.GetBytes(x.StartTime)));

        binaryControllerHeader.FirstDataOffset = (short)binaryControllerData.Count();
        foreach (var controller in controllers)
        {
            if (controller is MDLControllerAlpha alpha)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Alpha;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(alpha.Alpha));
            }
            else if (controller is MDLControllerEmitterAlphaEnd alphaEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaEnd;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(alphaEnd.Value));
            }
            else if (controller is MDLControllerEmitterAlphaMiddle alphaMiddle)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaMid;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(alphaMiddle.Value));
            }
            else if (controller is MDLControllerEmitterAlphaStart alphaStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.AlphaStart;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(alphaStart.Value));
            }
            else if (controller is MDLControllerEmitterBirthRate birthRate)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.BirthRate;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(birthRate.Value));
            }
            else if (controller is MDLControllerEmitterColourEnd colourEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorEnd;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Red));
                binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Green));
                binaryControllerData.Add(BitConverter.GetBytes(colourEnd.Blue));
            }
            else if (controller is MDLControllerEmitterColourMiddle colourMiddle)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorMid;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Red));
                binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Green));
                binaryControllerData.Add(BitConverter.GetBytes(colourMiddle.Blue));
            }
            else if (controller is MDLControllerEmitterColourStart colourStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ColorStart;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(colourStart.Red));
                binaryControllerData.Add(BitConverter.GetBytes(colourStart.Green));
                binaryControllerData.Add(BitConverter.GetBytes(colourStart.Blue));
            }
            else if (controller is MDLControllerEmitterCombineTime combineTime)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.CombineTime;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(combineTime.Value));
            }
            else if (controller is MDLControllerEmitterControlPointsCount controlPointsCount)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.NumberOfControlPoints;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(controlPointsCount.Value));
            }
            else if (controller is MDLControllerEmitterControlPointsDelay controlPointsDelay)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ControlPointDelay;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(controlPointsDelay.Value));
            }
            else if (controller is MDLControllerEmitterControlPointsRadius controlPointsRadius)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ControlPointRadius;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(controlPointsRadius.Value));
            }
            else if (controller is MDLControllerEmitterDrag drag)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Drag;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(drag.Value));
            }
            else if (controller is MDLControllerEmitterFPS fps)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FPS;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(fps.FramesPerSecond));
            }
            else if (controller is MDLControllerEmitterFrameEnd frameEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FrameEnd;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(frameEnd.Value));
            }
            else if (controller is MDLControllerEmitterFrameStart frameStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.FrameStart;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(frameStart.Value));
            }
            else if (controller is MDLControllerEmitterGravity gravity)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Gravity;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(gravity.Value));
            }
            else if (controller is MDLControllerEmitterLifeExpectancy lifeExpectancy)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LifeExpectancy;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lifeExpectancy.Time));
            }
            else if (controller is MDLControllerEmitterLightningDelay lightningDelay)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningDelay;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightningDelay.Value));
            }
            else if (controller is MDLControllerEmitterLightningRadius lightningRadius)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningRadius;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightningRadius.Value));
            }
            else if (controller is MDLControllerEmitterLightningScale lightningScale)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningScale;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightningScale.Value));
            }
            else if (controller is MDLControllerEmitterLightningSubDiv lightningSubDiv)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningSubdivide;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightningSubDiv.Value));
            }
            else if (controller is MDLControllerEmitterLightningZigZag lightningZigZag)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.LightningZigZag;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightningZigZag.Value));
            }
            else if (controller is MDLControllerEmitterMass mass)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Mass;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(mass.Speed));
            }
            else if (controller is MDLControllerEmitterP2PBezier2 bezier2)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.P2PBezier2;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(bezier2.Value));
            }
            else if (controller is MDLControllerEmitterP2PBezier3 bezier3)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.P2PBezier3;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(bezier3.Value));
            }
            else if (controller is MDLControllerEmitterParticleRotation rotation)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ParticleRotation;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(rotation.Rotation));
            }
            else if (controller is MDLControllerEmitterPercentEnd percentEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentEnd;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(percentEnd.Value));
            }
            else if (controller is MDLControllerEmitterPercentMiddle percentMiddle)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentMid;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(percentMiddle.Value));
            }
            else if (controller is MDLControllerEmitterPercentStart percentStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.PercentStart;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(percentStart.Value));
            }
            else if (controller is MDLControllerEmitterRandomBirthRate emitterBirthRate)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.BirthRate;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(emitterBirthRate.Value));
            }
            else if (controller is MDLControllerEmitterRandomVelocity emitterRandomVelocity)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.RandomVelocity;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(emitterRandomVelocity.Speed));
            }
            else if (controller is MDLControllerEmitterSizeEnd sizeEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeEnd;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeEnd.Value));
            }
            else if (controller is MDLControllerEmitterSizeMiddle sizeMiddle)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeMid;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeMiddle.Value));
            }
            else if (controller is MDLControllerEmitterSizeStart sizeStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeStart;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeStart.Value));
            }
            else if (controller is MDLControllerEmitterSizeX sizeX)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.XSize;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeX.SizeX));
            }
            else if (controller is MDLControllerEmitterSizeY sizeY)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.YSize;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeY.SizeY));
            }
            else if (controller is MDLControllerEmitterSizeYEnd sizeYEnd)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeEndY;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeYEnd.Value));
            }
            else if (controller is MDLControllerEmitterSizeYMiddle sizeYMiddle)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeMidY;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeYMiddle.Value));
            }
            else if (controller is MDLControllerEmitterSizeYStart sizeYStart)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SizeStartY;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(sizeYStart.Value));
            }
            else if (controller is MDLControllerEmitterSpread spread)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Spread;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(spread.Spread));
            }
            else if (controller is MDLControllerEmitterTangentLength tangentLength)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TangentLength;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(tangentLength.Value));
            }
            else if (controller is MDLControllerEmitterTangentSpread tangentSpread)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TangentSpread;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(tangentSpread.Value));
            }
            else if (controller is MDLControllerEmitterTargetSize targetSize)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.TargetSize;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(targetSize.Value));
            }
            else if (controller is MDLControllerEmitterThreshold threshold)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Threshold;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(threshold.Value));
            }
            else if (controller is MDLControllerEmitterVelocity emitterVelocity)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Velocity;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(emitterVelocity.Speed));
            }
            else if (controller is MDLControllerLightColour lightColour)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Colour;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(lightColour.Red));
                binaryControllerData.Add(BitConverter.GetBytes(lightColour.Green));
                binaryControllerData.Add(BitConverter.GetBytes(lightColour.Blue));
            }
            else if (controller is MDLControllerLightMultiplier lightMultiplier)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Multiplier;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightMultiplier.Multiplier));
            }
            else if (controller is MDLControllerLightRadius lightRadius)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Radius;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightRadius.Radius));
            }
            else if (controller is MDLControllerLightShadowRadius lightShadowRadius)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.ShadowRadius;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightShadowRadius.Radius));
            }
            else if (controller is MDLControllerLightVerticalDisplacement lightVerticalDisplacement)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.VerticalDisplacement;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(lightVerticalDisplacement.Displacement));
            }
            else if (controller is MDLControllerOrientation orientation)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Orientation;
                binaryControllerHeader.ColumnCount = 4;
                binaryControllerData.Add(BitConverter.GetBytes(orientation.X));
                binaryControllerData.Add(BitConverter.GetBytes(orientation.Y));
                binaryControllerData.Add(BitConverter.GetBytes(orientation.Z));
                binaryControllerData.Add(BitConverter.GetBytes(orientation.W));
            }
            else if (controller is MDLControllerPosition position)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Position;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(position.X));
                binaryControllerData.Add(BitConverter.GetBytes(position.Y));
                binaryControllerData.Add(BitConverter.GetBytes(position.Z));
            }
            else if (controller is MDLControllerScale scale)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.Scale;
                binaryControllerHeader.ColumnCount = 1;
                binaryControllerData.Add(BitConverter.GetBytes(scale.Scale));
            }
            else if (controller is MDLControllerSelfIllumination selfIllumination)
            {
                binaryControllerHeader.ControllerType = (int)MDLBinaryControllerType.SelfIlluminationColour;
                binaryControllerHeader.ColumnCount = 3;
                binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Red));
                binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Green));
                binaryControllerData.Add(BitConverter.GetBytes(selfIllumination.Blue));
            }
        }

        return binaryControllerHeader;
    }
}
