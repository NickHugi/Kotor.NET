namespace Kotor.NET.Resources.KotorTPC.Builder;

public class TPCLayerBuilder
{
    private TPCBuilder _tpcBuilder;
    private int _layer;

    internal TPCLayerBuilder(TPCBuilder tpcBuilder)
    {
        _tpcBuilder = tpcBuilder;
    }

    public TPCLayerBuilder SetLayer(int layer)
    {
        _layer = layer;
        return this;
    }

    public TPCLayerBuilder AddData(byte[] data, int mipmap)
    {
        _tpcBuilder.AddData(data, _layer, mipmap);
        return this;
    }

    public TPCBuilder Complete()
    {
        return _tpcBuilder;
    }
}
