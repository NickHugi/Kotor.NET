using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForGFF.Directive;
using Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorGFF.Builder;

namespace Kotor.NET.Patcher.Tests.Modifiers.ForGFF;

public class EditFieldGFFModifierTest
{
    [Fact]
    public void Apply_Value_Int8()
    {
        var gff = new GFF();
        gff.Root.SetInt8("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetInt8("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_Int16()
    {
        var gff = new GFF();
        gff.Root.SetInt16("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetInt16("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_Int32()
    {
        var gff = new GFF();
        gff.Root.SetInt32("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetInt32("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_Int64()
    {
        var gff = new GFF();
        gff.Root.SetInt64("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetInt64("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_UInt8()
    {
        var gff = new GFF();
        gff.Root.SetUInt8("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetUInt8("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_UInt16()
    {
        var gff = new GFF();
        gff.Root.SetUInt16("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetUInt16("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_UInt32()
    {
        var gff = new GFF();
        gff.Root.SetUInt32("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetUInt32("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_UInt64()
    {
        var gff = new GFF();
        gff.Root.SetUInt64("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetUInt64("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_Single()
    {
        var gff = new GFF();
        gff.Root.SetSingle("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetSingle("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_Double()
    {
        var gff = new GFF();
        gff.Root.SetDouble("Field", 0);

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetDouble("Field").Should().Be(123);
    }

    [Fact]
    public void Apply_Value_String()
    {
        var gff = new GFF();
        gff.Root.SetString("Field", "");

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetString("Field").Should().Be("123");
    }

    [Fact]
    public void Apply_Value_ResRef()
    {
        var gff = new GFF();
        gff.Root.SetResRef("Field", "");

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetResRef("Field")!.Get().Should().BeEquivalentTo("123");
    }

    [Fact]
    public void Apply_Value_LocalizedString_StringRef()
    {
        var gff = new GFF();
        gff.Root.SetLocalisedString("Field", new());

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" },
            LocalizedStringDirective = new StringRefLocalizedStringDirective()
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetLocalisedString("Field")!.StringRef.Should().Be(123);
    }

    [Fact]
    public void Apply_Value_LocalizedString_Substring()
    {
        var gff = new GFF();
        gff.Root.SetLocalisedString("Field", new());

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "123" },
            LocalizedStringDirective = new SubstringLocalizedStringDirective() { LanguageID = 3 }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetLocalisedString("Field")!.GetSubstring(Language.French, Gender.Female).Should().Be("123");
    }

    [Fact]
    public void Apply_Value_Vector3()
    {
        var gff = new GFF();
        gff.Root.SetVector3("Field", new());

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "1|2|3" },
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetVector3("Field").Should().BeEquivalentTo(new Vector3()
        {
            X = 1,
            Y = 2,
            Z = 3
        });
    }

    [Fact]
    public void Apply_Value_Vector4()
    {
        var gff = new GFF();
        gff.Root.SetVector4("Field", new());

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Value = new ValueStringConstant() { Value = "1|2|3|4" },
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetVector4("Field").Should().BeEquivalentTo(new Vector4()
        {
            X = 1,
            Y = 2,
            Z = 3,
            W = 4
        });
    }

    [Fact]
    public void Apply_Path()
    {
        var gff = new GFFBuilder().New(root =>
        {
            root.AddList("List0");
            root.AddList("List1", 0, list1 =>
            {
                list1.AddStruct();
                list1.AddStruct(0, struct1 =>
                {
                    struct1.AddInt32("Field", 0);
                });
                list1.AddStruct();
            });
            root.AddList("List2");
        });

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        var modifier = new EditFieldGFFModifier()
        {
            Label = "Field",
            Path = new(["List1", "1"]),
            Value = new ValueStringConstant() { Value = "123" }
        };

        modifier.Apply(gff.Root, memory);

        gff.Root.GetList("List1")!.ElementAt(1).GetInt32("Field").Should().Be(123);
    }
}
