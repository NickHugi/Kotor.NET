using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Parser;
using FluentAssertions.Execution;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Kotor.NET.Resources.KotorSSF;
using Kotor.NET.Patcher.Modifiers.ForSSF;
using Kotor.NET.Patcher.Parsers.LegacyINI.ForSSF;
using Kotor.NET.Patcher.Modifiers.ForSSF.ValueResolver;
using Kotor.NET.Patcher.Modifiers.ForSSF.Modifiers;

namespace Kotor.NET.Patcher.Tests.Parsers.LegacyINI.ForSSF;

public class ParseSetSoundTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [soundset.ssf]
                Battlecry 1=1
                Battlecry 2=2
                Battlecry 3=3
                Battlecry 4=4
                Battlecry 5=5
                Battlecry 6=6
                Selected 1=7
                Selected 2=8
                Selected 3=9
                Attack 1=10
                Attack 2=11
                Attack 3=12
                Pain 1=13
                Pain 2=14
                Low health=15
                Death=16
                Critical hit=17
                Target immune=18
                Place mine=19
                Disarm mine=20
                Stealth on=21
                Search=22
                Pick lock start=23
                Pick lock fail=24
                Pick lock done=25
                Leave party=26
                Rejoin party=2DAMEMORY5
                Poisoned=StrRef34
            """);

        var modifiers = ini["soundset.ssf"].Select(x => new ParseSetSound().Parse(x)).ToList();

        using (new AssertionScope())
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.IncludingAllRuntimeProperties());

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry1,
                Value = new ValueResolverForConstant() { Value = 1 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry2,
                Value = new ValueResolverForConstant() { Value = 2 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry3,
                Value = new ValueResolverForConstant() { Value = 3 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry4,
                Value = new ValueResolverForConstant() { Value = 4 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry5,
                Value = new ValueResolverForConstant() { Value = 5 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BattleCry6,
                Value = new ValueResolverForConstant() { Value = 6 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.Select1,
                Value = new ValueResolverForConstant() { Value = 7 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.Select2,
                Value = new ValueResolverForConstant() { Value = 8 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.Select3,
                Value = new ValueResolverForConstant() { Value = 9 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.AttackGrunt1,
                Value = new ValueResolverForConstant() { Value = 10 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.AttackGrunt2,
                Value = new ValueResolverForConstant() { Value = 11 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.AttackGrunt3,
                Value = new ValueResolverForConstant() { Value = 12 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.PainGrunt1,
                Value = new ValueResolverForConstant() { Value = 13 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.PainGrunt2,
                Value = new ValueResolverForConstant() { Value = 14 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.LowHealth,
                Value = new ValueResolverForConstant() { Value = 15 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.Dead,
                Value = new ValueResolverForConstant() { Value = 16 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.CriticalHit,
                Value = new ValueResolverForConstant() { Value = 17 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.TargetImmune,
                Value = new ValueResolverForConstant() { Value = 18 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.LayMine,
                Value = new ValueResolverForConstant() { Value = 19 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.DisarmMine,
                Value = new ValueResolverForConstant() { Value = 20 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BeginStealth,
                Value = new ValueResolverForConstant() { Value = 21 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BeginSearch,
                Value = new ValueResolverForConstant() { Value = 22 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.BeginUnlock,
                Value = new ValueResolverForConstant() { Value = 23 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.UnlockFailed,
                Value = new ValueResolverForConstant() { Value = 24 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.UnlockSuccess,
                Value = new ValueResolverForConstant() { Value = 25 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.PartySeparated,
                Value = new ValueResolverForConstant() { Value = 26 }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.PartyRejoined,
                Value = new ValueResolverForPatcherMemory() { Key = "2DAMEMORY5" }
            });

            modifiers.Should().ContainEquivalentOf(new SetSoundSSFModifier()
            {
                Sound = Sound.Poisoned,
                Value = new ValueResolverForPatcherMemory() { Key = "StrRef34" }
            });
        }
    }
}
