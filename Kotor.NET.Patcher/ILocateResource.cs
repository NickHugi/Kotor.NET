using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher;

public interface ILocateContainer
{
    public IEncapsulation? Locate(Installation installation);
}

public class KeyLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(Installation installation)
    {
        return installation.Chitin;
    }
}

public class IgnoreLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(Installation installation)
    {
        return null;
    }
}

public class ModuleLocateContainer : ILocateContainer
{
    public required string ModuleID { get; init; }

    public IEncapsulation? Locate(Installation installation)
    {
        return installation.Module(ModuleID);
    }
}

public class OverrideLocateContainer : ILocateContainer
{
    public IEncapsulation? Locate(Installation installation)
    {
        return installation.Override();
    }
}

public interface IFileOperation
{
    public byte[]? Read(Installation installation, ILocateContainer takeFrom, ResRef resref, ResourceType resourceType);
    public void Write(Installation installation, ILocateContainer saveTo, ResRef resref, ResourceType resourceType, byte[] data);
}

public class CreateOrReplaceFileOperation : IFileOperation
{
    public byte[]? Read(Installation installation, ILocateContainer takeFrom, ResRef resref, ResourceType resourceType)
    {
        return null;
    }

    public void Write(Installation installation, ILocateContainer saveTo, ResRef resref, ResourceType resourceType, byte[] data)
    {
        var target = saveTo.Locate(installation);

        if (target is null)
            throw new NotImplementedException();

        target.Write(resref.ToString(), resourceType, data);
    }
}

public class CreateOrModifyFileOperation : IFileOperation
{
    public byte[]? Read(Installation installation, ILocateContainer takeFrom, ResRef resref, ResourceType resourceType)
    {
        var source = takeFrom.Locate(installation);
        var resource = source?.FirstOrDefault(x => x.ResRef == resref && x.Type == resourceType);
        return resource?.ReadData();
    }

    public void Write(Installation installation, ILocateContainer saveTo, ResRef resref, ResourceType resourceType, byte[] data)
    {
        var target = saveTo.Locate(installation);

        if (target is null)
            throw new NotImplementedException();

        target.Write(resref.ToString(), resourceType, data);
    }
}
