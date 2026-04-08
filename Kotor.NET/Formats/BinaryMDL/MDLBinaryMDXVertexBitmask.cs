using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

[Flags]
public enum MDLBinaryMDXVertexBitmask : uint
{
    Position    = 0b00000000001,
    UV1         = 0b00000000010,
    UV2         = 0b00000000100,
    UV3         = 0b00000001000,
    UV4         = 0b00000010000,
    Normals     = 0b00000100000,
    Colours     = 0b00001000000,
    Tangent1    = 0b00010000000,
    Tangent2    = 0b00100000000,
    Tangent3    = 0b01000000000,
    Tangent4    = 0b10000000000,
}
