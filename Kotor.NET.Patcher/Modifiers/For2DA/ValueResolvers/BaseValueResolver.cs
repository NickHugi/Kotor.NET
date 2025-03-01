using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public abstract class BaseValueResolver
{
    public abstract string Resolve(TwoDA twoda, TwoDARow? row, PatcherMemory memory);

    public bool TryResolve(TwoDA twoda, TwoDARow? row, PatcherMemory memory, out string value)
    {
        try
        {
            value = Resolve(twoda, row, memory);
            return true;
        }
        catch (PatchingException)
        {
            value = "";
            return false;
        }
    }
}
