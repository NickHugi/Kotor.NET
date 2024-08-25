using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorUTI;

namespace Kotor.NET.Tests.Resources.KotorUTI;


public class TestUTI
{
    public static readonly string File1Filepath = "Resources/KotorUTI/file1.uti";

    [Fact]
    public void Getters()
    {
        // Setup
        var uti = UTI.FromFile(File1Filepath);

        // Assert
        Assert.Equal(1200U, uti.AddCost);
        Assert.Equal(9, uti.BaseItem);
        Assert.Equal(0, uti.BodyVariation);
        Assert.Equal(0, uti.Charges);
        Assert.Equal("double-bladed lightsaber - heart of the guardian", uti.Comment);
        Assert.Equal(1200U, uti.Cost);
        Assert.Equal(6059, uti.DescriptionIdentified.StringRef);
        Assert.Equal(-1, uti.DescriptionUnidentified.StringRef);
        Assert.Equal(38950, uti.LocalizedName.StringRef);
        Assert.Equal(6, uti.ModelVariation);
        Assert.Equal(7, uti.PaletteID);
        Assert.False(uti.Plot);
        Assert.Equal(1, uti.StackSize);
        Assert.Equal("g1_w_dblsbr001", uti.Tag);
        Assert.Equal("g1_w_dblsbr001", uti.TemplateResRef);
        Assert.Equal(0, uti.TextureVar);
        Assert.Equal(0, uti.UpgradeLevel);
        Assert.True(uti.Identified);

        Assert.Equal(5, uti.Properties.Count);

        var property2 = uti.Properties[2];
        Assert.Equal(100, property2.ChanceAppear);
        Assert.Equal(2, property2.CostTable);
        Assert.Equal(3, property2.CostValue);
        Assert.Equal(255, property2.Param1);
        Assert.Equal(0, property2.Param1Value);
        Assert.Equal(38, property2.PropertyName);
        Assert.Equal(0, property2.Subtype);
        Assert.Equal(9, property2.UpgradeType);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var uti = UTI.FromFile(File1Filepath);
        uti.AddCost = 1;
        uti.BaseItem = 2;
        uti.BodyVariation = 3;
        uti.Charges = 4;
        uti.Comment = "comment";
        uti.Cost = 5;
        uti.DescriptionIdentified.StringRef = 6;
        uti.DescriptionUnidentified.StringRef = 7;
        uti.LocalizedName.StringRef = 8;
        uti.ModelVariation = 9;
        uti.PaletteID = 10;
        uti.Plot = true;
        uti.StackSize = 11;
        uti.Tag = "tag";
        uti.TemplateResRef = "resref";
        uti.TextureVar = 12;
        uti.UpgradeLevel = 13;
        uti.Identified = false;

        var property2 = uti.Properties[2];
        property2.ChanceAppear = 1;
        property2.CostTable = 2;
        property2.CostValue = 3;
        property2.Param1 = 4;
        property2.Param1Value = 5;
        property2.PropertyName = 6;
        property2.Subtype = 7;
        property2.UpgradeType = 8;

        // Assert
        Assert.Equal(1U, uti.AddCost);
        Assert.Equal(2, uti.BaseItem);
        Assert.Equal(3, uti.BodyVariation);
        Assert.Equal(4, uti.Charges);
        Assert.Equal("comment", uti.Comment);
        Assert.Equal(5U, uti.Cost);
        Assert.Equal(6, uti.DescriptionIdentified.StringRef);
        Assert.Equal(7, uti.DescriptionUnidentified.StringRef);
        Assert.Equal(8, uti.LocalizedName.StringRef);
        Assert.Equal(9, uti.ModelVariation);
        Assert.Equal(10, uti.PaletteID);
        Assert.True(uti.Plot);
        Assert.Equal(11, uti.StackSize);
        Assert.Equal("tag", uti.Tag);
        Assert.Equal("resref", uti.TemplateResRef);
        Assert.Equal(12, uti.TextureVar);
        Assert.Equal(13, uti.UpgradeLevel);
        Assert.False(uti.Identified);

        property2 = uti.Properties[2];
        Assert.Equal(1, property2.ChanceAppear);
        Assert.Equal(2, property2.CostTable);
        Assert.Equal(3, property2.CostValue);
        Assert.Equal(4, property2.Param1);
        Assert.Equal(5, property2.Param1Value);
        Assert.Equal(6, property2.PropertyName);
        Assert.Equal(7, property2.Subtype);
        Assert.Equal(8, property2.UpgradeType);

    }

    [Fact]
    public void RemoveProperty()
    {
        // Setup
        var uti = UTI.FromFile(File1Filepath);

        // Act
        uti.Properties[0].Remove();
        //uti.Properties.Add(1, 2, 3, 4, 5, 6, 7);
        //uti.Properties.Add(10, 11, 12, 13, 14, 15, 16);

        // Assert
        Assert.Equal(4, uti.Properties.Count);

        var property0 = uti.Properties[0];
        Assert.Equal(100, property0.ChanceAppear);
        Assert.Equal(2, property0.CostTable);
        Assert.Equal(3, property0.CostValue);
        Assert.Equal(255, property0.Param1);
        Assert.Equal(0, property0.Param1Value);
        Assert.Equal(38, property0.PropertyName);
        Assert.Equal(0, property0.Subtype);
        Assert.Equal(2, property0.UpgradeType);
    }

    [Fact]
    public void AddProperty()
    {
        // Setup
        var uti = new UTI();

        // Act
        uti.Properties.Add(1, 2, 3, 4, 5, 6, 7);
        uti.Properties.Add(10, 11, 12, 13, 14, 15, 16);

        // Assert
        Assert.Equal(2, uti.Properties.Count);

        var property1 = uti.Properties[1];
        Assert.Equal(100, property1.ChanceAppear);
        Assert.Equal(10, property1.CostTable);
        Assert.Equal(11, property1.CostValue);
        Assert.Equal(12, property1.Param1);
        Assert.Equal(13, property1.Param1Value);
        Assert.Equal(14, property1.PropertyName);
        Assert.Equal(15, property1.Subtype);
        Assert.Equal(16, property1.UpgradeType);
    }
}
