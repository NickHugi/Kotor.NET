using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Modifiers;

public interface IGFFModifier
{
    public void Apply(GFF gff, INode cursor, Installation installation, PatcherMemory memory);
}
