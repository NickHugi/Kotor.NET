using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

[Flags]
public enum MDLBinaryEmitterFlags
{
    P2P                 = 0b0000000000000001,
    P2P_SEL             = 0b0000000000000010,
    AffectedByWind      = 0b0000000000000100,
    Tinted              = 0b0000000000001000,
    Bounce              = 0b0000000000010000,
    Random              = 0b0000000000100000,
    Inherit             = 0b0000000001000000,
    InheritVelocity     = 0b0000000010000000,
    InheritLocal        = 0b0000000100000000,
    Splat               = 0b0000001000000000,
    InheritPart         = 0b0000010000000000,
    DepthTexture        = 0b0000100000000000,
    Flag13              = 0b0001000000000000,
}
