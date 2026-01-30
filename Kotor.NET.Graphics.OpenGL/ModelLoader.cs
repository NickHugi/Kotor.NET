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

        var model = new KModel();
        model.Animations = [];
        for (int i = 0; i < modelHeader.AnimationCount; i++)
        {
            reader.SetStreamPosition(modelHeader.AnimationOffsetArrayOffset + (i * 4));
            var offset = reader.ReadInt32();
            model.Animations.Add(LoadAnimation(gl, reader, mdxReader, offset, model));
        }
        model.Root = LoadNode(gl, reader, mdxReader, modelHeader.OffsetToRootNode, null, model);
        return model;
    }

    private Animation LoadAnimation(GL gl, MDLBinaryReader reader, BinaryReader mdxReader, int animOffset, KModel model)
    {
        reader.SetStreamPosition(animOffset);
        var animationHeader = new MDLBinaryAnimationHeader(reader);

        var animation = new Animation();
        animation.Name = animationHeader.GeometryHeader.Name;
        animation.Root = LoadNode(gl, reader, mdxReader, animationHeader.GeometryHeader.RootNodeOffset, null, model);
        animation.Length = animationHeader.AnimationLength;
        animation.Transition = animationHeader.TransitionTime;
        return animation;
    }

    private BaseNode LoadNode(GL gl, MDLBinaryReader reader, BinaryReader mdxReader, int nodeOffset, BaseNode? parent, KModel model)
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
                    NodeID = dummyHeader.NodeNumber,
                    Model = model,
                    Parent = parent,
                    Visible = trimeshHeader.DoesRender != 0,
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
                    NodeID = dummyHeader.NodeNumber,
                    Model = model,
                    Parent = parent,
                    Visible = false,//trimeshHeader.DoesRender != 0,
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
                node = new SabermeshNode()
                {
                    NodeID = dummyHeader.NodeNumber,
                    Model = model,
                    Parent = parent,
                    Visible = trimeshHeader.DoesRender != 0,
                    Position = position,
                    Orientation = orientation
                };
            }
            else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.AABBFlag))
            {
                var walkmeshHeader = new MDLBinaryWalkmeshHeader(reader);
                node = new WalkmeshNode()
                {
                    NodeID = dummyHeader.NodeNumber,
                    Model = model,
                    Parent = parent,
                    Visible = trimeshHeader.DoesRender != 0,
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
                    NodeID = dummyHeader.NodeNumber,
                    Model = model,
                    Parent = parent,
                    Visible = true,//trimeshHeader.DoesRender != 0,
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
            node = new LightNode()
            {
                NodeID = dummyHeader.NodeNumber,
                Model = model,
                Parent = parent,
                Position = position,
                Orientation = orientation
            };
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.EmitterFlag))
        {
            node = new EmitterNode()
            {
                NodeID = dummyHeader.NodeNumber,
                Model = model,
                Parent = parent,
                Visible = true,
                Position = position,
                Orientation = orientation
            };
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.ReferenceFlag))
        {
            node = new ReferenceNode()
            {
                NodeID = dummyHeader.NodeNumber,
                Model = model,
                Parent = parent,
                Visible = true,
                Position = position,
                Orientation = orientation
            };
        }
        else if (((MDLBinaryNodeType)dummyHeader.NodeType).HasFlag(MDLBinaryNodeType.NodeFlag))
        {
            node = new DummyNode()
            {
                NodeID = dummyHeader.NodeNumber,
                Model = model,
                Parent = parent,
                Visible = true, 
                Position = position,
                Orientation = orientation
            };
        }
        else
        {
            throw new Exception();
        }

        var controllers = new List<Controller>();
        node.Controllers = controllers;
        for (int i = 0; i < dummyHeader.ControllerArrayCount; i++)
        {
            var controller = new Controller();

            reader.SetStreamPosition(dummyHeader.OffsetToControllerArray + (i * MDLBinaryControllerHeader.SIZE));
            var controllerHeader = new MDLBinaryControllerHeader(reader);
            for (int j = 0; j < controllerHeader.RowCount; j++)
            {
                var realColumnCount = (controllerHeader.ControllerType == 20 && controllerHeader.ColumnCount == 2) ? 1 : controllerHeader.ColumnCount;

                reader.SetStreamPosition(dummyHeader.OffsetToControllerData + (4 * controllerHeader.FirstKeyOffset) + (4 * j));
                var timeKey = reader.ReadSingle();
                reader.SetStreamPosition(dummyHeader.OffsetToControllerData + (4 * controllerHeader.FirstDataOffset) + (4 * j * realColumnCount));
                var data = Enumerable
                    .Range(0, realColumnCount)
                    .Select(x => reader.ReadSingle()).ToArray();

                controller.ControllerData.Add(new ControllerDataRow()
                {
                    TimeKey = timeKey,
                    Values = data
                });
            }
            controller.ControllerType = controllerHeader.ControllerType;
            controllers.Add(controller);
        }

        var childOffsets = new List<int>(dummyHeader.ChildArrayCount);
        reader.SetStreamPosition(dummyHeader.OffsetToChildOffsetArray);
        for (int i = 0; i < dummyHeader.ChildArrayCount; i++)
        {
            childOffsets.Add(reader.ReadInt32());
        }
        foreach (var childOffset in childOffsets)
        {
            node.Nodes.Add(LoadNode(gl, reader, mdxReader, childOffset, node, model));
        }

        return node;
    }
}
