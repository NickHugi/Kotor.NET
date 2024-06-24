using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryLTR;

public class LTRBinaryFileHeader
{
    public static readonly int SIZE = 9;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "LTR",
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "LTR ";
    public string FileVersion { get; set; } = "V1.0";
    public byte LetterCount { get; set; } = 28;

    public LTRBinaryFileHeader()
    {
    }
    public LTRBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        LetterCount = reader.ReadByte();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType);
        writer.Write(FileVersion);
        writer.Write(LetterCount);
    }
}
