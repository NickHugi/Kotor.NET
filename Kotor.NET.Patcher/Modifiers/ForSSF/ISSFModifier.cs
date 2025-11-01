using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Patcher.Modifiers.ForSSF;

public interface ISSFModifier
{
    void Apply(SSF ssf, PatcherMemory memory, PatcherLogger log);
}
