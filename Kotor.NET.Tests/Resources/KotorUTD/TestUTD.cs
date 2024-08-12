using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorUTD;

namespace Kotor.NET.Tests.Resources.KotorUTD;

public class TestUTD
{
    public static readonly string File1Filepath = "Resources/KotorUTD/file1.utd";

    [Fact]
    public void Getters()
    {
        // Setup
        var utd = UTD.FromFile(File1Filepath);

        // Assert
        Assert.Equal(0, utd.AnimationState);
        Assert.False(utd.AutoRemoveKey);
        Assert.Equal(0, utd.CloseLockDC);
        Assert.Equal("Peragus Door 1", utd.Comment);
        Assert.Equal("", utd.Conversation);
        Assert.Equal(60, utd.CurrentHP);
        Assert.Equal(28, utd.DisarmDC);
        Assert.Equal(1L, utd.FactionID);
        Assert.Equal(28, utd.Fortitude);
        Assert.Equal(64, utd.AppearanceID);
        Assert.Equal(5, utd.Hardness);
        Assert.Equal(20, utd.HP);
        Assert.True(utd.Interruptable);
        Assert.Equal("", utd.KeyName);
        Assert.False(utd.KeyRequired);
        Assert.False(utd.Lockable);
        Assert.False(utd.Locked);
        Assert.Equal(124820, utd.Name.StringRef);
        Assert.False(utd.Min1HP);
        Assert.False(utd.NotBlastable);
        Assert.Equal("", utd.OnClick);
        Assert.Equal("", utd.OnClosed);
        Assert.Equal("", utd.OnDamaged);
        Assert.Equal("", utd.OnDeath);
        Assert.Equal("", utd.OnFailToOpen);
        Assert.Equal("", utd.OnHeartbeat);
        Assert.Equal("", utd.OnMeleeAttacked);
        Assert.Equal("", utd.OnOpen);
        Assert.Equal("", utd.OnUnlock);
        Assert.Equal("", utd.OnUserDefined);
        Assert.Equal(28, utd.OpenLockDC);
        Assert.Equal(1, utd.OpenLockDifficulty);
        Assert.Equal(0, utd.OpenLockDifficultyModifier);
        Assert.Equal(0, utd.OpenState);
        Assert.Equal(1, utd.PaletteID);
        Assert.False(utd.Plot);
        Assert.Equal(0, utd.Reflex);
        Assert.False(utd.Static);
        Assert.Equal("PeragusDoor1", utd.Tag);
        Assert.Equal("door_per002", utd.ResRef);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var utd = UTD.FromFile(File1Filepath);
        utd.AnimationState = 5;
        utd.AutoRemoveKey = true;
        utd.CloseLockDC = 20;
        utd.Comment = "comment";
        utd.Conversation = "conv";
        utd.CurrentHP = 75;
        utd.DisarmDC = 25;
        utd.FactionID = 789012;
        utd.Fortitude = 15;
        utd.AppearanceID = 10;
        utd.Hardness = 12;
        utd.HP = 150;
        utd.Interruptable = true;
        utd.KeyName = "key";
        utd.KeyRequired = true;
        utd.Lockable = true;
        utd.Locked = true;
        utd.Name.StringRef = 99999;
        utd.Min1HP = true;
        utd.NotBlastable = true;
        utd.OnClick = "onclick";
        utd.OnClosed = "onclosed";
        utd.OnDamaged = "ondamaged";
        utd.OnDeath = "ondeath";
        utd.OnFailToOpen = "onfail";
        utd.OnHeartbeat = "onheartbeat";
        utd.OnMeleeAttacked = "onmelee";
        utd.OnOpen = "onopen";
        utd.OnUnlock = "onunlock";
        utd.OnUserDefined = "onuserdef";
        utd.OpenLockDC = 35;
        utd.OpenLockDifficulty = 40;
        utd.OpenLockDifficultyModifier = 5;
        utd.OpenState = 3;
        utd.PaletteID = 9;
        utd.Plot = true;
        utd.Reflex = 18;
        utd.Static = true;
        utd.Tag = "tag";
        utd.ResRef = "resref";

        // Assert
        Assert.Equal(5, utd.AnimationState);
        Assert.True(utd.AutoRemoveKey);
        Assert.Equal(20, utd.CloseLockDC);
        Assert.Equal("comment", utd.Comment);
        Assert.Equal("conv", utd.Conversation);
        Assert.Equal(75, utd.CurrentHP);
        Assert.Equal(25, utd.DisarmDC);
        Assert.Equal(789012L, utd.FactionID);
        Assert.Equal(15, utd.Fortitude);
        Assert.Equal(10, utd.AppearanceID);
        Assert.Equal(12, utd.Hardness);
        Assert.Equal(150, utd.HP);
        Assert.True(utd.Interruptable);
        Assert.Equal("key", utd.KeyName);
        Assert.True(utd.KeyRequired);
        Assert.True(utd.Lockable);
        Assert.True(utd.Locked);
        Assert.Equal(99999, utd.Name.StringRef);
        Assert.True(utd.Min1HP);
        Assert.True(utd.NotBlastable);
        Assert.Equal("onclick", utd.OnClick);
        Assert.Equal("onclosed", utd.OnClosed);
        Assert.Equal("ondamaged", utd.OnDamaged);
        Assert.Equal("ondeath", utd.OnDeath);
        Assert.Equal("onfail", utd.OnFailToOpen);
        Assert.Equal("onheartbeat", utd.OnHeartbeat);
        Assert.Equal("onmelee", utd.OnMeleeAttacked);
        Assert.Equal("onopen", utd.OnOpen);
        Assert.Equal("onunlock", utd.OnUnlock);
        Assert.Equal("onuserdef", utd.OnUserDefined);
        Assert.Equal(35, utd.OpenLockDC);
        Assert.Equal(40, utd.OpenLockDifficulty);
        Assert.Equal(5, utd.OpenLockDifficultyModifier);
        Assert.Equal(3, utd.OpenState);
        Assert.Equal(9, utd.PaletteID);
        Assert.True(utd.Plot);
        Assert.Equal(18, utd.Reflex);
        Assert.True(utd.Static);
        Assert.Equal("tag", utd.Tag);
        Assert.Equal("resref", utd.ResRef);
    }
}
