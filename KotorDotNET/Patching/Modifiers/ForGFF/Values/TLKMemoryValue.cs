using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorGFF;
using KotorDotNET.FileFormats.KotorSSF;

namespace KotorDotNET.Patching.Modifiers.ForGFF.Values
{
    public class TLKMemoryValue : IValue
    {
        public int TokenID { get; set; }

        public TLKMemoryValue(int tokenID)
        {
            TokenID = tokenID;
        }

        public string Resolve(IMemory memory, GFF gff)
        {
            var value = memory.FromTLKToken(TokenID);

            if (value is not null)
            {
                return value.Value.ToString();
            }
            else
            {
                throw new ApplyModifierException($"TLKMemory token {TokenID} does not exist.");
            }
        }
    }
}
