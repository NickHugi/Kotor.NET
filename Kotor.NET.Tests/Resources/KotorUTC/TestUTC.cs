using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorUTC;

namespace Kotor.NET.Tests.Resources.KotorUTC;

public class TestUTC
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Test()
    {
        UTC utc = new();
        utc.Head.ResRef = "ABC";
        utc.Head.Droppable = true;
        utc.Head.Remove();
    }
}
