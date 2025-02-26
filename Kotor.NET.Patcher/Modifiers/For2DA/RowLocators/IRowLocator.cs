using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

public interface IRowLocator
{
    public TwoDARow Locate(TwoDA twoda);
    public TwoDARow? TryLocate(TwoDA twoda)
    {
        try
        {
            return Locate(twoda);
        }
        catch (PatchingException)
        {
            return null;
        }
    }
}
