using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.NET.Resources.KotorGFF;

public class GFF
{
    public static GFF FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return new GFFBinary(stream).Parse();
    }
    public static GFF FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return new GFFBinary(stream).Parse();
    }
    public static GFF FromStream(Stream stream)
    {
        return new GFFBinary(stream).Parse();
    }

    public GFFType Type { get; set; }
    public GFFStruct Root { get; set; } = new();
}

public enum GFFType
{
    GFF,
    ARE,
    IFO,
    GIT,
    UTI,
    UTC,
    DLG,
    ITP,
    UTT,
    UTS,
    FAC,
    UTE,
    UTD,
    UTP,
    GUI,
    UTM,
    JRL,
    UTW,
    PTH,
}
