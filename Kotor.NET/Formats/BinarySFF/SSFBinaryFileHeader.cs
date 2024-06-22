using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinarySSF;


public class SSFBinaryFileHeader
{
    public static readonly int SIZE = 12;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "SSF "
    };
    public static readonly string FILE_VERSION = "V1.1";

    public string FileType { get; set; } = "SSF ";
    public string FileVersion { get; set; } = FILE_VERSION;
    public int OffsetToSounds { get; set; }

    public SSFBinaryFileHeader()
    {

    }
    public SSFBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        OffsetToSounds = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write(OffsetToSounds);
    }
}
