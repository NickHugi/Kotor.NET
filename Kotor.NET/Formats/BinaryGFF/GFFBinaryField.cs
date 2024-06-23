using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryGFF;

public class GFFBinaryField
{
    public static readonly int SIZE = 12;

    public int Type { get; set; }
    public uint LabelIndex { get; set; }
    public byte[] DataOrDataOffset { get; set; } = new byte[0];

    public GFFBinaryField()
    {
    }
    public GFFBinaryField(BinaryReader reader)
    {
        Type = reader.ReadInt32();
        LabelIndex = reader.ReadUInt32();
        DataOrDataOffset = reader.ReadBytes(4);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Type);
        writer.Write(LabelIndex);
        writer.Write(DataOrDataOffset);
    }
}
