using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryNCS;

public enum NCSBinaryType
{
    // Unary
    Integer = 3,
    Float = 4,
    String = 5,
    Object = 6,
    Effect = 16,
    Event = 17,
    Location = 18,
    Talent = 19,

    // Binary
    IntegerInteger = 32,
    FloatFloat = 33,
    ObjectObject = 34,
    StringString = 35,
    StructureStructure = 36,
    IntegerFloat = 37,
    FloatInteger = 38,
    EffectEffect = 48,
    EventEffect = 49,
    LocationLocation = 50,
    TalentTalent = 51,
    VectorVector = 58,
    VectorFloat = 59,
    FloatVector = 60,
}
