using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.LocateResource;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Patcher.CopyFiles;

public class PatchCopyFiles : IPatch
{
    public required ILocateContainer SourceContainer { get; init; }
    public required ILocateContainer TargetContainer { get; init; }
    public required List<CopyFileCommand> Commands { get; init; }

    public void Apply(Installation installation, PatcherMemory memory, string patchDirectory)
    {
        var source = SourceContainer.Locate(patchDirectory, installation);
        var target = TargetContainer.Locate(patchDirectory, installation);

        Commands.ForEach(x => x.Apply(source, target));
    }
}

public class CopyFileCommand
{
    public required string SourceFileName { get; init; }
    public required string TargetFileName { get; init; }
    public required ResourceType ResourceType { get; init; }

    public void Apply(IEncapsulation source, IEncapsulation target)
    {
        var resource = source.Single(x => x.ResRef == Path.GetFileNameWithoutExtension(SourceFileName) && x.Type == ResourceType);
        var data = resource.ReadData();

        target.Write(TargetFileName, ResourceType, data);
    }
}
