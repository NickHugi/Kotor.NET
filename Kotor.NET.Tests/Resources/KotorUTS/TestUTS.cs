using Kotor.NET.Resources.KotorUTS;

namespace Kotor.NET.Tests.Resources.KotorUTS;

public class TestUTS
{
    public static readonly string File1Filepath = "Resources/KotorUTS/file1.uts";

    [Fact]
    public void Getters()
    {
        // Setup
        var uts = UTS.FromFile(File1Filepath);

        // Assert
        Assert.True(uts.Active);
        Assert.Equal("", uts.Comment);
        Assert.True(uts.Continuous);
        Assert.Equal(1, uts.Elevation);
        Assert.Equal(20000U, uts.Interval);
        Assert.Equal(5000U, uts.IntervalVariation);
        Assert.Equal(45963, uts.Name.StringRef);
        Assert.False(uts.Looping);
        Assert.Equal(1, uts.MinDistance);
        Assert.Equal(10, uts.MaxDistance);
        Assert.Equal(6, uts.PaletteID);
        Assert.Equal(3, uts.PitchVariation);
        Assert.False(uts.Positional);
        Assert.Equal(21, uts.Priority);
        Assert.True(uts.Random);
        Assert.False(uts.RandomPosition);
        Assert.Equal(0, uts.RandomRangeX);
        Assert.Equal(0, uts.RandomRangeY);
        Assert.Equal("ApartmentWalla", uts.Tag);
        Assert.Equal("apartmentwalla", uts.ResRef);
        Assert.Equal(52, uts.Volume);
        Assert.Equal(15, uts.VolumeVariation);

        Assert.Equal(2, uts.Sounds.Count);
        var sound0 = uts.Sounds[0];
        Assert.Equal("as_pl_tarwal2_01", sound0.ResRef);
        var sound1 = uts.Sounds[1];
        Assert.Equal("as_pl_tarwal2_02", sound1.ResRef);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var uts = UTS.FromFile(File1Filepath);
        uts.Active = false;
        uts.Comment = "comment";
        uts.Continuous = false;
        uts.Elevation = 0;
        uts.Interval = 1;
        uts.IntervalVariation = 2;
        uts.Name.StringRef = 3;
        uts.Looping = true;
        uts.MaxDistance = 4;
        uts.MinDistance = 5;
        uts.PaletteID = 6;
        uts.PitchVariation = 7;
        uts.Positional = true;
        uts.Priority = 8;
        uts.Random = false;
        uts.RandomPosition = true;
        uts.RandomRangeX = 9;
        uts.RandomRangeY = 10;
        uts.Tag = "tag";
        uts.ResRef = "resref";
        uts.Volume = 11;
        uts.VolumeVariation = 12;

        uts.Sounds.Clear();
        uts.Sounds.Add("sound0");
        uts.Sounds.Add("sound1");
        uts.Sounds.Add("sound2");

        // Assert
        Assert.False(uts.Active);
        Assert.Equal("comment", uts.Comment);
        Assert.False(uts.Continuous);
        Assert.Equal(0, uts.Elevation);
        Assert.Equal(1U, uts.Interval);
        Assert.Equal(2U, uts.IntervalVariation);
        Assert.Equal(3, uts.Name.StringRef);
        Assert.True(uts.Looping);
        Assert.Equal(4, uts.MaxDistance);
        Assert.Equal(5, uts.MinDistance);
        Assert.Equal(6, uts.PaletteID);
        Assert.Equal(7, uts.PitchVariation);
        Assert.True(uts.Positional);
        Assert.Equal(8, uts.Priority);
        Assert.False(uts.Random);
        Assert.True(uts.RandomPosition);
        Assert.Equal(9, uts.RandomRangeX);
        Assert.Equal(10, uts.RandomRangeY);
        Assert.Equal("tag", uts.Tag);
        Assert.Equal("resref", uts.ResRef);
        Assert.Equal(11, uts.Volume);
        Assert.Equal(12, uts.VolumeVariation);

        Assert.Equal(3, uts.Sounds.Count);
        var sound0 = uts.Sounds[0];
        Assert.Equal("sound0", sound0.ResRef);
        var sound1 = uts.Sounds[1];
        Assert.Equal("sound1", sound1.ResRef);
        var sound2 = uts.Sounds[2];
        Assert.Equal("sound2", sound2.ResRef);
    }
}
