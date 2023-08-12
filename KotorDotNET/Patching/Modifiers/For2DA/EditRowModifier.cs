using KotorDotNET.FileFormats.Kotor2DA;
using KotorDotNET.Patching.Modifiers.For2DA.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA
{
    /// <summary>
    /// Used to edit a row in a TwoDA instance.
    /// </summary>
    public class EditRowModifier
    {
        /// <summary>
        /// The target row to copy.
        /// </summary>
        public ITarget ITarget { get; set; }
        /// <summary>
        /// Maps column headers (Keys) to cell values.
        /// </summary>
        public Dictionary<string, IValue> Data { get; set; }
        /// <summary>
        /// Maps token IDs (Keys) to values.
        /// </summary>
        public Dictionary<int, IValue> ToStoreInMemory { get; set; }

        public EditRowModifier(ITarget target, Dictionary<string, IValue> data, Dictionary<int, IValue> toStoreInMemory)
        {
            ITarget = target;
            Data = data;
            ToStoreInMemory = toStoreInMemory;
        }

        public void Apply(TwoDA twoda, Memory memory, Logger logger)
        {
            var source = ITarget.Search(twoda);

            foreach (var pair in Data)
            {
                var columnHeader = pair.Key;
                var cellValue = pair.Value;

                if (twoda.ColumnHeaders().Contains(columnHeader))
                {
                    var value = cellValue.GetValue(memory, twoda, source, columnHeader);
                    source.SetCell(columnHeader, value);
                }
            }

            foreach (var toStore in ToStoreInMemory)
            {
                var value = toStore.Value.GetValue(memory, twoda, source, null);
                memory.Set2DAToken(toStore.Key, value);
            }
        }
    }
}
