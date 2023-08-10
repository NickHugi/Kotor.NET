using KotorDotNET.Data.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Values
{
    /// <summary>
    /// Gets the highest integer value plus one under the given column.
    /// </summary>
    public class HighestValue : IValue
    {
        public string GetValue(Memory memory, TwoDA twoda, TwoDARow row, string columnHeader)
        {
            var cells = twoda.GetCellsUnderColumn(columnHeader);
            IEnumerable<int> integers = cells.Select(x =>
            {
                int integer = -1;
                int.TryParse(x, out integer);
                return integer;
            });
            var highest = integers.Max();
            var next = highest + 1;
            return next.ToString();
        }
    }
}
