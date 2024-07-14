using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryControllerHeader
{
    public static readonly int SIZE = 16;

    public int ControllerType { get; set; }
    public short Unknown { get; set; }
    public short RowCount { get; set; }
    public short FirstKeyOffset { get; set; }
    public short FirstDataOffset { get; set; }
    public byte ColumnCount { get; set; }
    public byte Padding0 { get; set; }
    public byte Padding1 { get; set; }
    public byte Padding2 { get; set; }

    public MDLBinaryControllerHeader()
    {
    }
    public MDLBinaryControllerHeader(MDLBinaryReader reader)
    {
        ControllerType = reader.ReadInt32();
        Unknown = reader.ReadInt16();
        RowCount = reader.ReadInt16();
        FirstKeyOffset = reader.ReadInt16();
        FirstDataOffset = reader.ReadInt16();
        ColumnCount = reader.ReadByte();
        Padding0 = reader.ReadByte();
        Padding1 = reader.ReadByte();
        Padding2 = reader.ReadByte();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(ControllerType);
        writer.Write(Unknown);
        writer.Write(RowCount);
        writer.Write(FirstKeyOffset);
        writer.Write(FirstDataOffset);
        writer.Write(ColumnCount);
        writer.Write(Padding0);
        writer.Write(Padding1);
        writer.Write(Padding2);
    }
}
