using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForTLK;
using Kotor.NET.Patcher.Modifiers.ForTLK.Modifiers;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Tests.Modifiers.ForTLK;

public class EditEntryTLKModifierTest
{
    [Fact]
    public void Apply()
    {
        var memory = new PatcherMemory();
        var log = new PatcherLogger();

        var source = new TLK();
        source.Add("consectetur", "Sound3");

        var target = new TLK();
        target.Add("Lorem ipsum", "Sound1");
        target.Add("dolor sit amet", "Sound2");

        var modifier0 = new EditEntryTLKModifier()
        {
            IndexIntoSourceTLK = 0,
            IndexIntoTargetTLK = 1
        };


        modifier0.Apply(target, source, memory, log);


        using (var scope = new AssertionScope())
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.IncludingAllRuntimeProperties());

            target.Should().HaveCount(2);

            target.ElementAt(0).Should().BeEquivalentTo(new
            {
                Text = "Lorem ipsum",
                Sound = new ResRef("Sound1")
            });

            target.ElementAt(1).Should().BeEquivalentTo(new
            {
                Text = "consectetur",
                Sound = new ResRef("Sound3")
            });
        }
    }
}
