using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher.LocateResource;

public class KeyLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(string patchDirectory, Installation installation)
    {
        return installation.Chitin;
    }
}
