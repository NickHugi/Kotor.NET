using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;
using Kotor.NET.Resources.KotorTPC;

namespace Kotor.NET.Formats.BinaryTPC;

public class TPCBinary
{
    public TPCBinaryFileHeader FileHeader { get; set; } = new();
    public List<TPCBinaryLayer> Layers { get; set; } = new();
    public string TXI { get; set; } = "";

    public TPCBinary()
    {
    }
    public TPCBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);
        FileHeader = new TPCBinaryFileHeader(reader);

        var layerCount = FileHeader.CubeMap ? 6 : 1;
        Layers = Enumerable.Range(0, layerCount).Select(x => new TPCBinaryLayer()).ToList();

        var mipmapHeight = FileHeader.CubeMap ? FileHeader.Width : FileHeader.Height;
        var mipmapWidth = FileHeader.Width;
        for (int i = 0; i < FileHeader.MipmapCount; i++)
        {
            foreach (var layer in Layers)
            {
                var size = GetMipmapDataSize(mipmapWidth, mipmapHeight, FileHeader.Encoding, FileHeader.Compressed);
                var mipmap = reader.ReadBytes(size);
                layer.Mipmaps.Add(mipmap);
            }

            mipmapWidth = (ushort)Math.Max(1, mipmapWidth >> 1);
            mipmapHeight = (ushort)Math.Max(1, mipmapHeight >> 1);
        }

        var txiLength = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
        TXI = reader.ReadString(txiLength);
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        for (int i = 0; i < FileHeader.MipmapCount; i++)
        {
            foreach (var layer in Layers)
            {
                writer.Write(layer.Mipmaps[i]);
            }
        }

        writer.Write(TXI);
    }

    public void Recalculate()
    {
        FileHeader.MipmapCount = (byte)Layers.First().Mipmaps.Count();
        FileHeader.Unused = new byte[114];
    }

    public int GetMipmapDataSize(int width, int height, int encoding, bool compressed) => (TPCBinaryEncoding)encoding switch
    {
        TPCBinaryEncoding.Grayscale => (compressed) ? throw new ArgumentException("Compressed Greyscale is not supported") : (width * height),
        TPCBinaryEncoding.RGB => (compressed) ? (Math.Max(8, ((width + 3) / 4) * ((height + 3) / 4) * 8)) : (width * height * 3),
        TPCBinaryEncoding.RGBA => (compressed) ? (Math.Max(16, ((width + 3) / 4) * ((height + 3) / 4) * 16)) : (width * height * 4),
        _ => throw new Exception("Unsupported encoding encountered"),
    };
}
