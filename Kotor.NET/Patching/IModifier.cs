using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patching
{
    public interface IModifier<T>
    {
        void Apply(T target, IMemory memory, ILogger logger);
    }
}
