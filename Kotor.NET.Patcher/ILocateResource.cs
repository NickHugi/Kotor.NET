namespace Kotor.NET.Patcher;

public interface ILocateResource
{
    public string Locate();
    public byte[] Load();
    public void Save(byte[] data);
}
