using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryGFF;

public class GFFBinaryStruct
{
    public static readonly int SIZE = 12;

    public uint ID { get; set; }
    public int DataOrDataOffset { get; set; }
    public uint FieldCount { get; set; }

    public GFFBinaryStruct()
    {
    }
    public GFFBinaryStruct(BinaryReader reader)
    {
        ID = reader.ReadUInt32();
        DataOrDataOffset = reader.ReadInt32();
        FieldCount = reader.ReadUInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ID);
        writer.Write(DataOrDataOffset);
        writer.Write(FieldCount);
    }
}
