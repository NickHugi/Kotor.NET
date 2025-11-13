using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;

public interface IGFFModifier
{
    void Apply(GFFStruct @struct, PatcherMemory memory);
}
