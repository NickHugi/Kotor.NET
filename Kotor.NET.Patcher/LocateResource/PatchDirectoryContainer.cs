using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher.LocateResource;

public class PatchDirectoryLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(string patchDirectory, Installation installation)
    {
        return new FolderEncapsulation(patchDirectory);
    }
}
