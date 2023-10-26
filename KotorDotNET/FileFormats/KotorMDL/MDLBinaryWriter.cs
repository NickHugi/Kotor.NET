using KotorDotNET.Common;
using KotorDotNET.Common.Geometry;
using KotorDotNET.Extensions;
using KotorDotNET.FileFormats.KotorRIM;
using static KotorDotNET.FileFormats.KotorMDL.MDLBinaryStructure;

namespace KotorDotNET.FileFormats.KotorMDL
{
    public class MDLBinaryWriter : IWriter<MDL>
    {
        public Stream Stream => _writer.BaseStream;
        public Stream MDXStream => _mdxWriter.BaseStream;

        private BinaryWriter _writer;
        private BinaryWriter _mdxWriter;
        private Stream _stream;

        private MDL? _mdl;
        private FileRoot _binaryMDL;

        private bool _tsl;
        private List<Node>? _nodes;
        private List<string>? _names;

        public MDLBinaryWriter(string filepath, string mdxFilepath)
        {
            _writer = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
        }
        public MDLBinaryWriter(Stream stream, Stream mdxStream)
        {
            _writer = new BinaryWriter(stream);
        }
        public MDLBinaryWriter()
        {
            _writer = new BinaryWriter(new MemoryStream());
            _mdxWriter = new BinaryWriter(new MemoryStream());
        }

        public byte[] Bytes(MDL mdl)
        {
            _mdl = mdl;
            var fileSize = Build();

            using (var memoryStream = new MemoryStream(fileSize))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                _binaryMDL.Write(binaryWriter);
                Stream.CopyTo(memoryStream);

                return new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }.Concat(memoryStream.ToArray()).ToArray();
            }
        }

        public void Write(MDL mdl)
        {
            
            //this._mdl = mdl;
            //file.Write(_writer);
        }

        private int Build()
        {
            _binaryMDL = new FileRoot();

            _nodes = _mdl.Nodes();
            _names = _nodes.Select(x => x.Name).Distinct().ToList();

            if (_tsl)
            {
                _binaryMDL.ModelHeader.GeometryHeader.FunctionPointer1 = GeometryHeader.K2_NORMAL_FP1;
                _binaryMDL.ModelHeader.GeometryHeader.FunctionPointer2 = GeometryHeader.K2_NORMAL_FP2;
            }
            else
            {
                _binaryMDL.ModelHeader.GeometryHeader.FunctionPointer1 = GeometryHeader.K1_NORMAL_FP1;
                _binaryMDL.ModelHeader.GeometryHeader.FunctionPointer2 = GeometryHeader.K1_NORMAL_FP2;
            }

            _binaryMDL.ModelHeader.GeometryHeader.Name = _mdl.ModelName;
            _binaryMDL.ModelHeader.GeometryHeader.NodeCount = _nodes.Count;
            _binaryMDL.ModelHeader.GeometryHeader.GeometryType = 2;
            _binaryMDL.ModelHeader.ModelType = _mdl.ModelType;
            _binaryMDL.ModelHeader.Fog = _mdl.Fog ? (byte)1 : (byte)0;
            _binaryMDL.ModelHeader.ChildModelCount = 0; // TODO
            _binaryMDL.ModelHeader.AnimationCount = _mdl.Animations.Count;
            _binaryMDL.ModelHeader.AnimationCount2 = _mdl.Animations.Count;
            _binaryMDL.ModelHeader.BoundingBoxMin = new Vector3(); // TODO
            _binaryMDL.ModelHeader.BoundingBoxMax = new Vector3(); // TODO
            _binaryMDL.ModelHeader.Radius = 0; // TODO
            _binaryMDL.ModelHeader.AnimationScale = _mdl.AnimationScale;
            _binaryMDL.ModelHeader.SupermodelName = _mdl.Supermodel;
            _binaryMDL.ModelHeader.MDXSize = 0; // TODO
            _binaryMDL.ModelHeader.MDXOffset = 0; // TODO
            _binaryMDL.ModelHeader.OffsetToNameOffsetArray = ModelHeader.SIZE;
            _binaryMDL.ModelHeader.NamesArrayCount = _names.Count;
            _binaryMDL.ModelHeader.NamesArrayCount2 = _names.Count;

            var nameOffset = _binaryMDL.ModelHeader.OffsetToNameOffsetArray + (_names.Count * 4);
            for (int i = 0; i < _names.Count; i++)
            {
                var name = _names[i];
                _binaryMDL.NameOffsets.Add(nameOffset);
                _binaryMDL.Names.Add(name + '\0');
                nameOffset += name.Length + 1;
            }

            _binaryMDL.ModelHeader.AnimationOffsetArrayOffset = nameOffset;
            var animationOffset = _binaryMDL.ModelHeader.AnimationOffsetArrayOffset + (_mdl.Animations.Count * 4);
            foreach (var animation in _mdl.Animations)
            {
                _binaryMDL.AnimationOffsets.Add(animationOffset);
                (var binaryAnimation, animationOffset) = BuildAnimation(animation, animationOffset);
                _binaryMDL.Animations.Add(binaryAnimation);
            }

            _binaryMDL.ModelHeader.OffsetToRootNode = animationOffset;
            _binaryMDL.ModelHeader.GeometryHeader.RootNodeOffset = animationOffset;
            (_binaryMDL.RootNode, var fileSize) = BuildNode(_mdl.Root, animationOffset);

            return fileSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="offset">Offset to the start of the animation data.</param>
        /// <returns>Offset to the end of the animation data.</returns>
        private (AnimationRoot, int) BuildAnimation(Animation animation, int offset)
        {
            var animationOffset = offset;
            var binaryAnimation = new AnimationRoot();

            if (_tsl)
            {
                binaryAnimation.Header.GeometryHeader.FunctionPointer1 = GeometryHeader.K2_ANIM_FP1;
                binaryAnimation.Header.GeometryHeader.FunctionPointer2 = GeometryHeader.K2_ANIM_FP2;
            }
            else
            {
                binaryAnimation.Header.GeometryHeader.FunctionPointer1 = GeometryHeader.K1_ANIM_FP1;
                binaryAnimation.Header.GeometryHeader.FunctionPointer2 = GeometryHeader.K1_ANIM_FP2;
            }

            binaryAnimation.Header.GeometryHeader.Name = animation.Name;
            binaryAnimation.Header.GeometryHeader.NodeCount = animation.Nodes().Count;
            binaryAnimation.Header.GeometryHeader.RootNodeOffset = offset + AnimationHeader.SIZE;
            binaryAnimation.Header.GeometryHeader.GeometryType = 5;
            binaryAnimation.Header.AnimationLength = animation.AnimationLength;
            binaryAnimation.Header.TransitionTime = animation.TransitionTime;
            binaryAnimation.Header.AnimationRoot = animation.AnimationRoot;
            binaryAnimation.Header.EventCount1 = animation.Events.Count;
            binaryAnimation.Header.EventCount2 = animation.Events.Count;

            (binaryAnimation.RootNode, offset) = BuildNode(animation.RootNode, binaryAnimation.Header.GeometryHeader.RootNodeOffset, 0, animationOffset);

            binaryAnimation.Header.OffsetToEventArray = offset;
            foreach (var e in animation.Events)
            {
                offset += EventRoot.SIZE;
                binaryAnimation.Events.Add(new EventRoot
                {
                    ActivationTime = e.ActivationTime,
                    Name = e.Name,
                });
            }

            return (binaryAnimation, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">The node to be converted into the binary structure class equivalent.</param>
        /// <param name="offset">Offset to the start of the node.</param>
        /// <param name="parentOffset">Offset to the start of the parent node.</param>
        /// <param name="rootOffset">Offset to the start of the animation (when applicable).</param>
        /// <returns></returns>
        private (NodeRoot, int) BuildNode(Node node, int offset, int parentOffset = 0, int rootOffset = 0)
        {
            var binaryNode = new NodeRoot();
            var nodeOffset = offset;

            // Headers
            // Child Offset Array
            // Controller Array
            // Controller Data
            // Faces [Trimesh]
            // Vertices [Trimesh]
            // [Trimesh]
            // [Trimesh]
            // [Trimesh]

            ushort type = 0;
            if (true)
            {
                type += NodeRoot.NodeFlag;
                offset += NodeHeader.SIZE;
            }
            //if (binaryNode.LightHeader is not null) type += NodeRoot.LightFlag;
            //if (binaryNode.EmitterHeader is not null) type += NodeRoot.EmitterFlag;
            //if (binaryNode.ReferenceHeader is not null) type += NodeRoot.ReferenceFlag;
            if (node.Trimesh is not null)
            {
                type += NodeRoot.TrimeshFlag;
                offset += _tsl ? TrimeshHeader.K2_SIZE : TrimeshHeader.K1_SIZE;
            }
            //if (binaryNode.SkinmeshHeader is not null) type += NodeRoot.SkinFlag;
            //if (binaryNode.DanglymeshHeader is not null) type += NodeRoot.DanglyFlag;
            //if (binaryNode.SabermeshHeader is not null) type += NodeRoot.SaberFlag;

            if (node.Trimesh is not null)
            {
                (offset, binaryNode.TrimeshHeader) = BuildTrimesh(node, binaryNode, offset);
            }

            binaryNode.NodeHeader.NodeType = type;
            binaryNode.NodeHeader.IndexNumber = (ushort)_nodes.FindIndex(x => x.Name == node.Name);
            binaryNode.NodeHeader.NodeNumber = (ushort)_names.IndexOf(node.Name);
            binaryNode.NodeHeader.OffsetToRootNode = rootOffset;
            binaryNode.NodeHeader.OffsetToParentNode = parentOffset;
            binaryNode.NodeHeader.Position = node.Position;
            binaryNode.NodeHeader.Rotation = node.Rotation;

            binaryNode.NodeHeader.OffsetToChildArray = offset;
            binaryNode.NodeHeader.ChildArrayCount = node.Children.Count;
            binaryNode.NodeHeader.ChildArrayCount2 = binaryNode.NodeHeader.ChildArrayCount;

            binaryNode.NodeHeader.OffsetToControllerArray = binaryNode.NodeHeader.OffsetToChildArray + (node.Children.Count * 4);
            binaryNode.NodeHeader.ControllerArrayCount = node.Controllers.Count;
            binaryNode.NodeHeader.ControllerArrayCount2 = binaryNode.NodeHeader.ControllerArrayCount;

            binaryNode.NodeHeader.OffsetToControllerData = binaryNode.NodeHeader.OffsetToControllerArray + (node.Controllers.Count * ControllerHeader.SIZE);
            binaryNode.NodeHeader.ControllerDataCount = node.Controllers.SelectMany(x => x.Rows).SelectMany(x => x.Data).Count() + node.Controllers.SelectMany(x => x.Rows).Count();
            binaryNode.NodeHeader.ControllerDataCount2 = binaryNode.NodeHeader.ControllerDataCount;

            offset = binaryNode.NodeHeader.OffsetToControllerData + (binaryNode.NodeHeader.ControllerDataCount * 4);

            var childOffset = offset;
            foreach (var child in node.Children)
            {
                binaryNode.ChildrenOffsets.Add(childOffset);
                (var binaryChild, childOffset) = BuildNode(child, childOffset, offset, rootOffset);
                binaryNode.Children.Add(binaryChild);
            }

            short dataOffset = 0;
            foreach (var controller in node.Controllers)
            {
                var binaryController = new ControllerHeader();
                binaryController.ControllerType = controller.ControllerType;
                binaryController.DataRowCount = (short)controller.Rows.Count;

                binaryController.FirstKeyOffset = dataOffset;
                var timekeys = controller.Rows.Select(x => BitConverter.GetBytes(x.TimeKey)).ToList();
                binaryNode.ControllerData.AddRange(timekeys);
                dataOffset += (short)timekeys.Count();

                binaryController.FirstDataOffset = dataOffset;
                var rows = controller.Rows.SelectMany(x => x.Data).ToList();
                binaryNode.ControllerData.AddRange(rows);
                dataOffset += (short)rows.Count();

                binaryController.ColumnCount = (byte)controller.Rows.First().Data.Count();
                if (controller.ControllerType == 20 && binaryController.ColumnCount == 1)
                    binaryController.ColumnCount = 2;
            }

            return (binaryNode, childOffset);
        }

        private (int, TrimeshHeader) BuildTrimesh(Node node, NodeRoot binaryNode, int offset)
        {
            var trimeshHeader = binaryNode.TrimeshHeader = new TrimeshHeader();

            trimeshHeader.Diffuse = node.Trimesh.DiffuseColor;
            trimeshHeader.Ambient = node.Trimesh.AmbientColor;
            trimeshHeader.TransparencyHint = node.Trimesh.TransperencyHint;
            trimeshHeader.Texture = node.Trimesh.DiffuseTexture;
            trimeshHeader.Lightmap = node.Trimesh.LightmapTexture;
            trimeshHeader.DoesRender = node.Trimesh.Render ? (byte)1 : (byte)0;
            trimeshHeader.HasShadow = node.Trimesh.Shadow ? (byte)1 : (byte)0;
            trimeshHeader.Beaming = node.Trimesh.Beaming ? (byte)1 : (byte)0;
            trimeshHeader.HasLightmap = node.Trimesh.Lightmap ? (byte)1 : (byte)0;
            trimeshHeader.RotateTexture = node.Trimesh.RotateTexture ? (byte)1 : (byte)0;
            trimeshHeader.BackgroundGeometry = node.Trimesh.BackgroundGeometry ? (byte)1 : (byte)0;
            trimeshHeader.AnimateUV = node.Trimesh.AnimateUV ? (byte)1 : (byte)0;
            trimeshHeader.UVDirection = node.Trimesh.UVDirection;
            trimeshHeader.UVSpeed = node.Trimesh.UVSpeed;
            trimeshHeader.UVJitterSpeed = node.Trimesh.UVJitter;

            var vertices = node.Trimesh.Faces.Select(x => x.Vertex1).Concat(node.Trimesh.Faces.Select(x => x.Vertex2)).Concat(node.Trimesh.Faces.Select(x => x.Vertex3)).ToList();

            trimeshHeader.VertexCount = (ushort)vertices.Count();
            trimeshHeader.MDXOffsetToData = (int)_writer.BaseStream.Position;
            var hasPositionMDX = vertices.Any(x => x.Position is not null);
            var hasNormalMDX = vertices.Any(x => x.Normal is not null);
            var hasTextureMDX = vertices.Any(x => x.DiffuseUV is not null);
            var hasLightmapMDX = vertices.Any(x => x.LightmapUV is not null);

            if (hasPositionMDX)
            {
                trimeshHeader.MDXDataBitmap += NodeRoot.VertexFlag;

                trimeshHeader.MDXPositionStride = trimeshHeader.MDXDataSize;
                trimeshHeader.MDXDataSize += 12;
            }
            if (hasNormalMDX)
            {
                trimeshHeader.MDXDataBitmap += NodeRoot.NormalFlag;

                trimeshHeader.MDXNormalStride = trimeshHeader.MDXDataSize;
                trimeshHeader.MDXDataSize += 12;
            }
            if (hasTextureMDX)
            {
                trimeshHeader.MDXDataBitmap += NodeRoot.UV1Flag;

                trimeshHeader.MDXTexture1Stride = trimeshHeader.MDXDataSize;
                trimeshHeader.MDXDataSize += 8;
            }
            if (hasLightmapMDX)
            {
                trimeshHeader.MDXDataBitmap += NodeRoot.UV2Flag;

                trimeshHeader.MDXTexture2Stride = trimeshHeader.MDXDataSize;
                trimeshHeader.MDXDataSize += 8;
            }

            foreach (var vertex in vertices)
            {
                if (hasPositionMDX)
                {
                    _mdxWriter.Write(vertex.Position ?? new());
                }
                if (hasNormalMDX)
                {
                    _mdxWriter.Write(vertex.Normal ?? new());
                }
                if (hasTextureMDX)
                {
                    _mdxWriter.Write(vertex.DiffuseUV ?? new());
                }
                if (hasLightmapMDX)
                {
                    _mdxWriter.Write(vertex.LightmapUV ?? new());
                }
            }
            // TODO - plus extra block for whatever reason

            trimeshHeader.OffsetToFaceArray = offset;
            trimeshHeader.FaceArrayCount = trimeshHeader.FaceArrayCount2 = node.Trimesh.Faces.Count;

            foreach (var face in node.Trimesh.Faces)
            {
                var binaryFace = new FaceRoot();
                binaryNode.TrimeshFaces.Add(binaryFace);

                binaryFace.Material = face.MaterialID;
                binaryFace.PlaneCoefficient = face.PlaneDistance;
                binaryFace.Normal = face.FaceNormal;
                binaryFace.Vertex1 = (ushort)vertices.IndexOf(face.Vertex1);
                binaryFace.Vertex2 = (ushort)vertices.IndexOf(face.Vertex2);
                binaryFace.Vertex3 = (ushort)vertices.IndexOf(face.Vertex3);
                binaryFace.FaceAdjacency1 = (face.Adjacent1 is null) ? (ushort)0xFFFF : (ushort)node.Trimesh.Faces.IndexOf(face.Adjacent1);
                binaryFace.FaceAdjacency2 = (face.Adjacent2 is null) ? (ushort)0xFFFF : (ushort)node.Trimesh.Faces.IndexOf(face.Adjacent2);
                binaryFace.FaceAdjacency3 = (face.Adjacent3 is null) ? (ushort)0xFFFF : (ushort)node.Trimesh.Faces.IndexOf(face.Adjacent3);

                offset += FaceRoot.SIZE;
            }

            return (offset, trimeshHeader);
        }




        private NodeRoot OldBuildNode(Node node, Node parent)
        {
            var binNode = new NodeRoot();
            var nodeOffset = NodeOffset(node);

            var childrenOffsetsOffset = nodeOffset + NodeHeader.SIZE;
            var controllerOffset = childrenOffsetsOffset + (node.Children.Count * 4);
            var controllerDataOffset = controllerOffset + (node.Controllers.Count * ControllerHeader.SIZE);

            ushort type = 0;
            if (binNode.NodeHeader is not null) type += NodeRoot.NodeFlag;
            if (binNode.LightHeader is not null) type += NodeRoot.LightFlag;
            if (binNode.EmitterHeader is not null) type += NodeRoot.EmitterFlag;
            if (binNode.ReferenceHeader is not null) type += NodeRoot.ReferenceFlag;
            if (binNode.TrimeshHeader is not null) type += NodeRoot.TrimeshFlag;
            if (binNode.SkinmeshHeader is not null) type += NodeRoot.SkinFlag;
            if (binNode.DanglymeshHeader is not null) type += NodeRoot.DanglyFlag;
            if (binNode.SabermeshHeader is not null) type += NodeRoot.SaberFlag;

            binNode.NodeHeader = new NodeHeader
            {
                NodeType = type,
                IndexNumber = (ushort)_nodes.IndexOf(node),
                NodeNumber = (ushort)_names.IndexOf(node.Name),
                OffsetToRootNode = 0, // TODO - supermodel?
                OffsetToParentNode = (parent is null) ? 0 : NodeOffset(parent),
                Position = node.Position,
                Rotation = node.Rotation,
                OffsetToChildArray = childrenOffsetsOffset,
                ChildArrayCount = node.Children.Count,
                ChildArrayCount2 = node.Children.Count,
                OffsetToControllerArray = controllerOffset,
                ControllerArrayCount = node.Controllers.Count,
                ControllerArrayCount2 = node.Controllers.Count,
                OffsetToControllerData = controllerDataOffset,
                //ControllerDataCount = node.Controllers.SelectMany(x => x.TimeKeys).Count() * 4,
                //ControllerDataCount2 = (node.Controllers.SelectMany(x => x.TimeKeys).Count() + node.Controllers.SelectMany(x => x.Rows).SelectMany(y => y).Count()) * 4,
            };

            return binNode;
        }

        private int NameArrayOffset()
        {
            return ModelHeader.SIZE;
        }

        private int NameOffset(string target)
        {
            var offset = NameArrayOffset() + (4 * _names.Count);

            foreach (var name in _names)
            {
                if (name == target)
                    break;

                offset += name.Length + 1;
            }

            return offset;
        }

        private int AnimationOffset(Animation? animation)
        {
            return NameOffset(_names.Last());
        }

        private int NodeOffset(Node target)
        {
            var offset = AnimationOffset(_mdl.Animations.Last());

            foreach (var node in _mdl.Nodes())
            {
                if (target == node)
                    break;

                offset += NodeHeader.SIZE;

                if (node.Trimesh is not null)
                {
                    offset += _tsl ? TrimeshHeader.K2_SIZE : TrimeshHeader.K1_SIZE;
                    offset += node.Trimesh.Faces.Count * FaceRoot.SIZE;
                }
                else if (node is not null)
                {
                    //offset += LightHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += EmitterHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += ReferenceHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += SkinmeshHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += DanglymeshHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += SabermeshHeader.SIZE;
                }
                else if (node is not null)
                {
                    //offset += WalkmeshHeader.SIZE;
                }

                offset += (node.Children.Count * 4);
                offset += (node.Controllers.Count * ControllerHeader.SIZE);
            }

            return offset;
        }
    }
}
