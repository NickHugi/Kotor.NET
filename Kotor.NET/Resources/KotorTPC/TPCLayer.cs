namespace Kotor.NET.Resources.KotorTPC;

public class TPCLayer
{
    internal List<TPCMipmap> _mipmaps = new();

    private TPC _tpc;

    internal TPCLayer(TPC tpc, int mipmaps)
    {
        _tpc = tpc;

        for (int i = 0; i < mipmaps; i++)
        {
            new TPCMipmap(_tpc, this);
        }
    }
}
