using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Encapsulations;

namespace Kotor.NET.Patcher;

public interface IPatch
{
    void Apply(Installation installation, PatcherMemory memory);
}
