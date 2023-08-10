using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Patching
{
    public interface IModifier<T>
    {
        void Apply(T target, Memory memory, ILogger logger);
    }
}
