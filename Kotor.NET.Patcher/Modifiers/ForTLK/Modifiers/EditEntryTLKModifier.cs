using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Modifiers.ForTLK.Modifiers;

public class EditEntryTLKModifier : ITLKModifier
{
    public required int IndexIntoTargetTLK { get; init; }
    public required int IndexIntoSourceTLK { get; init; }

    public void Apply(TLK target, TLK source, PatcherMemory memory, PatcherLogger log)
    {
        var entry = source.ElementAt(IndexIntoSourceTLK);

        target.ElementAt(IndexIntoTargetTLK).Text = entry.Text;
        target.ElementAt(IndexIntoTargetTLK).Sound = entry.Sound;
    }
}
