using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorGFF;

namespace KotorDotNET.Patching.Modifiers.ForGFF.Values
{
    public interface IValue
    {
        public string Resolve(IMemory memory, GFF gff);
    }
}
