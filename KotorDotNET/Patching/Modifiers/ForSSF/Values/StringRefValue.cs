using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorSSF;

namespace KotorDotNET.Patching.Modifiers.ForSSF.Values
{
    public class StringRefValue : IValue
    {
        public int StringRef { get; set; }

        public StringRefValue(int stringRef)
        {
            StringRef = stringRef;
        }

        public int GetValue(IMemory memory, SSF ssf, CreatureSound sound)
        {
            return StringRef;
        }
    }
}
