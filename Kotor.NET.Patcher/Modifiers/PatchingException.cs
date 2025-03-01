using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Modifiers;

public class PatchingException : Exception
{
    public PatchingException(string message) : base(message)
    {
    }
}
