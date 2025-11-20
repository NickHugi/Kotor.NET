using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForTLK.Modifiers;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Tests.Modifiers.For2DA;

public class AddEntryTLKModifierTest
{
    [Fact]
    public void Apply()
    {
        var memory = new PatcherMemory();
        var log = new PatcherLogger();

        var source = new TLK();
        source.Add("consectetur", "Sound2");
        source.Add("dolor sit amet", "Sound3");

        var target = new TLK();
        target.Add("Lorem ipsum", "Sound1");

        var modifier0 = new AddEntryTLKModifier()
        {
            Key = "StrRef7",
            IndexIntoSourceTLK = 1
        };
        var modifier1 = new AddEntryTLKModifier()
        {
            Key = "StrRef5",
            IndexIntoSourceTLK = 0
        };


        modifier0.Apply(target, source, memory, log);
        modifier1.Apply(target, source, memory, log);


        using (var scope = new AssertionScope())
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.IncludingAllRuntimeProperties());

            target.Should().HaveCount(3);

            target.ElementAt(0).Should().BeEquivalentTo(new
            {
                Text = "Lorem ipsum",
                Sound = new ResRef("Sound1")
            });

            target.ElementAt(1).Should().BeEquivalentTo(new
            {
                Text = "dolor sit amet",
                Sound = new ResRef("Sound3")
            });

            target.ElementAt(2).Should().BeEquivalentTo(new
            {
                Text = "consectetur",
                Sound = new ResRef("Sound2")
            });
        }
    }
}
