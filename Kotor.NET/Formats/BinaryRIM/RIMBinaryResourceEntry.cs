using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryRIM;

public class RIMBinaryResourceEntry
{
    public static readonly int SIZE = 32;

    public ResRef ResRef { get; set; } = new();
    public int ResourceID { get; set; }
    public int ResourceTypeID { get; set; }
    public int Offset { get; set; }
    public int Size { get; set; }

    public RIMBinaryResourceEntry()
    {

    }
    public RIMBinaryResourceEntry(BinaryReader reader)
    {
        ResRef = reader.ReadString(16);
        ResourceTypeID = reader.ReadInt32();
        ResourceID = reader.ReadInt32();
        Offset = reader.ReadInt32();
        Size = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ResRef, false);
        writer.Write(ResourceTypeID);
        writer.Write(ResourceID);
        writer.Write(Offset);
        writer.Write(Size);
    }
}
