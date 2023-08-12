using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA
{
    /// <summary>
    /// Used to insert a new column into a TwoDA instance.
    /// </summary>
    public class AddColumnModifier : IModifier<TwoDA>
    {
        /// <summary>
        /// The header for the column that is to be inserted.
        /// </summary>
        public string ColumnHeader { get; }
        /// <summary>
        /// The default value for all the cells under the new column.
        /// </summary>
        public string DefaultValue { get; }
        
        // TODO
        //public Dictionary<RowSearch, IValue> Values { get; set; }


        public AddColumnModifier(string header, string defaultValue)
        {
            ColumnHeader = header;
            DefaultValue = defaultValue;
        }

        public void Apply(TwoDA twoda, Memory memory, ILogger logger)
        {
            twoda.AddColumn(ColumnHeader);

            foreach (var row in twoda.Rows())
            {
                row.SetCell(ColumnHeader, DefaultValue);
            }
        }
    }
}
