using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using IniParser.Parser;
using Kotor.NET.Patcher.Modifiers.ForGFF;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;
using Kotor.NET.Patcher.Parsers.LegacyINI.For2DA;
using Kotor.NET.Patcher.Parsers.LegacyINI.ForGFF;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Tests.Parsers.LegacyINI.ForGFF;

public class ParseModifyExistingFieldTest
{
    public ParseModifyExistingFieldTest()
    {
        AssertionOptions.AssertEquivalencyUsing(options =>
         options.IncludingAllRuntimeProperties()
        );
    }

    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [edit_fields_gff]
                SomeList\0\Field=123
                SomeStruct\AnotherField=abc
                FinalField=2DAMEMORY1
            """);

        var parser = new ParseModifyExistingField();
        var modifiers = ini["edit_fields_gff"].Select(parser.Parse).ToList();

        using (new AssertionScope())
        {
            modifiers.Should().HaveCount(3);

            modifiers.Should().ContainEquivalentOf(new EditFieldUnknownGFFModifier
            {
                Label = "Field",
                Path = new BindingPath(["SomeList", "0"]),
                Value = new ValueStringConstant { Value = "123" }
            });

            modifiers.Should().ContainEquivalentOf(new EditFieldUnknownGFFModifier
            {
                Label = "AnotherField",
                Path = new BindingPath(["SomeStruct"]),
                Value = new ValueStringConstant { Value = "abc" }
            });

            modifiers.Should().ContainEquivalentOf(new EditFieldUnknownGFFModifier
            {
                Label = "FinalField",
                Path = new BindingPath([]),
                Value = new ValueMemory { Key = "2DAMEMORY1" }
            });
        }
    }
}
