using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryTGA;

public class TGABinaryFileHeader
{
    public byte IDLength { get; set; }
    public byte IsColourMapped { get; set; }
    public byte ImageType { get; set; }

    public short ColourMapOrigin { get; set; }
    public short ColourMapLength { get; set; }
    public byte ColourMapDepth { get; set; }

    public short XOrigin { get; set; }
    public short YOrigin { get; set; }
    public short Width { get; set; }
    public short Height { get; set; }
    public byte BitsPerPixel { get; set; }
    public byte ImageDescriptor { get; set; }

    public TGABinaryFileHeader()
    {
    }
    public TGABinaryFileHeader(BinaryReader reader)
    {
        IDLength = reader.ReadByte();
        IsColourMapped = reader.ReadByte();
        ImageType = reader.ReadByte();
        ColourMapOrigin = reader.ReadInt16();
        ColourMapLength = reader.ReadInt16();
        ColourMapDepth = reader.ReadByte();
        XOrigin = reader.ReadInt16();
        YOrigin = reader.ReadInt16();
        Width = reader.ReadInt16();
        Height = reader.ReadInt16();
        BitsPerPixel = reader.ReadByte();
        ImageDescriptor = reader.ReadByte();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(IDLength);
        writer.Write(IsColourMapped);
        writer.Write(ImageType);
        writer.Write(ColourMapOrigin);
        writer.Write(ColourMapLength);
        writer.Write(ColourMapDepth);
        writer.Write(XOrigin);
        writer.Write(YOrigin);
        writer.Write(Width);
        writer.Write(Height);
        writer.Write(BitsPerPixel);
        writer.Write(ImageDescriptor);
    }
}
