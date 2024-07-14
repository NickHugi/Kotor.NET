using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryKEY;

public class KEYBinaryFileHeader
{
    public static readonly int SIZE = 64;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "KEY "
    };
    public static readonly string FILE_VERSION = "V1  ";

    public string FileType { get; set; } = "KEY ";
    public string FileVersion { get; set; } = "V1  ";
    public int FileCount { get; set; }
    public int KeyCount { get; set; }
    public int OffsetToFileEntries { get; set; }
    public int OffsetToKeyEntries { get; set; }
    public int BuildYear { get; set; } = DateTime.UtcNow.Year;
    public int BuildDay { get; set; } = DateTime.UtcNow.Day;
    public byte[] Reserved { get; set; } = new byte[32];

    public KEYBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        FileCount = reader.ReadInt32();
        KeyCount = reader.ReadInt32();
        OffsetToFileEntries = reader.ReadInt32();
        OffsetToKeyEntries = reader.ReadInt32();
        BuildYear = reader.ReadInt32();
        BuildDay = reader.ReadInt32();
        Reserved = reader.ReadBytes(32);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 4);
        writer.Write(FileVersion, 4);
        writer.Write(FileCount);
        writer.Write(KeyCount);
        writer.Write(OffsetToFileEntries);
        writer.Write(OffsetToKeyEntries);
        writer.Write(Reserved);
    }
}
