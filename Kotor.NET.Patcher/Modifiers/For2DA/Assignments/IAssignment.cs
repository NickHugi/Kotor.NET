using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Assignments;

public interface IAssignment
{
    public void Assign(TwoDA twoda, TwoDARow row, Memory2DA memory2DA, MemoryTLK memoryTLK);
}
