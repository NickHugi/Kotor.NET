using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Modifiers.ForSSF;
using Kotor.NET.Patcher.Modifiers.ForSSF.ValueResolver;
using Kotor.NET.Resources.KotorSSF;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.ForSSF;

public class ParseSetSound
{
    public SetSoundSSFModifier Parse(KeyData data)
    {
        var sound = data.KeyName switch
        {
            "Battlecry 1" => Sound.BattleCry1,
            "Battlecry 2" => Sound.BattleCry2,
            "Battlecry 3" => Sound.BattleCry3,
            "Battlecry 4" => Sound.BattleCry4,
            "Battlecry 5" => Sound.BattleCry5,
            "Battlecry 6" => Sound.BattleCry6,
            "Selected 1" => Sound.Select1,
            "Selected 2" => Sound.Select2,
            "Selected 3" => Sound.Select3,
            "Attack 1" => Sound.AttackGrunt1,
            "Attack 2" => Sound.AttackGrunt2,
            "Attack 3" => Sound.AttackGrunt3,
            "Pain 1" => Sound.PainGrunt1,
            "Pain 2" => Sound.PainGrunt2,
            "Low health" => Sound.LowHealth,
            "Death" => Sound.Dead,
            "Critical hit" => Sound.CriticalHit,
            "Target immune" => Sound.TargetImmune,
            "Place mine" => Sound.LayMine,
            "Disarm mine" => Sound.DisarmMine,
            "Stealth on" => Sound.BeginStealth,
            "Search" => Sound.BeginSearch,
            "Pick lock start" => Sound.BeginUnlock,
            "Pick lock fail" => Sound.UnlockFailed,
            "Pick lock done" => Sound.UnlockSuccess,
            "Leave party" => Sound.PartySeparated,
            "Rejoin party" => Sound.PartyRejoined,
            "Poisoned" => Sound.Poisoned,
            _ => throw new Exception() // TODO
        };
        var value = ParseSoundValue(data.Value);

        return new()
        {
            Sound = sound,
            Value = value
        };
    }

    private BaseValueResolver ParseSoundValue(string value)
    {
        if (value.StartsWith("StrRef"))
        {
            return new ValueResolverForPatcherMemory()
            {
                Key = value
            };
        }
        else if (value.StartsWith("2DAMEMORY"))
        {
            return new ValueResolverForPatcherMemory()
            {
                Key = value
            };
        }
        else
        {
            return new ValueResolverForConstant()
            {
                Value = uint.Parse(value)
            };
        }
    }
}
