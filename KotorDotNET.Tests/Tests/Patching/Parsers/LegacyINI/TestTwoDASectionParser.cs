using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Patching.Modifiers.For2DA;
using KotorDotNET.Patching.Parsers.LegacyINI;

namespace KotorDotNET.Tests.Tests.Patching.Parsers.LegacyINI
{
    [TestClass]
    public class TestTwoDASectionParser
    {
        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void EditRow_TargetRowLabel()
        {
            var ini =
                """
                [2DAList]
                Table0=test.2da

                [test.2da]
                AddRow0=addrow

                [addrow]
                RowLabel=1
                cola=1
                colb=2
                """;

            var data = new LegacyINIReader(ini).Parse();

            //
            Assert.AreEqual(1, data.TwoDAFiles.Count);
            var fileModifier = data.TwoDAFiles.Single(); // TODO rename
            //
            Assert.AreEqual(1, fileModifier.Modifiers.Count);
            var modifier = fileModifier.Modifiers.Single();
            //
            Assert.IsInstanceOfType(modifier, typeof(EditRow2DAModifier));
            //Assert.AreEqual(2, modifier.)
        }
    }
}
