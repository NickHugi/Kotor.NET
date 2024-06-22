using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryLIP;


public class LIPBinaryFileHeader
{
    public static readonly int SIZE = 16;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "LIP "
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "LIP ";
    public string FileVersion { get; set; } = FILE_VERSION;
    public float Duration { get; set; }
    public int KeyFrameCount { get; set; }

    public LIPBinaryFileHeader()
    {

    }
    public LIPBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        Duration = reader.ReadSingle();
        KeyFrameCount = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write(Duration);
        writer.Write(KeyFrameCount);
    }
}
