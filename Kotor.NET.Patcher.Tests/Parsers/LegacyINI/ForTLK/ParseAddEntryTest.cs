using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using IniParser.Parser;
using Kotor.NET.Patcher.Modifiers.ForTLK;
using Kotor.NET.Patcher.Parsers.LegacyINI.ForTLK;

namespace Kotor.NET.Patcher.Tests.Parsers.LegacyINI.ForTLK;

public class ParseAddEntryTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [TLKList]
                StrRef0=0
                StrRef123=1
            """);

        var modifiers = ini["TLKList"].Select(x => new ParseAddEntry().Parse(x)).ToList();

        using (new AssertionScope())
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.IncludingAllRuntimeProperties());

            modifiers.Should().ContainEquivalentOf(new AddEntryTLKModifier()
            {
                Key = "StrRef0",
                IndexIntoSourceTLK = 0
            });

            modifiers.Should().ContainEquivalentOf(new AddEntryTLKModifier()
            {
                Key = "StrRef123",
                IndexIntoSourceTLK = 1
            });
        }
    }
}
