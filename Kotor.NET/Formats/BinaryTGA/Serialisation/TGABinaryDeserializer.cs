using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.BinaryTGA;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.Builder;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Formats.BinaryTPC.Serialisation;

public class TGABinaryDeserializer
{
    private TGABinary _binary { get; }

    public TGABinaryDeserializer(TGABinary binary)
    {
        _binary = binary;
    }

    public TPC Deserialize()
    {
        try
        {
            var compressed = false;
            var mipmaps = (byte)1;
            var width = (ushort)_binary.FileHeader.Width;
            var height = (ushort)_binary.FileHeader.Height;
            var layers = (byte)1;

            var encoding = _binary.FileHeader.BitsPerPixel switch
            {
                8 => TPCBinaryEncoding.Grayscale,
                24 => TPCBinaryEncoding.RGB,
                32 => TPCBinaryEncoding.RGBA,
            };

            return new TPCBuilder()
                .SetFormat(encoding, compressed)
                .SetDimensions(width, height)
                .SetLayerCount(layers)
                .SetMipmapCount(mipmaps)
                .AddLayer(layer =>
                {
                    layer.SetLayer(0);
                    
                    layer.AddData(_binary.ImageData.SelectMany(x => new byte[] { x[1], x[2], x[0], x[3] }).ToArray(), 0);
                })
                .Build();
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the TPC data.", e);
        }
    }
}
