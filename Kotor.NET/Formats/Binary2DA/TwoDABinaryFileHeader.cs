using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.Binary2DA;

public class TwoDABinaryFileHeader
{
    public static readonly int SIZE = 11;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "2DA ",
    };
    public static readonly string FILE_VERSION = "V2.b";

    public string FileType { get; set; } = "2DA ";
    public string FileVersion { get; set; } = "v2.b";

    public TwoDABinaryFileHeader()
    {

    }

    public TwoDABinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        var linebreak = reader.ReadString(1);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType.PadRight(4).Truncate(4), 0);
        writer.Write(FileVersion.PadRight(4).Truncate(4), 0);
        writer.Write("\n", 0);
    }
}
