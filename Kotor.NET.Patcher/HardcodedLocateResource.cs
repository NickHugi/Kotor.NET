using Kotor.NET.Patcher.For2DA;

namespace Kotor.NET.Patcher;

public class HardcodedLocateResource : ILocateResource
{
    public string FilePath = @"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II\override\appearance.2da";

    public string Locate()
    {
        return FilePath;
    }
    public byte[] Load()
    {
        return File.ReadAllBytes(FilePath);
    }
    public void Save(byte[] data)
    {
        File.WriteAllBytes(FilePath, data);
    }
}
