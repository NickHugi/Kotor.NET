using KotorDotNET.FileFormats.Kotor2DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers.For2DA
{
    /// <summary>
    /// Gets a string value intended to be stored inside a TwoDA cell. How the
    /// value is determined is up to the implementation.
    /// </summary>
    public interface IValue
    {
        public string GetValue(Memory memory, TwoDA twoda, TwoDARow? row, string? columnHeader);
    }
}
