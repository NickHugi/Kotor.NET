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
    public class TwoDAMemoryValue : IValue
    {
        public int TokenID { get; set; }

        public TwoDAMemoryValue(int tokenID)
        {
            TokenID = tokenID;
        }

        public string Resolve(IMemory memory, GFF gff)
        {
            var value = memory.From2DAToken(TokenID);

            if (value is not null)
            {
                int integer;
                var isNumber = int.TryParse(value, out integer);

                if (isNumber)
                {
                    return integer.ToString();
                }
                else
                {
                    throw new ApplyModifierException($"2DAMemory token {TokenID} does not contain a valid integer.");
                }
            }
            else
            {
                throw new ApplyModifierException($"2DAMemory token {TokenID} does not exist.");
            }
        }
    }
}
