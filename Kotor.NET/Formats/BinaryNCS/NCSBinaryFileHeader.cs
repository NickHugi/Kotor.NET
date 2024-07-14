using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryNCS;

public class NCSBinaryFileHeader
{
    public static readonly int SIZE = 13;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "NCS",
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "NCS ";
    public string FileVersion { get; set; } = "V1.0";
    public byte ProgramSizeByteCode { get; set; } = 0x42;
    public int ProgramSize { get; set; }

    public NCSBinaryFileHeader()
    {
    }
    public NCSBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        ProgramSizeByteCode = reader.ReadByte();
        ProgramSize = BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(4));
    }

    public void Write(BinaryWriter writer)
    {

        writer.Write(FileType);
        writer.Write(FileVersion);

        Span<byte> programSize = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(programSize, ProgramSize);
        writer.Write(ProgramSizeByteCode);

        writer.Write(programSize);
    }
}
