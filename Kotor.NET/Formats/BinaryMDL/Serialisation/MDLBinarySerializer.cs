using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Resources.KotorMDL;
using System.Security.Cryptography;
using Kotor.NET.Formats.BinaryLIP;

namespace Kotor.NET.Formats.BinaryMDL.Serialisation;

public class MDLBinarySerializer
{
    private MDL _mdl { get; }
    private List<string> names { get; set; }
    private bool _isTSL { get; set; }

    public MDLBinarySerializer(MDL mdl)
    {
        _mdl = mdl;
    }

    public MDLBinary Serialize(bool isTSL)
    {
        var binary = new MDLBinary();
        _isTSL = isTSL;

        binary.ModelHeader.GeometryHeader.Name = _mdl.Name;
        binary.ModelHeader.GeometryHeader.GeometryType = 2;
        binary.ModelHeader.ModelType = _mdl.ModelType;
        binary.ModelHeader.Fog = Convert.ToByte(_mdl.AffectedByFog);
        binary.ModelHeader.BoundingBoxMin = _mdl.BoundingBox.Min;
        binary.ModelHeader.BoundingBoxMax = _mdl.BoundingBox.Max;
        binary.ModelHeader.Radius = _mdl.Radius;
        binary.ModelHeader.AnimationScale = _mdl.AnimationScale;
        binary.ModelHeader.SupermodelName = _mdl.SupermodelName;

        names =
        [
            _mdl.Root.Name,
            .. _mdl.Root.GetAllAncestors().Select(x => x.Name).ToList(),
            .. _mdl.Animations.SelectMany(x => x.RootNode.GetAllAncestors().Select(x => x.Name)),
            .. _mdl.Animations.Select(x => x.RootNode.Name),
        ];
        binary.Names = names.Distinct().ToList();

        binary.Animations = _mdl.Animations.Select(SerializeAnimation).ToList();

        binary.RootNode = SerializeNode(_mdl.Root);

        binary.Recalculate();

        return binary;
    }

    private MDLBinaryAnimation SerializeAnimation(MDLAnimation animation)
    {
        var binaryAnimation = new MDLBinaryAnimation();
        binaryAnimation.AnimationHeader.GeometryHeader.Name = animation.Name;
        binaryAnimation.AnimationHeader.GeometryHeader.GeometryType = 5;
        binaryAnimation.AnimationHeader.AnimationLength = animation.AnimationLength;
        binaryAnimation.AnimationHeader.TransitionTime = animation.TransitionTime;
        binaryAnimation.AnimationHeader.AnimationRoot = animation.AnimationRoot;
        binaryAnimation.RootNode = SerializeNode(animation.RootNode, true);
        binaryAnimation.Events = animation.Events.Select(x => new MDLBinaryAnimationEvent
        {
            ActivationTime = x.ActivationTime,
            Name = x.Name,
        }).ToList();
        return binaryAnimation;
    }
    private MDLBinaryNode SerializeNode(MDLNode node, bool animation = false)
    {
        var binaryNode = new MDLBinaryNode();

        var positionController = node.GetController<MDLControllerDataPosition>();
        var orientationController = node.GetController<MDLControllerDataOrientation>();

        binaryNode.NodeHeader.NodeType = (ushort)MDLBinaryNodeType.NodeFlag;
        binaryNode.NodeHeader.NodeIndex = node.NodeIndex;
        binaryNode.NodeHeader.NameIndex = (ushort)names.IndexOf(node.Name);
        binaryNode.NodeHeader.Position = positionController.IsEmpty ? new() : positionController.First().Data[0].ToVector3();
        binaryNode.NodeHeader.Orientation = orientationController.IsEmpty ? new(0, 0, 0, 1) : orientationController.First().Data[0].ToVector4();

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
                InvertedCounters = [trimeshNode.InvertedCounter],
            };

            binaryNode.TrimeshHeader = new()
            {
                FunctionPointer1 = _isTSL ? MDLBinaryTrimeshHeader.K2_NORMAL_FP1 : MDLBinaryTrimeshHeader.K1_NORMAL_FP1,
                FunctionPointer2 = _isTSL ? MDLBinaryTrimeshHeader.K2_NORMAL_FP2 : MDLBinaryTrimeshHeader.K1_NORMAL_FP2,
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
            trimeshNode.SaberValues.CopyTo(binaryNode.TrimeshHeader.UnknownSaberValues, 0);

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

            // TODO - accurate mdx
            binaryNode.MDXVertices.Add(new()
            {
                Position = trimeshNode.HasPositions() ? new(1e7f, 1e7f, 1e7f) : null,
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
            var vertices = danglyNode.AllVertices().ToList();

            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.DanglyFlag;

            binaryNode.DanglymeshHeader = new()
            {
                Period = danglyNode.Period,
                Tightness = danglyNode.Tightness,
                Displacement = danglyNode.Displacement,
            };
            binaryNode.Danglymesh = new()
            {
                Constraints = vertices.Select(x => x.Dangly.Constraint).ToList(),
                Data = vertices.Select(x => x.Dangly.Unknown).ToList(),
            };
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
            binaryNode.NodeHeader.NodeType |= (ushort)MDLBinaryNodeType.AABBFlag;
            binaryNode.WalkmeshHeader = new();
            binaryNode.RootAABBNode = SerializeAABBNode(walkmeshNode, walkmeshNode.RootNode);
        }

        binaryNode.Children = node.Children.Select(x => SerializeNode(x, animation)).ToList();
        binaryNode.ControllerHeaders = node.GetControllers().Select(x => SerializeController(x, binaryNode.ControllerData, animation)).ToList();

        return binaryNode;
    }
    private MDLBinaryControllerHeader SerializeController(MDLController controller, List<byte[]> binaryControllerData, bool animation)
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
            if (row.IsLinear)
            {
                SerializeControllerData(row.Data.First(), binaryControllerData);
                dataType = controller.Type;
            }
            else if (row.IsBezier)
            {
                SerializeControllerData(row.Data[0], binaryControllerData);
                SerializeControllerData(row.Data[1], binaryControllerData);
                SerializeControllerData(row.Data[2], binaryControllerData);
                dataType = controller.Type;
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
    private void SerializeControllerData(BaseMDLControllerData data, List<byte[]> binaryControllerData)
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
    private MDLBinaryAABBNode SerializeAABBNode(MDLWalkmeshNode walkmeshNode, MDLWalkmeshAABBNode aabbNode)
    {
        return new MDLBinaryAABBNode()
        {
            BoundingBox = aabbNode.BoundingBox,
            FaceIndex = (aabbNode.Face is null) ? -1 : walkmeshNode.Faces.IndexOf(aabbNode.Face),
            MostSiginificantPlane = (int)aabbNode.MostSignificantPlane,
            LeftNode = (aabbNode.LeftChild is null) ? null : SerializeAABBNode(walkmeshNode, aabbNode.LeftChild),
            RightNode = (aabbNode.RightChild is null) ? null : SerializeAABBNode(walkmeshNode, aabbNode.RightChild),
        };
    }
}
