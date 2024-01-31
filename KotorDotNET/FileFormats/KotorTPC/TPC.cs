using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.FileFormats.KotorTPC
{
    public class TPC
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TPCTextureFormat Format {  get; set; }
        public string TXI { get; set; } = "";

        private List<byte[]> _mipmapData = new();

        public TPC(int width, int height, TPCTextureFormat format, byte[] data)
        {
            Width = width;
            Height = height;
            Format = format;
            _mipmapData.Add(data);
        }

        public Mipmap GetMipmap(int index)
        {
            return new Mipmap
            {
                Width = (int)Math.Sqrt(Width),
                Height = (int)Math.Sqrt(Height),
                Data = _mipmapData[index],
            };
        }
    }

    public class Mipmap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Data { get; set; }

        public int Size => Data.Length;
    }

    public enum TPCTextureFormat
    {
        Greyscale,
        RGB,
        RGBA,
        DXT1,
        DXT5
    }
}
