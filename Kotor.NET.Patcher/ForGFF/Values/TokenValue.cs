using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Values;

public class TokenValue<T> : IValue<T>
{
    public required string Token { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        return memory.Get<T>(Token);
    }
}
