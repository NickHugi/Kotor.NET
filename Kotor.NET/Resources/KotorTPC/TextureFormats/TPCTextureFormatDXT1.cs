using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public sealed class TPCTextureFormatDXT1 : TPCTextureFormat
{
    public override bool IsCompressed => true;
    public override TPCBinaryEncoding BinaryEncoding => TPCBinaryEncoding.RGB;

    internal TPCTextureFormatDXT1()
    {
    }

    public override int GetDataSize(int width, int height)
    {
        return width * height / 2;
    }

    public override byte[] ConvertToRGBA(byte[] data)
    {
        throw new NotImplementedException();
    }
}
