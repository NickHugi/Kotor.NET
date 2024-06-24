using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryTPC;

public class TPCBinaryFileHeader
{
    public int Size { get; set; }
    public float Unknown { get; set; }
    public ushort Width { get; set; }
    public ushort Height { get; set; }
    public byte Encoding { get; set; }
    public byte MipmapCount { get; set; }
    public byte[] Unused { get; set; } = new byte[114];

    public bool Compressed => Size != 0;
    public bool CubeMap => Height / Width == 6;

    public TPCBinaryFileHeader()
    {
    }
    public TPCBinaryFileHeader(BinaryReader reader)
    {
        Size = reader.ReadInt32();
        Unknown = reader.ReadSingle();
        Width = reader.ReadUInt16();
        Height = reader.ReadUInt16();
        Encoding = reader.ReadByte();
        MipmapCount = reader.ReadByte();
        Unused = reader.ReadBytes(114);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Size);
        writer.Write(Unknown);
        writer.Write(Width);
        writer.Write(Height);
        writer.Write(Encoding);
        writer.Write(MipmapCount);
        writer.Write(Unused);
    }
}
