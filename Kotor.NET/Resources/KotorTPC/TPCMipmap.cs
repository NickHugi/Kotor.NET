namespace Kotor.NET.Resources.KotorTPC;

public class TPCMipmap
{
    public int Width => TPCMipmap.GetWidth(_tpc.Width, Level);
    public int Height => TPCMipmap.GetHeight(_tpc.Width, Level);
    public int Level => _layer._mipmaps.IndexOf(this);
    public byte[] Data { get; }

    private TPC _tpc;
    private TPCLayer _layer;

    internal TPCMipmap(TPC tpc, TPCLayer layer)
    {
        _tpc = tpc;
        _layer = layer;
        _layer._mipmaps.Add(this);

        var size = _tpc._textureFormat.GetDataSize(Width, Height);
        Data = new byte[size];
    }

    public static int GetWidth(int width, int mipmap)
    {
        return width / (int)Math.Pow(2, mipmap);
    }
    public static int GetHeight(int height, int mipmap)
    {
        return GetWidth(height, mipmap);
    }
}
