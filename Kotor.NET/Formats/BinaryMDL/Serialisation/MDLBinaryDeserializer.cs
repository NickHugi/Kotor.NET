using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Resources.KotorMDL.VertexData;
using Kotor.NET.Resources.KotorMDL;

namespace Kotor.NET.Formats.BinaryMDL.Serialisation;

public class MDLBinaryDeserializer
{
    private MDLBinary _binary { get; }

    public MDLBinaryDeserializer(MDLBinary binary)
    {
        _binary = binary;
    }

    public MDL Deserialize()
    {
        var mdl = new MDL();

        mdl.Name = _binary.ModelHeader.GeometryHeader.Name;
        mdl.ModelType = _binary.ModelHeader.ModelType;
        mdl.AffectedByFog = _binary.ModelHeader.Fog > 0;
        mdl.AnimationScale = _binary.ModelHeader.AnimationScale;
        mdl.SupermodelName = _binary.ModelHeader.SupermodelName;
        mdl.BoundingBox = new(_binary.ModelHeader.BoundingBoxMin, _binary.ModelHeader.BoundingBoxMax);

        mdl.Animations = _binary.Animations.Select(x => DeserializeAnimation(x)).ToList();
        mdl.Root = DeserializeNode(_binary.RootNode);

        return mdl;
    }

    private MDLAnimation DeserializeAnimation(MDLBinaryAnimation binaryAnim)
    {
        var animation = new MDLAnimation();
        animation.Name = binaryAnim.AnimationHeader.GeometryHeader.Name;
        animation.AnimationRoot = binaryAnim.AnimationHeader.AnimationRoot;
        animation.AnimationLength = binaryAnim.AnimationHeader.AnimationLength;
        animation.TransitionTime = binaryAnim.AnimationHeader.TransitionTime;
        animation.RootNode = DeserializeNode(binaryAnim.RootNode);
        animation.Events = binaryAnim.Events.Select(x => new MDLAnimationEvent()
        {
            Name = x.Name,
            ActivationTime = x.ActivationTime,
        }).ToList();
        return animation;
    }
    private MDLNode DeserializeNode(MDLBinaryNode binaryNode)
    {
        var name = _binary.Names[binaryNode.NodeHeader.NameIndex];
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
            trimeshNode.InvertedCounter = binaryNode.Trimesh.InvertedCounters.FirstOrDefault();
            binaryNode.TrimeshHeader.UnknownSaberValues.CopyTo(trimeshNode.SaberValues, 0);

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
                walkmeshNode.RootNode = DeserializeAABB(walkmeshNode, binaryNode.RootAABBNode);
            }
        }

        node._controllerRows.AddRange(binaryNode.ControllerHeaders.SelectMany(x => DeserializeController(binaryNode, x)).ToList());
        node.Children = binaryNode.Children.Select(x => DeserializeNode(x)).ToList();
        node.NodeIndex = binaryNode.NodeHeader.NodeIndex;
        return node;
    }
    private List<IMDLControllerRow<BaseMDLControllerData>> DeserializeController(MDLBinaryNode binaryNode, MDLBinaryControllerHeader binaryController)
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

        return Enumerable
            .Range(0, binaryController.RowCount)
            .Select(x => DeserializeControllerRow(nodeType, controllerType, columnCount, bezier, times[x], data.Take(columnCount * binaryController.RowCount)))
            .ToList();
    }
    private IMDLControllerRow<BaseMDLControllerData> DeserializeControllerRow(MDLBinaryNodeType nodeType, MDLBinaryControllerType controllerType, int columnCount, bool bezier, float timeStart, IEnumerable<byte[]> data)
    {
        if (bezier)
        {
            var cdata1 = DeserializeControllerData(nodeType, controllerType, data.Skip(columnCount * 0).Take(columnCount));
            var cdata2 = DeserializeControllerData(nodeType, controllerType, data.Skip(columnCount * 1).Take(columnCount));
            var cdata3 = DeserializeControllerData(nodeType, controllerType, data.Skip(columnCount * 2).Take(columnCount));
            var rowType = typeof(MDLControllerRow<>).MakeGenericType(cdata1.GetType());
            var factoryMethod = rowType.GetMethod(nameof(MDLControllerRow<BaseMDLControllerData>.CreateBezier))!;
            return (IMDLControllerRow<BaseMDLControllerData>)factoryMethod.Invoke(null, [timeStart, cdata1, cdata2, cdata3])!;
        }
        else
        {
            var cdata = DeserializeControllerData(nodeType, controllerType, data);
            var rowType = typeof(MDLControllerRow<>).MakeGenericType(cdata.GetType());
            var factoryMethod = rowType.GetMethod(nameof(MDLControllerRow<BaseMDLControllerData>.CreateLinear))!;
            return (IMDLControllerRow<BaseMDLControllerData>)factoryMethod.Invoke(null, [timeStart, cdata])!;
        }
    }
    private BaseMDLControllerData DeserializeControllerData(MDLBinaryNodeType nodeType, MDLBinaryControllerType controllerType, IEnumerable<byte[]> data)
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
            MDLBinaryControllerType.Orientation => DeserializeControllerOrientationData(data),
            MDLBinaryControllerType.Scale => new MDLControllerDataScale(floatData[0]),
            _ => throw new ArgumentException("Unknown Controller Type"),
        };
    }
    private MDLWalkmeshAABBNode DeserializeAABB(MDLTrimeshNode node, MDLBinaryAABBNode binaryAABB)
    {
        var boundingBox = binaryAABB.BoundingBox;
        var face = node.Faces.ElementAtOrDefault(binaryAABB.FaceIndex);
        var mostSignificantPlane = (MDLWalkmeshAABBNodeMostSignificantPlane)binaryAABB.MostSiginificantPlane;
        var leftChild = (binaryAABB.FaceIndex == -1) ? DeserializeAABB(node, binaryAABB.LeftNode) : null;
        var rightChild = (binaryAABB.FaceIndex == -1) ? DeserializeAABB(node, binaryAABB.RightNode) : null;

        return new MDLWalkmeshAABBNode(boundingBox, face, leftChild, rightChild, mostSignificantPlane);
    }
    private MDLControllerDataOrientation DeserializeControllerOrientationData(IEnumerable<byte[]> data)
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
}
