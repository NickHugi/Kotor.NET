using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher.ForGFF.FieldLocators;
using Kotor.NET.Patcher.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Modifiers;

public class EditLocalizedStringStringRefModifier : IGFFModifier
{
    public required INodeResolver Field { get; set; }
    public required IValue<int> Value { get; set; }

    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory)
    {
        var field = Field.Locate(gff, cursor, installation, memory);
        var value = Value.Get(gff, installation, memory);

        if (field is not FieldNode fieldNode)
            throw new NotImplementedException();

        var locstring = fieldNode.Struct.GetLocalisedString(fieldNode.Label) ?? new();
        locstring.StringRef = value;
        fieldNode.Struct.SetLocalisedString(fieldNode.Label, locstring);
    }
}
