using Kotor.NET.Resources.KotorUTT;

namespace Kotor.NET.Tests.Resources.KotorUTT;

public class TestUTT
{
    public static readonly string File1Filepath = "Resources/KotorUTT/file1.utt";

    [Fact]
    public void Getters()
    {
        // Setup
        var utt = UTT.FromFile(File1Filepath);

        // Assert
        Assert.False(utt.AutoRemoveKey);
        Assert.Equal("", utt.Comment);
        Assert.Equal(1, utt.CursorID);
        Assert.Equal(0, utt.DisarmDC);
        Assert.Equal(1U, utt.FactionID);
        Assert.Equal(0, utt.HighlightHeight);
        Assert.Equal("", utt.KeyName);
        Assert.Equal(21739, utt.Name.StringRef);
        Assert.Equal("", utt.OnClick);
        Assert.Equal("", utt.OnDisarm);
        Assert.Equal("", utt.OnTrapTriggered);
        Assert.Equal(5, utt.PaletteID);
        Assert.Equal("", utt.OnHeartbeat);
        Assert.Equal("", utt.OnEnter);
        Assert.Equal("", utt.OnExit);
        Assert.Equal("", utt.OnUserDefined);
        Assert.Equal("tar02_02aca", utt.Tag);
        Assert.Equal("tar02_02aca", utt.ResRef);
        Assert.Equal(0, utt.TrapDetectDC);
        Assert.True(utt.TrapDetectable);
        Assert.True(utt.TrapDisarmable);
        Assert.False(utt.TrapFlag);
        Assert.True(utt.TrapOneShot);
        Assert.Equal(0, utt.TrapType);
        Assert.Equal(1, utt.Type);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var utt = UTT.FromFile(File1Filepath);
        utt.AutoRemoveKey = true;
        utt.Comment = "comment";
        utt.CursorID = 0;
        utt.DisarmDC = 1;
        utt.FactionID = 2;
        utt.HighlightHeight = 3;
        utt.KeyName = "keyname";
        utt.Name.StringRef = 4;
        utt.OnClick = "onclick";
        utt.OnDisarm = "ondisarm";
        utt.OnTrapTriggered = "ontraptriggered";
        utt.PaletteID = 5;
        utt.OnHeartbeat = "onheartbeat";
        utt.OnEnter = "onenter";
        utt.OnExit = "onexit";
        utt.OnUserDefined = "onuserdefined";
        utt.Tag = "tag";
        utt.ResRef = "resref";
        utt.TrapDetectDC = 6;
        utt.TrapDetectable = false;
        utt.TrapDisarmable = false;
        utt.TrapFlag = true;
        utt.TrapOneShot = false;
        utt.TrapType = 7;
        utt.Type = 8;

        // Assert
        Assert.True(utt.AutoRemoveKey);
        Assert.Equal("comment", utt.Comment);
        Assert.Equal(0, utt.CursorID);
        Assert.Equal(1, utt.DisarmDC);
        Assert.Equal(2U, utt.FactionID);
        Assert.Equal(3, utt.HighlightHeight);
        Assert.Equal("keyname", utt.KeyName);
        Assert.Equal(4, utt.Name.StringRef);
        Assert.Equal("onclick", utt.OnClick);
        Assert.Equal("ondisarm", utt.OnDisarm);
        Assert.Equal("ontraptriggered", utt.OnTrapTriggered);
        Assert.Equal(5, utt.PaletteID);
        Assert.Equal("onheartbeat", utt.OnHeartbeat);
        Assert.Equal("onenter", utt.OnEnter);
        Assert.Equal("onexit", utt.OnExit);
        Assert.Equal("onuserdefined", utt.OnUserDefined);
        Assert.Equal("tag", utt.Tag);
        Assert.Equal("resref", utt.ResRef);
        Assert.Equal(6, utt.TrapDetectDC);
        Assert.False(utt.TrapDetectable);
        Assert.False(utt.TrapDisarmable);
        Assert.True(utt.TrapFlag);
        Assert.False(utt.TrapOneShot);
        Assert.Equal(7, utt.TrapType);
        Assert.Equal(8, utt.Type);

    }
}
