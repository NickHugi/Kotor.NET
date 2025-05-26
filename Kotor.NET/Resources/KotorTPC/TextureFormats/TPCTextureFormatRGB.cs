using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public class TPCTextureFormatRGB : TPCTextureFormat
{
    public override bool IsCompressed => false;
    public override TPCBinaryEncoding BinaryEncoding => TPCBinaryEncoding.RGB;

    internal TPCTextureFormatRGB()
    {
    }

    public override int GetDataSize(int width, int height)
    {
        return width *height * 3;
    }

    public override byte[] ConvertToRGBA(byte[] data)
    {
        throw new NotImplementedException();
    }
}
