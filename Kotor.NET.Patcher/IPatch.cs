using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.For2DA;

namespace Kotor.NET.Patcher;

public interface IPatch
{
    void Apply(PatcherMemory memory);
}
