using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher.LocateResource;

public class ModuleLocateContainer : ILocateContainer
{
    public required string ModuleID { get; init; }

    public IEncapsulation? Locate(string patchDirectory, Installation installation)
    {
        return installation.Module(ModuleID);
    }
}
