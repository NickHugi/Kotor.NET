using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common;
using KotorDotNET.Common.Conversation;
using KotorDotNET.Common.Creature;
using KotorDotNET.Common.Data;
using static KotorDotNET.FileFormats.KotorMDL.MDLBinaryStructure;

namespace KotorDotNET.FileFormats.KotorMDL
{
    public class MDLBinaryReader : IReader<MDL>
    {
        private BinaryReader _reader;
        private MDL? _mdl;
        private FileRoot _bin;

        public MDLBinaryReader(string filepath)
        {
            var data = File.ReadAllBytes(filepath);
            data = data.TakeLast(data.Length - 12).ToArray();
            _reader = new BinaryReader(new MemoryStream(data));
        }
        public MDLBinaryReader(byte[] data)
        {
            data = data.TakeLast(data.Length - 12).ToArray();
            _reader = new BinaryReader(new MemoryStream(data));
        }
        public MDLBinaryReader(Stream stream)
        {
            _reader = new BinaryReader(stream);
            var length = (int)(stream.Length - stream.Length);
            var data = _reader.ReadBytes(length).TakeLast(length - 12).ToArray();
            _reader = new BinaryReader(new MemoryStream(data));
        }

        public MDL Read()
        {
            _mdl = new MDL();

            _bin = new FileRoot(_reader);

            _mdl.Fog = _bin.ModelHeader.Fog != 0;
            _mdl.AnimationScale = _bin.ModelHeader.AnimationScale;
            _mdl.Supermodel = _bin.ModelHeader.SupermodelName;
            _mdl.ModelType = _bin.ModelHeader.ModelType;
            _mdl.ModelName = _bin.ModelHeader.GeometryHeader.Name;
            _mdl.Root = BuildNode(_bin.RootNode);
            _mdl.Animations = _bin.Animations.Select(x => new Animation
            {
                Name = x.Header.GeometryHeader.Name,
                AnimationLength = x.Header.AnimationLength,
                TransitionTime = x.Header.TransitionTime,
                AnimationRoot = x.Header.AnimationRoot,
                GeometryType = x.Header.GeometryHeader.GeometryType,
                RootNode = BuildNode(x.RootNode),
                Events = x.Events.Select(x => new Event
                {
                    ActivationTime = x.ActivationTime,
                    Name = x.Name,
                }).ToList(),
            }).ToList();

            return _mdl;
        }

        private Node BuildNode(NodeRoot binNode)
        {
            Node node = new Node();

            node.Name = _bin.Names[binNode.NodeHeader!.NodeNumber];

            foreach (var binNodeChild in binNode.Children)
            {
                node.Children.Add(BuildNode(binNodeChild));
            }

            foreach (var binController in binNode.Controllers)
            {
                var controller = new Controller();
                controller.ControllerType = binController.ControllerType;
                for (int i = 0; i < binController.DataRowCount; i++)
                {
                    var row = new ControllerRow();
                    controller.Rows.Add(row);

                    row.TimeKey = BitConverter.ToSingle(binNode.ControllerData.ElementAt(binController.FirstKeyOffset));

                    if (controller.ControllerType == 20 && binController.ColumnCount == 2)
                    {
                        row.Data = binNode.ControllerData.GetRange(binController.FirstDataOffset, 1).ToList();
                    }
                    else
                    {
                        row.Data = binNode.ControllerData.GetRange(binController.FirstDataOffset, binController.ColumnCount).ToList();
                    }

                    
                }
                node.Controllers.Add(controller);
            }

            return node;
        }
    }
}
