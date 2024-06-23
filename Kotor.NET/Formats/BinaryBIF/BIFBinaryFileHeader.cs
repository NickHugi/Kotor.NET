using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryBIF;

public class BIFBinaryFileHeader
{
    public static readonly int SIZE = 16;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "BIFF"
    };
    public static readonly string FILE_VERSION = "V1  ";

    public string FileType { get; set; } = "BIF ";
    public string FileVersion { get; set; } = "V1  ";
    public int ResourceCount { get; set; }
    public int OffsetToUnused { get; set; }
    public int OffsetToResources { get; set; }

    public BIFBinaryFileHeader()
    {
    }
    public BIFBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        ResourceCount = reader.ReadInt32();
        OffsetToUnused = reader.ReadInt32();
        OffsetToResources = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType);
        writer.Write(FileVersion);
        writer.Write(ResourceCount);
        writer.Write(OffsetToUnused);
        writer.Write(OffsetToResources);
    }
}
