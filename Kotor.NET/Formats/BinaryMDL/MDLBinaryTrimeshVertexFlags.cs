using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public enum MDLBinaryTrimeshVertexFlags
{
    VertexFlag = 0x00000001,
    UV1Flag = 0x00000002,
    UV2Flag = 0x00000004,
    UV3Flag = 0x00000008,
    UV4Flag = 0x00000010,
    NormalFlag = 0x00000020,
    ColorsFlag = 0x00000040,
    Tangent1Flag = 0x00000080,
    Tangent2Flag = 0x00000100,
    Tangent3Flag = 0x00000200,
    Tangent4Flag = 0x00000400,
}
