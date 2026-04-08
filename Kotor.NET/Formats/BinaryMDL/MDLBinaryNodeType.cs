using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

[Flags]
public enum MDLBinaryNodeType : ushort
{
    NodeFlag = 0x00000001,
    LightFlag = 0x00000002,
    EmitterFlag = 0x00000004,
    ReferenceFlag = 0x00000010,
    TrimeshFlag = 0x00000020,
    SkinFlag = 0x00000040,
    AnimationFlag = 0x00000080,
    DanglyFlag = 0x00000100,
    AABBFlag = 0x00000200,
    SaberFlag = 0x00000800,
}
