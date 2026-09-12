using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForGFF.FieldLocators;
using Kotor.NET.Patcher.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Modifiers;

public class SetStructModifier : IGFFModifier
{
    public required INodeResolver Parent { get; set; }
    public required IValue<int> StructID { get; set; }
    public required List<IGFFModifier> Modifiers { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var node = Parent.Locate(gff, cursor, installation, memory);
        var structID = StructID.Get(gff, installation, memory);

        if (node is FieldNode fieldNode)
        {
            fieldNode.Struct.SetStruct(fieldNode.Label, structID);
        }
        else if (node is ListStructNode listStructNode)
        {
            if (listStructNode.Index == -1)
            {
                listStructNode.List.Add(structID);
                node = new ListStructNode()
                {
                    List = listStructNode.List,
                    Index = listStructNode.List.Count() - 1
                };
            }
            else
            {
                var @struct = listStructNode.List.ElementAt(listStructNode.Index);
                @struct.ID = structID;
            }

        }

        Modifiers.ForEach(x => x.Apply(gff, node, installation, memory));
    }
}
