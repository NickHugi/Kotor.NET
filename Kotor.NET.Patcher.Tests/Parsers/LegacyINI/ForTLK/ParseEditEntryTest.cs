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

public class ParseEditEntryTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [TLKList]
                Replace0=append.tlk

                [append.tlk]
                20001=0
                20002=5
            """);

        var modifiers = ini["append.tlk"].Select(x => new ParseEditEntry().Parse(x)).ToList();

        using (new AssertionScope())
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.IncludingAllRuntimeProperties());

            modifiers.Should().ContainEquivalentOf(new EditEntryTLKModifier()
            {
                IndexIntoTargetTLK = 20001,
                IndexIntoSourceTLK = 0
            });

            modifiers.Should().ContainEquivalentOf(new EditEntryTLKModifier()
            {
                IndexIntoTargetTLK = 20002,
                IndexIntoSourceTLK = 5
            });
        }
    }
}
