using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorSSF;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Modifiers.ForTLK;

public class AddEntryTLKModifier
{
    public required string Key { get; init; }
    public required int IndexIntoSourceTLK { get; init; }

    public void Apply(TLK target, TLK source, PatcherMemory memory, PatcherLogger log)
    {
        var entry = source.ElementAt(IndexIntoSourceTLK);
        var indexIntoTargetTLK = source.Count();

        target.Add(entry.Text, entry.Sound);
        memory.Set(Key, indexIntoTargetTLK.ToString());
    }
}
