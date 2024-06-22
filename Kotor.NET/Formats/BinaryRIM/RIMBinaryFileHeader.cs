using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryRIM;

public class RIMBinaryFileHeader
{
    public static readonly int SIZE = 20;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "ERF ", "SAV ", "MOD ", "HAK ",
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "RIM ";
    public string FileVersion { get; set; } = "V1.0";
    public int ResourceCount { get; set; }
    public int OffsetToResources { get; set; }

    public RIMBinaryFileHeader()
    {

    }
    public RIMBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        reader.BaseStream.Position += 4;
        ResourceCount = reader.ReadInt32();
        OffsetToResources = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write((int)0);
        writer.Write(ResourceCount);
        writer.Write(OffsetToResources);
    }
}
