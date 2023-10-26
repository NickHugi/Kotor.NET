﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorSSF;
using KotorDotNET.Patching.Modifiers.For2DA.Values;

namespace KotorDotNET.Patching.Modifiers.ForSSF.Values
{
    public class TLKMemoryValue : IValue
    {
        public int TokenID { get; set; }

        public TLKMemoryValue(int tokenID)
        {
            TokenID = tokenID;
        }

        public int GetValue(IMemory memory, SSF ssf, CreatureSound sound)
        {
            var value = memory.FromTLKToken(TokenID);

            if (value is not null)
            {
                return value.Value;
            }
            else
            {
                throw new ApplyModifierException($"TLKMemory token {TokenID} does not exist.");
            }
        }
    }
}
