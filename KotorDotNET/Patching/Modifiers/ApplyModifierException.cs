using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching.Modifiers
{
    public class ApplyModifierException : Exception
    {
        public ApplyModifierException(string message) : base(message)
        {

        }
    }
}
