using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryBIF;

public class BIFBinaryVariableResource
{
    public static readonly int SIZE = 16;

    public uint ResourceID { get; set; }
    public int Offset { get; set; }
    public int FileSize { get; set; }
    public int ResourceType { get; set; }

    public BIFBinaryVariableResource()
    {
    }
    public BIFBinaryVariableResource(BinaryReader reader)
    {
        ResourceID = reader.ReadUInt32();
        Offset = reader.ReadInt32();
        FileSize = reader.ReadInt32();
        ResourceType = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ResourceID);
        writer.Write(Offset);
        writer.Write(FileSize);
        writer.Write(ResourceType);
    }
}
