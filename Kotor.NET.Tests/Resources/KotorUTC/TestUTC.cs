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
    public void Getters()
    {
        // Setup
        var utc = UTC.FromFile(File1Filepath);

        // Assert
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

    [Fact]
    public void Setters()
    {
        // Setup
        var utc = UTC.FromFile(File1Filepath);
        utc.AppearanceID = 1;
        utc.BodyVariation = 2;
        utc.Charisma = 3;
        utc.ChallengeRating = 4;
        utc.Comment = "comment";
        utc.Constitution = 5;
        utc.Conversation = "conversation";
        utc.CurrentForce = 6;
        utc.CurrentHitPoints = 7;
        utc.Dexterity = 8;
        utc.Disarmable = true;
        utc.FactionID = 9;
        utc.FirstName = new(10);
        utc.ForcePoints = 10;
        utc.Gender = 11;
        utc.Alignment = 12;
        utc.HitPoints = 13;
        utc.Intelligence = 14;
        utc.IsPC = true;
        utc.MaxHitPoints = 15;
        utc.Min1HP = true;
        utc.NaturalAC = 16;
        utc.NoPermDeath = true;
        utc.NotReorienting = true;
        utc.PaletteID = 17;
        utc.PartyInteract = true;
        utc.PerceptionRangeID = 18;
        utc.Plot = true;
        utc.RaceID = 19;
        utc.OnAttacked = "attacked";
        utc.OnDamaged = "damaged";
        utc.OnDeath = "death";
        utc.OnDialogue = "dialogue";
        utc.OnEndDialogue = "enddialogue";
        utc.OnHeartbeat = "heartbeat";
        utc.OnBlocked = "blocked";
        utc.OnNoticed = "noticed";
        utc.OnSpawned = "spawned";
        utc.OnSpellAt = "spellat";
        utc.OnUserDefined = "userdefined";
        utc.SoundSetID = 20;
        utc.Strength = 21;
        utc.SubraceID = 22;
        utc.Tag = "tag";
        utc.ResourceResRef = "resref";
        utc.TextureVariation = 23;
        utc.Wisdom = 24;
        utc.FortitudeBonus = 25;
        utc.ReflexBonus = 26;
        utc.WillBonus = 27;

        var class1 = utc.Classes.First();
        class1.ClassID = 1;
        class1.Level = 2;
        class1.ForcePowers.Clear();
        class1.ForcePowers.Add(3);

        utc.Items[0].Remove();
        utc.Items[0].ResRef = "resref";
        utc.Items[0].Droppable = true;

        utc.Feats.Clear();
        utc.Feats.Add(1);

        utc.Equipment.Armor.Remove();
        utc.Equipment.HeadGear.ResRef = "headgear";
        utc.Equipment.HeadGear.Droppable = true;

        // Assert
        Assert.Equal(1, utc.AppearanceID);
        Assert.Equal(2, utc.BodyVariation);
        Assert.Equal(3, utc.Charisma);
        Assert.Equal(4, utc.ChallengeRating);
        Assert.Equal("comment", utc.Comment);
        Assert.Equal(5, utc.Constitution);
        Assert.Equal("conversation", utc.Conversation);
        Assert.Equal(6, utc.CurrentForce);
        Assert.Equal(7, utc.CurrentHitPoints);
        Assert.Equal(8, utc.Dexterity);
        Assert.True(utc.Disarmable);
        Assert.Equal(9, utc.FactionID);
        Assert.Equal(10, utc.FirstName.StringRef);
        Assert.Equal(10, utc.ForcePoints);
        Assert.Equal(11, utc.Gender);
        Assert.Equal(12, utc.Alignment);
        Assert.Equal(13, utc.HitPoints);
        Assert.Equal(14, utc.Intelligence);
        Assert.True(utc.IsPC);
        Assert.Equal(15, utc.MaxHitPoints);
        Assert.True(utc.Min1HP);
        Assert.Equal(16, utc.NaturalAC);
        Assert.True(utc.NoPermDeath);
        Assert.True(utc.NotReorienting);
        Assert.Equal(17, utc.PaletteID);
        Assert.True(utc.PartyInteract);
        Assert.Equal(18, utc.PerceptionRangeID);
        Assert.True(utc.Plot);
        Assert.Equal(19, utc.RaceID);
        Assert.Equal("attacked", utc.OnAttacked);
        Assert.Equal("damaged", utc.OnDamaged);
        Assert.Equal("death", utc.OnDeath);
        Assert.Equal("dialogue", utc.OnDialogue);
        Assert.Equal("enddialogue", utc.OnEndDialogue);
        Assert.Equal("heartbeat", utc.OnHeartbeat);
        Assert.Equal("blocked", utc.OnBlocked);
        Assert.Equal("noticed", utc.OnNoticed);
        Assert.Equal("spawned", utc.OnSpawned);
        Assert.Equal("spellat", utc.OnSpellAt);
        Assert.Equal("userdefined", utc.OnUserDefined);
        Assert.Equal(20, utc.SoundSetID);
        Assert.Equal(21, utc.Strength);
        Assert.Equal(22, utc.SubraceID);
        Assert.Equal("tag", utc.Tag);
        Assert.Equal("resref", utc.ResourceResRef);
        Assert.Equal(23, utc.TextureVariation);
        Assert.Equal(24, utc.Wisdom);
        Assert.Equal(25, utc.FortitudeBonus);
        Assert.Equal(26, utc.ReflexBonus);
        Assert.Equal(27, utc.WillBonus);

        Assert.NotNull(class1);
        Assert.Equal(1, class1.ClassID);
        Assert.Equal(2, class1.Level);
        Assert.Single(class1.ForcePowers);
        Assert.Equal(3, class1.ForcePowers[0].ForcePowerID);

        Assert.False(utc.Equipment.Armor.Exists());
        Assert.True(utc.Equipment.HeadGear.Exists());
        Assert.Equal("headgear", utc.Equipment.HeadGear.ResRef);
        Assert.True(utc.Equipment.HeadGear.Droppable);

        Assert.Single(utc.Feats);
        Assert.Equal(1, utc.Feats[0].FeatID);

        Assert.Equal(1, utc.Items.Count());
        var item1 = utc.Items[0];
        Assert.Equal("resref", item1.ResRef);
        Assert.True(item1.Droppable);
    }
}
