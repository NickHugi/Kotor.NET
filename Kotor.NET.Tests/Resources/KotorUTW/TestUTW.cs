using Kotor.NET.Resources.KotorUTW;

namespace Kotor.NET.Tests.Resources.KotorUTW;

public class TestUTW
{
    public static readonly string File1Filepath = "Resources/KotorUTW/file1.utw";

    [Fact]
    public void Getters()
    {
        // Setup
        var utw = UTW.FromFile(File1Filepath);

        // Assert
        Assert.Equal(1, utw.AppearanceID);
        Assert.Equal("thisisacomment", utw.Comment);
        Assert.False(utw.HasMapNote);
        Assert.Equal(21757, utw.Name.StringRef);
        Assert.Equal(-1, utw.MapNote.StringRef);
        Assert.True(utw.MapNoteEnabled);
        Assert.Equal(5, utw.PaletteID);
        Assert.Equal("tar02_spcand", utw.Tag);
        Assert.Equal("tar02_spcand", utw.ResRef);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var utw = UTW.FromFile(File1Filepath);
        utw.AppearanceID = 0;
        utw.Comment = "comment";
        utw.HasMapNote = true;
        utw.Name.StringRef = 1;
        utw.MapNote.StringRef = 2;
        utw.MapNoteEnabled = false;
        utw.PaletteID = 3;
        utw.Tag = "tag";
        utw.ResRef = "resref";

        // Assert
        Assert.Equal(0, utw.AppearanceID);
        Assert.Equal("comment", utw.Comment);
        Assert.True(utw.HasMapNote);
        Assert.Equal(1, utw.Name.StringRef);
        Assert.Equal(2, utw.MapNote.StringRef);
        Assert.False(utw.MapNoteEnabled);
        Assert.Equal(3, utw.PaletteID);
        Assert.Equal("tag", utw.Tag);
        Assert.Equal("resref", utw.ResRef);
    }
}
