using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorGFF;

namespace KotorDotNET.Patching.Modifiers.ForGFF.Values
{
    public class ConstantValue : IValue
    {
        public int StringRef { get; set; }

        public ConstantValue(int stringRef)
        {
            StringRef = stringRef;
        }

        public string Resolve(IMemory memory, GFF gff)
        {
            return StringRef.ToString();
        }
    }
}
