using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher.LocateResource;

public class OverrideLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(string patchDirectory, Installation installation)
    {
        return installation.Override();
    }
}
