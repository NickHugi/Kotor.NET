using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.FieldLocators;

public interface INodeResolver
{
    public INode Locate(GFF gff, INode cursor, Installation installation, PatcherMemory memory);
}
