using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorUTC;

namespace Kotor.NET.Tests.Resources.KotorUTC;

public class TestUTC
{
    public static readonly string File1Filepath = "Resources/KotorUTC/test1.utc";

    [Fact]
    public void ReadsCorrectly()
    {
        var utc = UTC.FromFile(File1Filepath);

        Assert.Equal(4, utc.AppearanceID);
        Assert.Equal(1, utc.BodyVariation);
        Assert.Equal(15, utc.Charisma);
        Assert.Equal(3, utc.ChallengeRating);
        Assert.Equal("bastilla", utc.Comment);
        Assert.Equal(12, utc.Constitution);
        Assert.Equal("k_hbas_dialog", utc.Conversation);
        Assert.Equal(18, utc.CurrentForce);
        Assert.Equal(24, utc.CurrentHitPoints);
        Assert.Equal(18, utc.Dexterity);
        Assert.False(utc.Disarmable);
        Assert.Equal(2, utc.FactionID);
        Assert.Equal(31360, utc.FirstName.StringRef);
        Assert.Equal(18, utc.ForcePoints);
        Assert.Equal(1, utc.Gender);
        Assert.Equal(70, utc.Alignment);
        Assert.Equal(24, utc.HitPoints);
        Assert.Equal(10, utc.Intelligence);
        Assert.True(utc.Interruptable);
        Assert.False(utc.IsPC);
        Assert.Equal(-1, utc.LastName.StringRef);
        Assert.Equal(27, utc.MaxHitPoints);
        Assert.False(utc.Min1HP);
        Assert.Equal(0, utc.NaturalAC);
        Assert.True(utc.NoPermDeath);
        Assert.False(utc.NotReorienting);
        Assert.Equal(2, utc.PaletteID);
        Assert.False(utc.PartyInteract);
        Assert.Equal(11, utc.PerceptionRangeID);
        Assert.Equal(6, utc.RaceID);
        Assert.Equal("k_hen_attacked01", utc.OnAttacked);
        Assert.Equal("k_hen_damage01", utc.OnDamaged);
        Assert.Equal("", utc.OnDeath);
        Assert.Equal("k_hen_dialogue01", utc.OnDialogue);
        Assert.Equal("", utc.OnDisturbed);
        Assert.Equal("", utc.OnEndDialogue);
        Assert.Equal("k_hen_combend01", utc.OnEndRound);
        Assert.Equal("k_hen_heartbt01", utc.OnHeartbeat);
        Assert.Equal("k_hen_blocked01", utc.OnBlocked);
        Assert.Equal("k_hen_percept01", utc.OnNoticed);
        Assert.Equal("k_hen_spawn01", utc.OnSpawned);
        Assert.Equal("", utc.OnSpellAt);
        Assert.Equal("", utc.OnUserDefined);
        Assert.Equal(75, utc.SoundSetID);
        Assert.Equal(12, utc.Strength);
        Assert.Equal(0, utc.SubraceID);
        Assert.Equal("Bastila", utc.Tag);
        Assert.Equal("p_bastilla", utc.ResourceResRef);
        Assert.Equal(1, utc.TextureVariation);
        Assert.Equal(12, utc.Wisdom);
        Assert.Equal(0, utc.FortitudeBonus);
        Assert.Equal(0, utc.ReflexBonus);
        Assert.Equal(0, utc.WillBonus);

        var class1 = utc.Classes.FirstOrDefault();
        Assert.NotNull(class1);
        Assert.Equal(5, class1.ClassID);
        Assert.Equal(3, class1.Level);
        Assert.Equal(5, class1.ForcePowers.Count());
        Assert.Equal(6, class1.ForcePowers[0].ForcePowerID);
        Assert.Equal(18, class1.ForcePowers[1].ForcePowerID);
        Assert.Equal(23, class1.ForcePowers[2].ForcePowerID);
        Assert.Equal(46, class1.ForcePowers[3].ForcePowerID);
        Assert.Equal(49, class1.ForcePowers[4].ForcePowerID);

        Assert.Equal("g_a_clothes01", utc.Equipment.Armor.ResRef);

        Assert.Equal(9, utc.Feats.Count());
        Assert.Equal(94, utc.Feats[0].FeatID);
        Assert.Equal(11, utc.Feats[1].FeatID);
        Assert.Equal(98, utc.Feats[2].FeatID);
        Assert.Equal(55, utc.Feats[3].FeatID);
        Assert.Equal(107, utc.Feats[4].FeatID);
        Assert.Equal(3, utc.Feats[5].FeatID);
        Assert.Equal(39, utc.Feats[6].FeatID);
        Assert.Equal(43, utc.Feats[7].FeatID);
        Assert.Equal(44, utc.Feats[8].FeatID);

        Assert.Equal(2, utc.Items.Count());
        var item1 = utc.Items.ElementAtOrDefault(0);
        Assert.NotNull(item1);
        Assert.Equal("g_a_class8005", item1.ResRef);
        Assert.True(item1.Droppable);
        var item2 = utc.Items.ElementAtOrDefault(1);
        Assert.NotNull(item2);
        Assert.Equal("g_w_hvyblstr03", item2.ResRef);
        Assert.False(item2.Droppable);

        Assert.Equal(2, utc.ComputerUse);
        Assert.Equal(4, utc.Demolitions);
        Assert.Equal(6, utc.Stealth);
        Assert.Equal(8, utc.Awareness);
        Assert.Equal(10, utc.Persuade);
        Assert.Equal(12, utc.Repair);
        Assert.Equal(14, utc.Security);
        Assert.Equal(16, utc.TreatInjury);
    }
}
