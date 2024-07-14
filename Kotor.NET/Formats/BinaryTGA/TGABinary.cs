using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryTGA;

public class TGABinary
{
    public TGABinaryFileHeader FileHeader { get; set; } = new();
    public string ID { get; set; } = "";
    public List<byte[]> ColourMap { get; set; } = new(); // Element = Colour Map Entry
    public List<byte[]> ImageData { get; set; } = new(); // Element = Pixel / Colour Map Index / RLE Packet

    public TGABinary()
    {
    }
    public TGABinary(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        var reader = new BinaryReader(stream);
        FileHeader = new(reader);
        ID = reader.ReadString(FileHeader.IDLength);

        var bytesPerColourMapEntry = FileHeader.ColourMapDepth / 8;
        for (int i = 0; i < FileHeader.ColourMapLength; i++)
        {
            ColourMap.Add(reader.ReadBytes(bytesPerColourMapEntry));
        }

        var imageType = (TGABinaryImageType)FileHeader.ImageType;
        ImageData = imageType switch
        {
            TGABinaryImageType.ColourMapped => ReadMapped(reader),
            TGABinaryImageType.RGB => ReadRGB(reader),
            TGABinaryImageType.RLE_ColourMapped => ReadRLEMapped(reader),
            TGABinaryImageType.RLE_RGB => ReadRLERGB(reader),
            _ => throw new Exception()
        };
    }

    public void Write(BinaryWriter writer)
    {
        FileHeader.Write(writer);
        writer.Write(ID, 0);
        ColourMap.ForEach(x => writer.Write(x));
        ImageData.ForEach(x => writer.Write(x));
    }

    private List<byte[]> ReadMapped(BinaryReader reader)
    {
        var bytesPerEntry = FileHeader.ColourMapDepth / 8;
        var imageData = new List<byte[]>();
        for (int i = 0; i < FileHeader.Width; i++)
        {
            for (int j = 0; j < FileHeader.Height; j++)
            {
                imageData.Add(reader.ReadBytes(bytesPerEntry));
            }
        }
        return imageData;
    }
    private List<byte[]> ReadRGB(BinaryReader reader)
    {
        var bytesPerPixel = FileHeader.BitsPerPixel / 8;
        var imageData = new List<byte[]>();
        for (int i = 0; i < FileHeader.Width; i++)
        {
            for (int j = 0; j < FileHeader.Height; j++)
            {
                imageData.Add(reader.ReadBytes(bytesPerPixel));
            }
        }
        return imageData;
    }
    private List<byte[]> ReadRLEMapped(BinaryReader reader)
    {
        var bytesPerEntry = FileHeader.ColourMapDepth / 8;
        var imageData = new List<byte[]>();
        var pixelCount = 0;
        while (pixelCount < FileHeader.Width*FileHeader.Height)
        {
            var packetHeader = reader.ReadBytes(1);
            var isRunLengthPacket = (packetHeader[0] & 0x80) > 0;
            var n = packetHeader[0] & 0x7F;

            if (isRunLengthPacket)
            {
                imageData.Add(packetHeader.Concat(reader.ReadBytes(bytesPerEntry)).ToArray());
                pixelCount += n;
            }
            else
            {
                imageData.Add(packetHeader.Concat(reader.ReadBytes(n*bytesPerEntry)).ToArray());
                pixelCount += n;
            }
        }
        return imageData;
    }
    private List<byte[]> ReadRLERGB(BinaryReader reader)
    {
        var bytesPerPixel = FileHeader.BitsPerPixel / 8;
        var imageData = new List<byte[]>();
        var pixelCount = 0;
        while (pixelCount < FileHeader.Width * FileHeader.Height)
        {
            var packetHeader = reader.ReadBytes(1);
            var isRunLengthPacket = (packetHeader[0] & 0x80) > 0;
            var n = (packetHeader[0] & 0x7F) + 1;

            if (isRunLengthPacket)
            {
                imageData.Add(packetHeader.Concat(reader.ReadBytes(bytesPerPixel)).ToArray());
                pixelCount += n;
            }
            else
            {
                imageData.Add(packetHeader.Concat(reader.ReadBytes(bytesPerPixel*n)).ToArray());
                pixelCount += n;
            }
        }
        return imageData;
    }
}
