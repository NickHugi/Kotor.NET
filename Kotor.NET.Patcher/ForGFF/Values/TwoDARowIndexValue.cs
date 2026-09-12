using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.ForGFF.Values;

public class TwoDARowIndexValue<T> : IValue<T>  
{
    public required string ResRef { get; set; }
    public required string SearchColumn { get; set; }
    public required string SearchForCell { get; set; }

    public T Get(GFF gff, Installation installation, PatcherMemory memory)
    {
        var twoda = installation.Get2DA(ResRef);

        var row = twoda.GetRows().Single(x => x.GetCell(SearchColumn).AsString() == SearchForCell);
        var index = row.Index;

        if (typeof(T) == typeof(string))
            return (T)(object)index.ToString();

        if (typeof(T).IsPrimitive)
            return (T)Convert.ChangeType(index, typeof(T));

        throw new Exception(); // todo
    }
}
