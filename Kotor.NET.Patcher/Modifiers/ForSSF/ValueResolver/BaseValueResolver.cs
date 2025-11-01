using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Patcher.Modifiers.ForSSF.ValueResolver;

public abstract class BaseValueResolver
{
    public abstract uint Resolve(SSF ssf, PatcherMemory memory);

    public bool TryResolve(SSF ssf, PatcherMemory memory, out uint value)
    {
        try
        {
            value = Resolve(ssf, memory);
            return true;
        }
        catch (PatchingException)
        {
            value = 0;
            return false;
        }
    }
}
