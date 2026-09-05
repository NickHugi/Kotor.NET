using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.LocateResource;

namespace Kotor.NET.Patcher.FileOperation;

public interface IFileOperation
{
    public byte[]? Read(string patchDirectory, Installation installation, ILocateContainer takeFrom, ResRef resref, ResourceType resourceType);
    public void Write(string patchDirectory, Installation installation, ILocateContainer saveTo, ResRef resref, ResourceType resourceType, byte[] data);
}
