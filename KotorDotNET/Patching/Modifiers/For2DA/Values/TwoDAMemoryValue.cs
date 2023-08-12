using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA.Values
{
    /// <summary>
    /// Gets the value from the 2DA memory using the stored token ID.
    /// </summary>
    public class TwoDAMemoryValue : IValue
    {
        /// <summary>
        /// The token ID to search for in the 2DA memory.
        /// </summary>
        public int TokenID { get; set; }

        public TwoDAMemoryValue(int tokenID)
        {
            TokenID = tokenID;
        }

        public string GetValue(Memory memory, TwoDA twoda, TwoDARow? row, string? columnHeader)
        {
            return memory.From2DAToken(TokenID).ToString();
        }
    }
}
