using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public class TPCTextureFormatGrayscale : TPCTextureFormat
{
    public override bool IsCompressed => false;
    public override TPCBinaryEncoding BinaryEncoding => TPCBinaryEncoding.Grayscale;

    internal TPCTextureFormatGrayscale()
    {
    }

    public override int GetDataSize(int width, int height)
    {
        return width * height;
    }

    public override byte[] ConvertToRGBA(byte[] data)
    {
        throw new NotImplementedException();
    }
}
