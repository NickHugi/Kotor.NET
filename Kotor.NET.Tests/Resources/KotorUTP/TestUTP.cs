using System.Linq;
using Kotor.NET.Resources.KotorJRL;
using Kotor.NET.Resources.KotorUTP;
using Xunit;

namespace Kotor.NET.Tests.Resources.KotorUTP;

public class TestUTP
{
    public static readonly string File1Filepath = "Resources/KotorUTP/file1.utp";

    [Fact]
    public void Getters()
    {
        // Setup
        var utp = UTP.FromFile(File1Filepath);

        // Assert
        Assert.Equal(2, utp.AnimationState);
        Assert.Equal(67U, utp.AppearanceID);
        Assert.True(utp.AutoRemoveKey);
        Assert.Equal("Military High Plastic cylinder crate", utp.Comment);
        Assert.Equal("minecntr", utp.Conversation);
        Assert.Equal(15, utp.CurrentHP);
        Assert.Equal(1U, utp.FactionID);
        Assert.Equal(16, utp.Fortitude);
        Assert.Equal(5, utp.Hardness);
        Assert.True(utp.HasInventory);
        Assert.Equal(15, utp.HP);
        Assert.False(utp.IsComputer);
        Assert.Equal("key_locker", utp.KeyName);
        Assert.True(utp.KeyRequired);
        Assert.False(utp.Lockable);
        Assert.True(utp.Locked);
        Assert.Equal(124188, utp.Name.StringRef);
        Assert.False(utp.Min1HP);
        Assert.True(utp.NotBlastable);
        Assert.Equal("a_compdlg", utp.OnClosed);
        Assert.Equal("", utp.OnDamaged);
        Assert.Equal("", utp.OnDeath);
        Assert.Equal("", utp.OnEndDialogue);
        Assert.Equal("a_compdlg", utp.OnFailToOpen);
        Assert.Equal("", utp.OnHeartbeat);
        Assert.Equal("", utp.OnInvDisturbed);
        Assert.Equal("", utp.OnMeleeAttacked);
        Assert.Equal("a_set_unlocked", utp.OnOpen);
        Assert.Equal("", utp.OnSpellCastAt);
        Assert.Equal("", utp.OnUnlock);
        Assert.Equal("", utp.OnUsed);
        Assert.Equal("", utp.OnUserDefined);
        Assert.Equal(27, utp.OpenLockDC);
        Assert.Equal(1, utp.OpenLockDiff);
        Assert.Equal(0, utp.OpenLockDiffMod);
        Assert.Equal(6, utp.PaletteID);
        Assert.True(utp.PartyInteract);
        Assert.True(utp.Plot);
        Assert.Equal(0, utp.Reflex);
        Assert.False(utp.Static);
        Assert.Equal("locker_locked", utp.Tag);
        Assert.Equal("g_tresmilhig007", utp.ResRef);
        Assert.Equal(0, utp.Type);
        Assert.True(utp.Useable);
        Assert.Equal(0, utp.Will);

        Assert.Equal(3, utp.Inventory.Count);

        var item2 = utp.Inventory[2];
        Assert.Equal(item2.ResRef, "g_i_progspike01");
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var utp = UTP.FromFile(File1Filepath);
        utp.AnimationState = 1;
        utp.AppearanceID = 2;
        utp.AutoRemoveKey = false;
        utp.Comment = "comment";
        utp.Conversation = "conversation";
        utp.CurrentHP = 3;
        utp.FactionID = 4;
        utp.Fortitude = 5;
        utp.Hardness = 6;
        utp.HasInventory = false;
        utp.HP = 7;
        utp.IsComputer = true;
        utp.KeyName = "key";
        utp.KeyRequired = false;
        utp.Lockable = true;
        utp.Locked = false;
        utp.Name.StringRef = 8;
        utp.Min1HP = true;
        utp.NotBlastable = false;
        utp.OnClosed = "closed";
        utp.OnDamaged = "damaged";
        utp.OnDeath = "death";
        utp.OnEndDialogue = "enddialogue";
        utp.OnFailToOpen = "failtoopen";
        utp.OnHeartbeat = "heartbeat";
        utp.OnInvDisturbed = "invdisturbed";
        utp.OnMeleeAttacked = "meleeattacked";
        utp.OnOpen = "open";
        utp.OnSpellCastAt = "spellcastat";
        utp.OnUnlock = "unlock";
        utp.OnUsed = "used";
        utp.OnUserDefined = "userdefined";
        utp.PaletteID = 9;
        utp.OpenLockDC = 10;
        utp.OpenLockDiff = 11;
        utp.OpenLockDiffMod = 12;
        utp.PartyInteract = false;
        utp.Plot = false;
        utp.Reflex = 13;
        utp.Static = true;
        utp.Tag = "tag";
        utp.ResRef = "resref";
        utp.Type = 14;
        utp.Useable = false;
        utp.Will = 15;

        // Assert
        Assert.Equal(1, utp.AnimationState);
        Assert.Equal(2U, utp.AppearanceID);
        Assert.False(utp.AutoRemoveKey);
        Assert.Equal("comment", utp.Comment);
        Assert.Equal("conversation", utp.Conversation);
        Assert.Equal(3, utp.CurrentHP);
        Assert.Equal(4U, utp.FactionID);
        Assert.Equal(5, utp.Fortitude);
        Assert.Equal(6, utp.Hardness);
        Assert.False(utp.HasInventory);
        Assert.Equal(7, utp.HP);
        Assert.True(utp.IsComputer);
        Assert.Equal("key", utp.KeyName);
        Assert.False(utp.KeyRequired);
        Assert.True(utp.Lockable);
        Assert.False(utp.Locked);
        Assert.Equal(8, utp.Name.StringRef);
        Assert.True(utp.Min1HP);
        Assert.False(utp.NotBlastable);
        Assert.Equal("closed", utp.OnClosed);
        Assert.Equal("damaged", utp.OnDamaged);
        Assert.Equal("death", utp.OnDeath);
        Assert.Equal("enddialogue", utp.OnEndDialogue);
        Assert.Equal("failtoopen", utp.OnFailToOpen);
        Assert.Equal("heartbeat", utp.OnHeartbeat);
        Assert.Equal("invdisturbed", utp.OnInvDisturbed);
        Assert.Equal("meleeattacked", utp.OnMeleeAttacked);
        Assert.Equal("open", utp.OnOpen);
        Assert.Equal("spellcastat", utp.OnSpellCastAt);
        Assert.Equal("unlock", utp.OnUnlock);
        Assert.Equal("used", utp.OnUsed);
        Assert.Equal("userdefined", utp.OnUserDefined);
        Assert.Equal(9, utp.PaletteID);
        Assert.Equal(10, utp.OpenLockDC);
        Assert.Equal(11, utp.OpenLockDiff);
        Assert.Equal(12, utp.OpenLockDiffMod);
        Assert.False(utp.PartyInteract);
        Assert.False(utp.Plot);
        Assert.Equal(13, utp.Reflex);
        Assert.True(utp.Static);
        Assert.Equal("tag", utp.Tag);
        Assert.Equal("resref", utp.ResRef);
        Assert.Equal(14, utp.Type);
        Assert.False(utp.Useable);
        Assert.Equal(15, utp.Will);
    }

    [Fact]
    public void AddItem()
    {
        // Setup
        var utp = new UTP();

        utp.Inventory.Add("item0");
        utp.Inventory.Add("item1");
        utp.Inventory.Add("item2");

        // Assert
        Assert.Equal(3, utp.Inventory.Count);
        Assert.Equal(2U, utp.Source.Root.GetList("ItemList")?.ElementAt(2).ID);

        var item = utp.Inventory[2];
        Assert.True(item.Exists);
        Assert.Equal(2, item.Index);
        Assert.Equal("item2", item.ResRef);
    }

    [Fact]
    public void RemoveItem()
    {
        // Setup
        var utp = UTP.FromFile(File1Filepath);

        //Act
        utp.Inventory[0].Remove();

        // Assert
        Assert.Equal(2, utp.Inventory.Count);
        Assert.Equal(0U, utp.Source.Root.GetList("ItemList")?.ElementAt(0).ID);
        Assert.Equal(1U, utp.Source.Root.GetList("ItemList")?.ElementAt(1).ID);
    }

    [Fact]
    public void ClearInventory()
    {
        // Setup
        var utp = UTP.FromFile(File1Filepath);

        // Act
        utp.Inventory.Clear();

        // Assert
        Assert.Equal(0, utp.Inventory.Count);
    }

}
