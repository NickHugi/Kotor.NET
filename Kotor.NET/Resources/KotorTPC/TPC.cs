using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Resources.KotorTPC;

public class TPC
{
    public IReadOnlyList<TPCLayer> Layer => _layers.AsReadOnly();
    public ushort Width { get; }
    public ushort Height { get; }
    public byte MipmapCount { get; }
    public byte LayerCount { get; }
    public TPCTextureFormat TextureFormat => _textureFormat;

    internal List<TPCLayer> _layers = new();
    internal TPCTextureFormat _textureFormat;

    public TPC(ushort width, ushort height, byte layers, byte mipmaps, TPCTextureFormat format)
    {
        Width = width;
        Height = height;
        MipmapCount = mipmaps;
        LayerCount = layers;
        _textureFormat = format;

        for (int i = 0; i < layers; i++)
        {
            _layers.Add(new(this, mipmaps));
        }
    }
}
