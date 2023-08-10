using KotorDotNET.Data.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Values
{
    /// <summary>
    /// Gets the row index as the value.
    /// </summary>
    public class RowIndexValue : IValue
    {
        public string GetValue(Memory memory, TwoDA twoda, TwoDARow row, string columnHeader)
        {
            return row.Index.ToString();
        }
    }
}
