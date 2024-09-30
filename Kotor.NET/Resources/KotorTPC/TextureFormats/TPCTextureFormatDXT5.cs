using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public sealed class TPCTextureFormatDXT5 : TPCTextureFormat
{
    public override bool IsCompressed => true;
    public override TPCBinaryEncoding BinaryEncoding => TPCBinaryEncoding.RGBA;

    internal TPCTextureFormatDXT5()
    {
    }

    public override int GetDataSize(int width, int height)
    {
        return Math.Max(16, width * height);
    }

    public override byte[] ConvertToRGBA(byte[] data)
    {
        throw new NotImplementedException();
    }
}
