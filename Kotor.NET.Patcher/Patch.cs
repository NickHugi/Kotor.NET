using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher;

public class Patch
{
    public ICollection<Patch> Patches { get; set; } = [];
}
