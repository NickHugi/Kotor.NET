using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryTPC;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Resources.KotorTPC.Builder;

public class TPCBuilder
{
    private TPCTextureFormat? _format = null;
    private byte _layerCount = 1;
    private byte _mipmapCount = 1;
    private ushort _width = 256;
    private ushort _height = 256;
    private Dictionary<(int Layer, int Mipmap), byte[]> _data = new();

    public TPCBuilder SetFormat(TPCBinaryEncoding encoding, bool compressed)
    {
        var format = TPCTextureFormat.GetFrom(encoding, compressed);
        return SetFormat(format);
    }
    public TPCBuilder SetFormat(TPCTextureFormat format)
    {
        if (_format is not null && _data.Count() > 0)
            throw new ArgumentException("Cannot change the texture format after data has been added.");

        _format = format;
        return this;
    }

    public TPCBuilder SetLayerCount(byte count)
    {
        _layerCount = count;
        return this;
    }

    public TPCBuilder SetMipmapCount(byte count)
    {
        _mipmapCount = count;
        return this;
    }

    public TPCBuilder SetDimensions(ushort width, ushort height)
    {
        _width = width;
        _height = height;
        return this;
    }

    public TPCBuilder AddLayer(Action<TPCLayerBuilder> predicate)
    {
        var layerBuilder = new TPCLayerBuilder(this);
        predicate.Invoke(layerBuilder);
        return this;
    }
    public TPCLayerBuilder AddLayer()
    {
        return new(this);
    }

    public TPCBuilder AddData(byte[] data, int layer = 0, int mipmap = 0)
    {
        if (_format is null)
            throw new ArgumentException("A texture format must be set before adding data to the TPC.");
        if (_data.ContainsKey((layer, mipmap)))
            throw new ArgumentException("Mipmap data has already been assigned to that layer.");
        if (layer >= _layerCount)
            throw new ArgumentException("Trying to assign data to a layer that does not exist.");
        if (mipmap >= _mipmapCount)
            throw new ArgumentException("Trying to assign data to a mipmap that does not exist.");

        var width = TPCMipmap.GetWidth(_width, mipmap);
        var height = TPCMipmap.GetHeight(_height, mipmap);
        var size = _format.GetDataSize(width, height);

        if (data.Length != size)
            throw new ArgumentException($"The size of the texture data did not match what was expected at the given mipmap level. Expected: ${size} Actual: ${data.Length}");

        _data.Add((layer, mipmap), data);

        return this;
    }

    public TPC Build()
    {
        if (_format is null)
            throw new ArgumentException("A texture format must be set before building the TPC.");

        return new TPC(_width, _height, _layerCount, _mipmapCount, _format);
    }
}
