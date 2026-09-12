using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Values;

public interface IValue<T>
{
    public T Get(GFF gff, Installation installation, PatcherMemory memory);
}
