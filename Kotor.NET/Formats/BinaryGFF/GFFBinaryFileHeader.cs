using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryGFF;

public class GFFBinaryFileHeader
{
    public static readonly int SIZE = 56;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "GFF ", // TODO
    };
    public static readonly string FILE_VERSION = "V3.2";

    public string FileType { get; set; } = "GFF ";
    public string FileVersion { get; set; } = "V3.2";
    public int OffsetToStructs { get; set; }
    public int StructCount { get; set; }
    public int OffsetToFields { get; set; }
    public int FieldCount { get; set; }
    public int OffsetToLabels { get; set; }
    public int LabelCount { get; set; }
    public int OffsetToFieldData { get; set; }
    public int FieldDataLength { get; set; }
    public int OffsetToFieldIndices { get; set; }
    public int FieldIndiciesLength { get; set; }
    public int OffsetToListIndicies { get; set; }
    public int ListIndiciesLength { get; set; }

    public GFFBinaryFileHeader()
    {
    }
    public GFFBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        OffsetToStructs = reader.ReadInt32();
        StructCount = reader.ReadInt32();
        OffsetToFields = reader.ReadInt32();
        FieldCount = reader.ReadInt32();
        OffsetToLabels = reader.ReadInt32();
        LabelCount = reader.ReadInt32();
        OffsetToFieldData = reader.ReadInt32();
        FieldDataLength = reader.ReadInt32();
        OffsetToFieldIndices = reader.ReadInt32();
        FieldIndiciesLength = reader.ReadInt32();
        OffsetToListIndicies = reader.ReadInt32();
        ListIndiciesLength = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType, 0);
        writer.Write(FileVersion, 0);
        writer.Write(OffsetToStructs);
        writer.Write(StructCount);
        writer.Write(OffsetToFields);
        writer.Write(FieldCount);
        writer.Write(OffsetToLabels);
        writer.Write(LabelCount);
        writer.Write(OffsetToFieldData);
        writer.Write(FieldDataLength);
        writer.Write(OffsetToFieldIndices);
        writer.Write(FieldIndiciesLength);
        writer.Write(OffsetToListIndicies);
        writer.Write(ListIndiciesLength);
    }
}
