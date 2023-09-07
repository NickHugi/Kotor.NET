using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common.Creature;
using KotorDotNET.FileFormats.KotorSSF;

namespace KotorDotNET.Patching.Modifiers.ForSSF
{
    public interface IValue
    {
        public void Apply(Memory memory, SSF ssf, CreatureSound sound);
    }
}
