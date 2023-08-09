using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryERF;

public class ERFBinaryFileHeader
{
    public static readonly int SIZE = 40;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "ERF ", "SAV ", "MOD ", "HAK ",
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "ERF ";
    public string FileVersion { get; set; } = FILE_VERSION;
    public int EntryCount { get; set; }
    public int OffsetToKeyList { get; set; }
    public int OffsetToResourceList { get; set; }
    public int BuildYear { get; set; }
    public int BuildDay { get; set; }

    public ERFBinaryFileHeader()
    {

    }
    public ERFBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        reader.BaseStream.Position += 4; // LanguageCount
        reader.BaseStream.Position += 4; // LocalizedStringSize
        EntryCount = reader.ReadInt32();
        reader.BaseStream.Position += 4; // OffsetToLocalizedString
        OffsetToKeyList = reader.ReadInt32();
        OffsetToResourceList = reader.ReadInt32();
        BuildYear = reader.ReadInt32();
        BuildDay = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(EntryCount);
        writer.Write(0);
        writer.Write(OffsetToKeyList);
        writer.Write(OffsetToResourceList);
        writer.Write(BuildYear);
        writer.Write(BuildDay);
    }
}
