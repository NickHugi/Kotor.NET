using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.ForSSF.ValueResolver;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Patcher.Modifiers.ForSSF.Modifiers;

public class SetSoundSSFModifier : ISSFModifier
{
    public required Sound Sound { get; set; }
    public required BaseValueResolver Value { get; set; }

    public void Apply(SSF ssf, PatcherMemory memory, PatcherLogger log)
    {
        var value = Value.Resolve(ssf, memory);

        _ = Sound switch
        {
            Sound.BattleCry1 => ssf.Battlecry1 = value,
            Sound.BattleCry2 => ssf.Battlecry2 = value,
            Sound.BattleCry3 => ssf.Battlecry3 = value,
            Sound.BattleCry4 => ssf.Battlecry4 = value,
            Sound.BattleCry5 => ssf.Battlecry5 = value,
            Sound.BattleCry6 => ssf.Battlecry6 = value,
            Sound.Select1 => ssf.Select1 = value,
            Sound.Select2 => ssf.Select2 = value,
            Sound.Select3 => ssf.Select3 = value,
            Sound.AttackGrunt1 => ssf.AttackGrunt1 = value,
            Sound.AttackGrunt2 => ssf.AttackGrunt2 = value,
            Sound.AttackGrunt3 => ssf.AttackGrunt3 = value,
            Sound.PainGrunt1 => ssf.PainGrunt1 = value,
            Sound.PainGrunt2 => ssf.PainGrunt2 = value,
            Sound.LowHealth => ssf.LowHealth = value,
            Sound.Dead => ssf.Dead = value,
            Sound.CriticalHit => ssf.CriticalHit = value,
            Sound.TargetImmune => ssf.TargetImmune = value,
            Sound.LayMine => ssf.LayMine = value,
            Sound.DisarmMine => ssf.DisarmMine = value,
            Sound.BeginStealth => ssf.BeginStealth = value,
            Sound.BeginSearch => ssf.BeginSearch = value,
            Sound.BeginUnlock => ssf.BeginUnlock = value,
            Sound.UnlockFailed => ssf.UnlockFailed = value,
            Sound.UnlockSuccess => ssf.UnlockSuccess = value,
            Sound.PartySeparated => ssf.PartySeparated = value,
            Sound.PartyRejoined => ssf.PartyRejoined = value,
            Sound.Poisoned => ssf.Poisoned = value,
            _ => throw new Exception() // TODO
        };
    }
}
