using Kotor.NET.Formats.BinaryTPC;

namespace Kotor.NET.Resources.KotorTPC.TextureFormats;

public abstract class TPCTextureFormat
{
    public static readonly TPCTextureFormatGrayscale Grayscale = new();
    public static readonly TPCTextureFormatRGB RGB = new();
    public static readonly TPCTextureFormatDXT1 DXT1 = new();
    public static readonly TPCTextureFormatRGBA RGBA = new();
    public static readonly TPCTextureFormatDXT5 DXT5 = new();

    public static TPCTextureFormat GetFrom(TPCBinaryEncoding encoding, bool compressed)
    {
        if (compressed)
        {
            return encoding switch
            {
                TPCBinaryEncoding.RGB => DXT1,
                TPCBinaryEncoding.RGBA => DXT5,
                _ => throw new NotImplementedException("The given encoding is invalid or only valid when compressed.")
            };
        }
        else
        {
            return encoding switch
            {
                TPCBinaryEncoding.Grayscale => Grayscale,
                TPCBinaryEncoding.RGB => RGB,
                TPCBinaryEncoding.RGBA => RGBA,
                _ => throw new NotImplementedException("The given encoding is invalid or does not support compression.")
            };
        }
    }


    public abstract bool IsCompressed { get; }
    public abstract TPCBinaryEncoding BinaryEncoding { get; }

    private protected TPCTextureFormat() { }

    public abstract int GetDataSize(int width, int height);
    public abstract byte[] ConvertToRGBA(byte[] data);
}
