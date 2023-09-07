using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorSSF;

namespace KotorDotNET.Patching.Modifiers.ForSSF.Values
{
    public class TwoDAMemoryValue : IValue
    {
        public int TokenID { get; set; }

        public TwoDAMemoryValue(int tokenID)
        {
            TokenID = tokenID;
        }

        public void Apply(Memory memory, SSF ssf, CreatureSound sound)
        {
            var stringref = memory.From2DAToken(TokenID);
            ssf.Set(sound, Convert.ToInt32(stringref)); // TODO exception
        }
    }
}
