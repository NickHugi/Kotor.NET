using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryBIF;

public class BIFBinary
{
    public BIFBinaryFileHeader FileHeader { get; set; } = new();
    public List<BIFBinaryVariableResource> Resources { get; set; } = new();
    public List<byte[]> ResourceData { get; set; } = new();

    public BIFBinary()
    {
    }
    public BIFBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new BIFBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToResources;
        for (int i = 0; i < FileHeader.ResourceCount; i++)
        {
            var resource = new BIFBinaryVariableResource(reader);
            Resources.Add(resource);
        }

        for (int i = 0; i < FileHeader.ResourceCount; i++)
        {
            reader.BaseStream.Position = Resources[i].Offset;
            var data = reader.ReadBytes(Resources[i].FileSize);
            ResourceData.Add(data);
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        writer.BaseStream.Position = FileHeader.OffsetToResources;
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].Write(writer);
        }

        for (int i = 0; i < Resources.Count; i++)
        {
            writer.BaseStream.Position = Resources[i].Offset;
            Resources[i].Write(writer);
        }
    }

    public void Recalculate()
    {
        FileHeader.ResourceCount = Resources.Count;

        var offset = 0;
        FileHeader.OffsetToUnused = offset;

        offset += BIFBinaryFileHeader.SIZE;
        FileHeader.OffsetToResources = offset;

        offset += BIFBinaryVariableResource.SIZE * Resources.Count;
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].FileSize = ResourceData[i].Length;
            Resources[i].Offset = offset;
            offset += BIFBinaryVariableResource.SIZE;
        }
    }
}
