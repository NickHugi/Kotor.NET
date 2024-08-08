using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Resources.KotorIFO;

namespace Kotor.NET.Tests.Resources.KotorIFO;

public class TestIFO
{
    public static readonly string File1Filepath = "Resources/KotorIFO/file1.ifo";

    [Fact]
    public void Getters()
    {
        // Setup
        var ifo = IFO.FromFile(File1Filepath);

        // Assert
        Assert.Equal("m02aa", ifo.VoiceOverID);
        Assert.Equal("tar_m02aa", ifo.Name.GetSubstring(Language.English, Gender.MaleOrNeutral));
        Assert.Equal("", ifo.Tag);
        Assert.Equal("m02aa", ifo.EntryArea);
        Assert.Equal(3, ifo.ModEntryX);
        Assert.Equal(4, ifo.ModEntryY);
        Assert.Equal(5, ifo.ModEntryZ);
        Assert.Equal(1, ifo.ModEntryDirectionX);
        Assert.Equal(2, ifo.ModEntryDirectionY);
        Assert.Equal(10, ifo.XPScale);
        Assert.Equal("", ifo.OnHeartbeat);
        Assert.Equal("", ifo.OnModLoad);
        Assert.Equal("", ifo.OnModStart);
        Assert.Equal("", ifo.OnClientEnter);
        Assert.Equal("", ifo.OnClientLeave);
        Assert.Equal("", ifo.OnActivateItem);
        Assert.Equal("", ifo.OnAcquireItem);
        Assert.Equal("", ifo.OnUserDefined);
        Assert.Equal("", ifo.OnUnAcquireItem);
        Assert.Equal("nw_o0_death", ifo.OnPlayerDeath);
        Assert.Equal("nw_o0_dying", ifo.OnPlayerDying);
        Assert.Equal("", ifo.OnPlayerLevelUp);
        Assert.Equal("nw_o0_respawn", ifo.OnSpawnButtonDown);

        Assert.Single(ifo.ModAreaList);
        var area0 = ifo.ModAreaList[0];
        Assert.Equal("m02aa", area0.ResRef);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var ifo = IFO.FromFile(File1Filepath);
        ifo.VoiceOverID = "voice_over_id";
        ifo.Name.StringRef = 12345;
        ifo.Tag = "module_tag";
        ifo.EntryArea = "entry_area";
        ifo.ModEntryX = 400.0f;
        ifo.ModEntryY = 500.0f;
        ifo.ModEntryZ = 600.0f;
        ifo.ModEntryDirectionX = 2.0f;
        ifo.ModEntryDirectionY = 3.0f;
        ifo.XPScale = 10;
        ifo.OnHeartbeat = "heartbeat";
        ifo.OnModLoad = "modload";
        ifo.OnModStart = "modstart";
        ifo.OnClientEnter = "cliententer";
        ifo.OnClientLeave = "clientleave";
        ifo.OnActivateItem = "activateitem";
        ifo.OnAcquireItem = "acquireitem";
        ifo.OnUserDefined = "userdefined";
        ifo.OnUnAcquireItem = "unacquireitem";
        ifo.OnPlayerDeath = "playerdeath";
        ifo.OnPlayerDying = "playerdying";
        ifo.OnPlayerLevelUp = "playerlevelup";
        ifo.OnSpawnButtonDown = "spawnbutton";

        ifo.ModAreaList.Clear();
        ifo.ModAreaList.Add("area1");
        ifo.ModAreaList.Add("area2");

        // Assert
        Assert.Equal("voice_over_id", ifo.VoiceOverID);
        Assert.Equal(12345, ifo.Name.StringRef);
        Assert.Equal("module_tag", ifo.Tag);
        Assert.Equal("entry_area", ifo.EntryArea);
        Assert.Equal(400.0f, ifo.ModEntryX);
        Assert.Equal(500.0f, ifo.ModEntryY);
        Assert.Equal(600.0f, ifo.ModEntryZ);
        Assert.Equal(2.0f, ifo.ModEntryDirectionX);
        Assert.Equal(3.0f, ifo.ModEntryDirectionY);
        Assert.Equal(10, ifo.XPScale);
        Assert.Equal("heartbeat", ifo.OnHeartbeat);
        Assert.Equal("modload", ifo.OnModLoad);
        Assert.Equal("modstart", ifo.OnModStart);
        Assert.Equal("cliententer", ifo.OnClientEnter);
        Assert.Equal("clientleave", ifo.OnClientLeave);
        Assert.Equal("activateitem", ifo.OnActivateItem);
        Assert.Equal("acquireitem", ifo.OnAcquireItem);
        Assert.Equal("userdefined", ifo.OnUserDefined);
        Assert.Equal("unacquireitem", ifo.OnUnAcquireItem);
        Assert.Equal("playerdeath", ifo.OnPlayerDeath);
        Assert.Equal("playerdying", ifo.OnPlayerDying);
        Assert.Equal("playerlevelup", ifo.OnPlayerLevelUp);
        Assert.Equal("spawnbutton", ifo.OnSpawnButtonDown);

        var area0 = ifo.ModAreaList.ElementAtOrDefault(0);
        Assert.NotNull(area0);
        Assert.Equal("area1", area0.ResRef);

        var area1 = ifo.ModAreaList.ElementAtOrDefault(1);
        Assert.NotNull(area1);
        Assert.Equal("area2", area1.ResRef);
    }
}
