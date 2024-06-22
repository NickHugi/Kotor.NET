using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryTLK;


public class TLKBinaryFileHeader
{
    public static readonly int SIZE = 20;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "TLK "
    };
    public static readonly string FILE_VERSION = "V3.0";

    public string FileType { get; set; } = "TLK ";
    public string FileVersion { get; set; } = "V3.0";
    public int LanguageID { get; set; }
    public int EntryCount { get; set; }
    public int OffsetToEntries { get; set; }

    public TLKBinaryFileHeader()
    {

    }

    public TLKBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        LanguageID = reader.ReadInt32();
        EntryCount = reader.ReadInt32();
        OffsetToEntries = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write(LanguageID);
        writer.Write(EntryCount);
        writer.Write(OffsetToEntries);
    }
}
