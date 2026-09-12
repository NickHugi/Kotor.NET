using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Values;

public class ConstantValue<T> : IValue<T>
{
    public required T Value { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        return Value;
    }
}
