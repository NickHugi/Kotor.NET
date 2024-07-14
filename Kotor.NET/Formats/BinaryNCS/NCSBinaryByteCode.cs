using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryNCS;

public enum NCSBinaryByteCode
{
    CPDOWNSP = 0x01,
    RSADD = 0x02,
    CPTOPSP = 0x03,
    CONST = 0x04,
    ACTION = 0x05,
    LOGAND = 0x06,
    LOGOR = 0x07,
    INCOR = 0x08,
    EXCOR = 0x09,
    BOOLAND = 0x0A,
    EQUAL = 0x0B,
    NEQUAL = 0x0C,
    GEQ = 0x0D,
    GT = 0x0E,
    LT = 0x0F,
    LEQ = 0x10,
    SHLEFT = 0x11,
    SHRIGHT = 0x12,
    USHRIGHT = 0x13,
    ADD = 0x14,
    SUB = 0x15,
    MUL = 0x16,
    DIV = 0x17,
    MOD = 0x18,
    NEG = 0x19,
    COMP = 0x1A,
    MOVSP = 0x1B,
    [Obsolete] STORE_STATEALL = 0x1C,
    JMP = 0x1D,
    JSR = 0x1E,
    JZ = 0x1F,
    RETN = 0x20,
    DESTRUCT = 0x21,
    NOT = 0x22,
    DECSP = 0x23,
    INCSP = 0x24,
    JNZ = 0x25,
    CPDOWNBP = 0x26,
    CPTOPBP = 0x27,
    DECBP = 0x28,
    INCBP = 0x29,
    SAVEBP = 0x2A,
    RESTOREBP = 0x2B,
    STORE_STATE = 0x02C,
    NOP = 0x2D,
    T = 0x42,
}
