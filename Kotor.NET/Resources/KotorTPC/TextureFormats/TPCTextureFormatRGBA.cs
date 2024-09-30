using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public class TPCTextureFormatRGBA : TPCTextureFormat
{
    public override bool IsCompressed => false;
    public override TPCBinaryEncoding BinaryEncoding => TPCBinaryEncoding.RGBA;

    internal TPCTextureFormatRGBA()
    {
    }

    public override int GetDataSize(int width, int height)
    {
        return width * height * 4;
    }

    public override byte[] ConvertToRGBA(byte[] data)
    {
        throw new NotImplementedException();
    }
}
