using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Exceptions
{
    public class PatchingParserException : Exception
    {
        public PatchingParserException(string message)
            : base(message)
        {
        }
    }
}
