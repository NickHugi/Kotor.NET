using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForGFF.FieldLocators;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Modifiers;

public class SetListModifier : IGFFModifier
{
    public required INodeResolver Parent { get; set; }
    public required List<IGFFModifier> Modifiers { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var node = Parent.Locate(gff, cursor, installation, memory);

        if (node is not FieldNode fieldNode)
            throw new NotImplementedException();

        if (fieldNode.Struct.GetList(fieldNode.Label) is null)
            fieldNode.Struct.SetList(fieldNode.Label);

        Modifiers.ForEach(x => x.Apply(gff, node, installation, memory));
    }
}
