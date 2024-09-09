using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorERF;

public class ERFResource
{
    public ResRef ResRef { get; set; }
    public ResourceType Type { get; set; }
    public byte[] Data { get; set; }
    public int Size => Data.Length;
    public int Index => _erf._resources.IndexOf(this);

    private ERF _erf;

    internal ERFResource(ERF erf, ResRef resref, ResourceType resourceType, byte[] data)
    {
        _erf = erf;
        ResRef = resref;
        Type = resourceType;
        Data = data;
    }

    public void Remove()
    {
        _erf._resources.Remove(this);
    }
}
