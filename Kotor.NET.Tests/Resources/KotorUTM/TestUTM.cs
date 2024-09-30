using Kotor.NET.Resources.KotorUTM;

namespace Kotor.NET.Tests.Resources.KotorUTM;

public class TestUTM
{
    public static readonly string File1Filepath = "Resources/KotorUTM/test1.utm";

    [Fact]
    public void Getters()
    {
        // Setup
        var utm = UTM.FromFile(File1Filepath);

        // Assert
        Assert.Equal("larrim", utm.ResRef);
        Assert.Equal(40478, utm.Name.StringRef);
        Assert.Equal("Larrim_Store", utm.Tag);
        Assert.Equal(110, utm.MarkUp);
        Assert.Equal(40, utm.MarkDown);
        Assert.Equal("", utm.OnOpenStore);
        Assert.Equal(5, utm.ID);
        Assert.Equal("", utm.Comment);
        Assert.True(utm.CanBuy);
        Assert.True(utm.CanSell);

        var item0 = utm.Items.ElementAtOrDefault(0);
        Assert.NotNull(item0);
        Assert.Equal("g_a_class4001", item0.ResRef);
        Assert.True(item0.Infinite);

        var item1 = utm.Items.ElementAtOrDefault(1);
        Assert.NotNull(item1);
        Assert.Equal("g_a_class5001", item1.ResRef);
        Assert.False(item1.Infinite);

        var item2 = utm.Items.ElementAtOrDefault(2);
        Assert.NotNull(item2);
        Assert.Equal("g_a_class6001", item2.ResRef);
        Assert.False(item2.Infinite);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var utm = UTM.FromFile(File1Filepath);
        utm.ResRef = "resref";
        utm.Name.StringRef = 1;
        utm.Tag = "tag";
        utm.MarkUp = 2;
        utm.MarkDown = 3;
        utm.OnOpenStore = "onopenstore";
        utm.ID = 4;
        utm.Comment = "comment";
        utm.CanBuy = false;
        utm.CanSell = false;

        utm.Items.Clear();
        utm.Items.Add("item1", false);
        utm.Items.Add("item2", true);

        // Assert
        Assert.Equal("resref", utm.ResRef);
        Assert.Equal(1, utm.Name.StringRef);
        Assert.Equal("tag", utm.Tag);
        Assert.Equal(2, utm.MarkUp);
        Assert.Equal(3, utm.MarkDown);
        Assert.Equal("onopenstore", utm.OnOpenStore);
        Assert.Equal(4, utm.ID);
        Assert.Equal("comment", utm.Comment);
        Assert.False(utm.CanBuy);
        Assert.False(utm.CanSell);

        var item0 = utm.Items.ElementAtOrDefault(0);
        Assert.NotNull(item0);
        Assert.Equal("item1", item0.ResRef);
        Assert.False(item0.Infinite);

        var item1 = utm.Items.ElementAtOrDefault(1);
        Assert.NotNull(item1);
        Assert.Equal("item2", item1.ResRef);
        Assert.True(item1.Infinite);
    }
}
