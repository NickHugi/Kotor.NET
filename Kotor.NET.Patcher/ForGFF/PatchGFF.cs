using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.FileOperation;
using Kotor.NET.Patcher.For2DA;
using Kotor.NET.Patcher.ForGFF.Modifiers;
using Kotor.NET.Patcher.LocateResource;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF;

public class PatchGFF : IPatch
{
    public required string ResRef { get; init; }
    public required ResourceType ResourceType { get; init; }
    public required ILocateContainer TakeFrom { get; set; }
    public required ILocateContainer SaveTo { get; set; }
    public required IFileOperation FileOperation { get; set; }
    public ICollection<IGFFModifier> Modifiers { get; set; } = [];

    public void Apply(Installation installation, PatcherMemory memory, string patchDirectory)
    {
        var data = FileOperation.Read(patchDirectory, installation, TakeFrom, ResRef, ResourceType);
        var gff = (data is null) ? (new GFF()) : GFF.FromBytes(data);
        
        Modifiers.ToList().ForEach(x => x.Apply(gff, new RootNode() { Struct = gff.Root }, installation, memory));

        data = GFF.ToBytes(gff);
        FileOperation.Write(patchDirectory, installation, SaveTo, ResRef, ResourceType, data);
    }
}
public class EditCreature : PatchGFF
{
    public EditCreature()
    {
        //TakeFrom = new HardcodedLocateResource();
        //SaveTo = new HardcodedLocateResource();
    }
}

public interface INode
{
    public bool IsStruct { get; }
}
public class RootNode : INode
{
    public bool IsStruct => true;

    public required GFFStruct Struct { get; init; }
}
public class FieldNode : INode
{
    public bool IsStruct => Struct.GetFields().FirstOrDefault(x => x.Label == Label).value is GFFStruct;

    public required GFFStruct Struct { get; init; }
    public required string Label { get; init; }
}
public class ListStructNode : INode
{
    public bool IsStruct => true;

    public required GFFList List { get; init; }
    public required int Index { get; init; }
}
