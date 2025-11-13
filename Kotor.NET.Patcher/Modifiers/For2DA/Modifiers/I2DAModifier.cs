using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Modifiers;

public interface I2DAModifier
{
    public void Apply(TwoDA twoda, PatcherMemory memory, PatcherLogger log);
}
