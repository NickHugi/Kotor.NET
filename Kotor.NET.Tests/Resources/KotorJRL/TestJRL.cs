using Kotor.NET.Resources.KotorJRL;

namespace Kotor.NET.Tests.Resources.KotorJRL;

public class TestJRL
{
    public static readonly string File1Filepath = "Resources/KotorJRL/file1.jrl";

    [Fact]
    public void Getters()
    {
        // Setup
        var jrl = JRL.FromFile(File1Filepath);

        // Assert
        Assert.Equal(2, jrl.Categories.Count);

        var category0 = jrl.Categories[0];
        Assert.Equal("", category0.Comment);
        Assert.Equal(132187, category0.Name.StringRef);
        Assert.Equal(-1, category0.PlanetID);
        Assert.Equal(3, category0.PlotIndex);
        Assert.Equal(4U, category0.Priority);
        Assert.Equal("tutorial_garage", category0.Tag);
        Assert.Equal(4, category0.Entries.Count);

        var category0_entry0 = category0.Entries[0];
        Assert.False(category0_entry0.End);
        Assert.Equal(10U, category0_entry0.ID);
        Assert.Equal(136016, category0_entry0.Text.StringRef);
        Assert.Equal(0, category0_entry0.XPPercentage);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var jrl = JRL.FromFile(File1Filepath);

        var category1 = jrl.Categories[1];
        category1.Comment = "comment";
        category1.Name.StringRef = 1;
        category1.PlanetID = 2;
        category1.PlotIndex = 3;
        category1.Priority = 4;
        category1.Tag = "tag";

        var category1_entry1 = category1.Entries[0];
        category1_entry1.End = true;
        category1_entry1.ID = 5;
        category1_entry1.Text.StringRef = 6;
        category1_entry1.XPPercentage = 7;

        // Assert
        Assert.Equal(2, jrl.Categories.Count);

        Assert.Equal("comment", category1.Comment);
        Assert.Equal(1, category1.Name.StringRef);
        Assert.Equal(2, category1.PlanetID);
        Assert.Equal(3, category1.PlotIndex);
        Assert.Equal(4U, category1.Priority);
        Assert.Equal("tag", category1.Tag);

        Assert.True(category1_entry1.End);
        Assert.Equal(5U, category1_entry1.ID);
        Assert.Equal(6, category1_entry1.Text.StringRef);
        Assert.Equal(7, category1_entry1.XPPercentage);
    }

    [Fact]
    public void AddCategories()
    {
        // Setup
        var jrl = new JRL();

        jrl.Categories.Add(new(), 0, "tag0", "", 0, 0);
        jrl.Categories.Add(new(), 1, "tag1", "", 0, 0);
        jrl.Categories.Add(new(2), 2, "tag2", "comment", 20, 200);

        // Assert
        Assert.Equal(3, jrl.Categories.Count);
        Assert.Equal(2U, jrl.Source.Root.GetList("Categories")?.ElementAt(2).ID);

        var category2 = jrl.Categories[2];
        Assert.True(category2.Exists);
        Assert.Equal(2, category2.Name.StringRef);
        Assert.Equal(2U, category2.Priority);
        Assert.Equal("tag2", category2.Tag);
        Assert.Equal("comment", category2.Comment);
        Assert.Equal(20, category2.PlotIndex);
        Assert.Equal(200, category2.PlanetID);
    }

    [Fact]
    public void RemoveCategory()
    {
        // Setup
        var jrl = JRL.FromFile(File1Filepath);

        jrl.Categories[0].Remove();

        //Assert
        Assert.Equal(1, jrl.Categories.Count);
        Assert.Equal(0U, jrl.Source.Root.GetList("Categories")?.ElementAt(0).ID);
    }

    [Fact]
    public void RemoveEntry()
    {
        // Setup
        var jrl = JRL.FromFile(File1Filepath);

        jrl.Categories[0].Entries[0].Remove();

        //Assert
        Assert.Equal(3, jrl.Categories[0].Entries.Count);
        Assert.Equal(0U, jrl.Source.Root.GetList("Categories")?.ElementAt(0)?.GetList("EntryList")?.ElementAt(0).ID);
        Assert.Equal(1U, jrl.Source.Root.GetList("Categories")?.ElementAt(0)?.GetList("EntryList")?.ElementAt(1).ID);
        Assert.Equal(2U, jrl.Source.Root.GetList("Categories")?.ElementAt(0)?.GetList("EntryList")?.ElementAt(2).ID);
    }
}
