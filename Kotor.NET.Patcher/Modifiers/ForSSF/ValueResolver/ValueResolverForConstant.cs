using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Patcher.Modifiers.ForSSF.ValueResolver;

public class ValueResolverForConstant : BaseValueResolver
{
    public required uint Value { get; init; }

    public override uint Resolve(SSF ssf, PatcherMemory memory)
    {
        return Value;
    }
}
