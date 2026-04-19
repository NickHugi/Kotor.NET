using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher;

public class PatchBundle
{
    public ICollection<IPatch> Patches { get; set; } = [];
}
