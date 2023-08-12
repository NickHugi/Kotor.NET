using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Targets
{
    /// <summary>
    /// Searches for a row in a TwoDA class that matches the stored cell value.
    /// </summary>
    public class ColumnTarget : ITarget
    {
        /// <summary>
        /// 
        /// </summary>
        public string ColumnHeader { get; set; }
        public string CellValue { get; set; }

        public ColumnTarget(string columnHeader, string cellValue)
        {
            ColumnHeader = columnHeader;
            CellValue = cellValue;
        }

        public TwoDARow Search(TwoDA twoda)
        {
            return twoda.Rows().Single(x => x.GetCell(ColumnHeader) == CellValue);
        }
    }
}
