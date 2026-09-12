using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForGFF.FieldLocators;
using Kotor.NET.Patcher.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Modifiers;

public class EditUInt32Modifier : IGFFModifier
{
    public required INodeResolver Field { get; set; }
    public required IValue<uint> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        fieldNode.Struct.SetUInt32(fieldNode.Label, value);
    }
}
