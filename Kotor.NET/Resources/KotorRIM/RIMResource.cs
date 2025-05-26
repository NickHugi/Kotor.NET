using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorRIM;

public class RIMResource
{
    public ResRef ResRef { get; set; }
    public ResourceType Type { get; set; }
    public byte[] Data { get; set; }
    public int Size => Data.Length;
    public int Index => _rim._resources.IndexOf(this);

    private RIM _rim;

    internal RIMResource(RIM rim, ResRef resref, ResourceType resourceType, byte[] data)
    {
        _rim = rim;
        ResRef = resref;
        Type = resourceType;
        Data = data;
    }

    public void Remove()
    {
        _rim._resources.Remove(this);
    }
}
