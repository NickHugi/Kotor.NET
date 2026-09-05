using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.LocateResource;

namespace Kotor.NET.Patcher.FileOperation;

public class CreateOrModifyFileOperation : IFileOperation
{
    public byte[]? Read(string patchDirectory, Installation installation, ILocateContainer takeFrom, ResRef resref, ResourceType resourceType)
    {
        var source = takeFrom.Locate(patchDirectory, installation);
        var resource = source?.FirstOrDefault(x => x.ResRef == resref && x.Type == resourceType);
        return resource?.ReadData();
    }

    public void Write(string patchDirectory, Installation installation, ILocateContainer saveTo, ResRef resref, ResourceType resourceType, byte[] data)
    {
        var target = saveTo.Locate(patchDirectory, installation);

        if (target is null)
            throw new NotImplementedException();

        target.Write(resref.ToString(), resourceType, data);
    }
}
