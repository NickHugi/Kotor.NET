using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryKEY;

public class KEYBinaryKeyEntry
{
    public static readonly int SIZE = 22;

    public ResRef ResRef { get; set; }
    public ushort ResourceType { get; set; }
    public uint ResourceID { get; set; }

    public uint IndexIntoFileTable => ResourceID >> 20;
    public uint IndexIntoResourceTable => (ResourceID << 20) >> 20;

    public KEYBinaryKeyEntry(BinaryReader reader)
    {
        ResRef = reader.ReadString(16);
        ResourceType = reader.ReadUInt16();
        ResourceID = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ResRef);
        writer.Write(ResourceType);
        writer.Write(ResourceID);
    }
}
