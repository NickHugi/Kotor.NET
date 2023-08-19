using KotorDotNET.Patching;
using KotorDotNET.Patching.Modifiers;
using KotorDotNET.Patching.Modifiers.For2DA.Values;
using KotorDotNET.Patching.Modifiers.For2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Patching.Modifiers.For2DA.Targets;
using KotorDotNET.FileFormats.Kotor2DA;

namespace KotorDotNET.Tests.Tests.Patching.Modifiers.For2DA
{
    [TestClass]
    public class TestEditRowModifier
    {
        Memory memory;
        Logger logger;
        TwoDA twoda;

        [TestInitialize]
        public void Init()
        {
            memory = new();
            memory.Set2DAToken(0, "2");
            memory.SetTLKToken(0, 2);

            logger = new();

            twoda = new();
            twoda.AddColumn("C1");
            twoda.AddColumn("C2");
            twoda.AddColumn("C3");
            twoda.AddRow("l0");
            twoda.AddRow("l1");
        }

        [TestMethod]
        public void EditCells()
        {
            var data = new Dictionary<string, IValue>
            {
                ["C1"] = new ConstantValue("a"),
                ["C2"] = new ConstantValue("b"),
                ["C3"] = new ConstantValue("c"),
            };
            var toStoreInMemory = new Dictionary<int, IValue>();
            var target = new RowIndexTarget(1);
            var modifier = new EditRow2DAModifier(target, data, toStoreInMemory);

            modifier.Apply(twoda, memory, logger);

            // No rows were inserted.
            Assert.AreEqual(2, twoda.Rows().Count);
            // The cells have the correct values.
            Assert.AreEqual("a", twoda.Row(1).GetCell("C1"));
            Assert.AreEqual("b", twoda.Row(1).GetCell("C2"));
            Assert.AreEqual("c", twoda.Row(1).GetCell("C3"));
        }

        [TestMethod]
        public void StoreValue()
        {
            var data = new Dictionary<string, IValue>
            {
                ["C1"] = new ConstantValue("a"),
                ["C2"] = new ConstantValue("b"),
                ["C3"] = new ConstantValue("c"),
            };
            var toStoreInMemory = new Dictionary<int, IValue>
            {
                [30] = new RowIndexValue(),
            };
            var target = new RowIndexTarget(1);
            var modifier = new EditRow2DAModifier(target, data, toStoreInMemory);

            modifier.Apply(twoda, memory, logger);

            // The correct value was stored into the 2DA Memory.
            Assert.AreEqual("1", memory.From2DAToken(30));
        }
    }
}
