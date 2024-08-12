using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorUTE;

namespace Kotor.NET.Tests.Resources.KotorUTE;

public class TestUTE
{
    public static readonly string File1Filepath = "Resources/KotorUTE/file1.ute";

    [Fact]
    public void Getters()
    {
        // Setup
        var ute = UTE.FromFile(File1Filepath);

        // Assert
        Assert.Equal("106drdambush", ute.ResRef);
        Assert.Equal("106drdambush", ute.Tag);
        Assert.Equal(77428, ute.Name.StringRef);
        Assert.False(ute.Active);
        Assert.Equal(0, ute.Difficulty);
        Assert.Equal(1, ute.DifficultyIndex);
        Assert.Equal(1U, ute.FactionID);
        Assert.Equal(3, ute.MaxCreatures);
        Assert.True(ute.PlayerOnly);
        Assert.Equal(3, ute.RecommendedCreatures);
        Assert.False(ute.Reset);
        Assert.Equal(60, ute.ResetTime);
        Assert.Equal(0, ute.Respawns);
        Assert.Equal(1, ute.SpawnOption);
        Assert.Equal("t_spawndrd", ute.OnEntered);
        Assert.Equal("", ute.OnExit);
        Assert.Equal("", ute.OnExhausted);
        Assert.Equal("", ute.OnHeartbeat);
        Assert.Equal("", ute.OnUserDefined);
        Assert.Equal(6, ute.PaletteID);
        Assert.Equal("", ute.Comment);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var ute = UTE.FromFile(File1Filepath);
        ute.ResRef = "resref";
        ute.Tag = "tag";
        ute.Name.StringRef = 54321;
        ute.Active = true;
        ute.Difficulty = 10;
        ute.DifficultyIndex = 20;
        ute.FactionID = 4U;
        ute.MaxCreatures = 15;
        ute.PlayerOnly = true;
        ute.RecommendedCreatures = 8;
        ute.Reset = true;
        ute.ResetTime = 30;
        ute.Respawns = 5;
        ute.SpawnOption = 2;
        ute.OnEntered = "entered";
        ute.OnExit = "exit";
        ute.OnExhausted = "exhausted";
        ute.OnHeartbeat = "heartbeat";
        ute.OnUserDefined = "userdefined";
        ute.PaletteID = 3;
        ute.Comment = "comment";

        // Assert
        Assert.Equal("resref", ute.ResRef);
        Assert.Equal("tag", ute.Tag);
        Assert.Equal(54321, ute.Name.StringRef);
        Assert.True(ute.Active);
        Assert.Equal(10, ute.Difficulty);
        Assert.Equal(20, ute.DifficultyIndex);
        Assert.Equal(4U, ute.FactionID);
        Assert.Equal(15, ute.MaxCreatures);
        Assert.True(ute.PlayerOnly);
        Assert.Equal(8, ute.RecommendedCreatures);
        Assert.True(ute.Reset);
        Assert.Equal(30, ute.ResetTime);
        Assert.Equal(5, ute.Respawns);
        Assert.Equal(2, ute.SpawnOption);
        Assert.Equal("entered", ute.OnEntered);
        Assert.Equal("exit", ute.OnExit);
        Assert.Equal("exhausted", ute.OnExhausted);
        Assert.Equal("heartbeat", ute.OnHeartbeat);
        Assert.Equal("userdefined", ute.OnUserDefined);
        Assert.Equal(3, ute.PaletteID);
        Assert.Equal("comment", ute.Comment);
    }
}
