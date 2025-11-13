using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Modifiers.ForTLK.Modifiers;

public interface ITLKModifier
{
    void Apply(TLK target, TLK source, PatcherMemory memory, PatcherLogger log);
}
