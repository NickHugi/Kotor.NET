using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.FileFormats.KotorNCS
{
    public class NCS
    {
    }

    public enum Instruction
    {
        NOP = 0x2D,
        CPDOWNSP = 0x01,
        RSADDx = 0x02,
        CPTOPSP = 0x03,
        CONSTx = 0x04,
        ACTION = 0x05,
        LOGANDxx = 0x06,
        LOGORxx = 0x07,
        INCORxx = 0x08,
        EXCORxx = 0x09,
        BOOLAND = 0x0A,
        EQUALxx = 0x0B,
        NEQUALxx = 0x0C,
        GEQxx = 0x0D,
        GTxx = 0x0E,
        LTxx = 0x0F,
        LEQxx = 0x10,
        SHLEFTxx = 0x11,
        SHRIGHTxx = 0x12,
        USHRIGHTxx = 0x13,
        ADDxx = 0x14,
        SUBxx = 0x15,
        MULxx = 0x16,
        DIVxx = 0x17,
        MODxx = 0x18,
        NEGx = 0x19,
        COMPx = 0x1A,
        MOVSP = 0x1B,
        JMP = 0x1D,
        JSR = 0x1E,
        JZ = 0x1F,
        CPDOWNBP = 0x26,
        CPTOPBP = 0x27,
        DECxBP = 0x28,
        INCxBP = 0x29,
        SAVEBP = 0x2A,
        RESTOREBP = 0x2B,
        STORESTATE = 0x2C,
        NOP2 = 0x2D,
    }

    public enum InstructionQualifier
    {
        INT = 0x03,
        FLOAT = 0x04,
        STRING = 0x05,
        OBJECT = 0x06,
        EFFECT = 0x10,
        EVENT = 0x11,
        LOCATION = 0x12,
        TALENT = 0x13,
        INT_INT =  0x20,
        FLOAT_FLOAT = 0x21,
        OBJECT_BOJECT = 0x22,
        STRING_STRING = 0x23,
        STRUCT_STRUCT = 0x24,
        INT_FLOAT = 0x25,
        FLOAT_INT = 0x26,
        EFFECT_EFFECT = 0x30,
        EVENT_EVENT = 0x31,
        LOCATION_LOCATION = 0x32,
        TALENT_TALENT = 0x33,
        VECTOR_VECTOR = 0x3A,
        VECTOR_FLOAT = 0x3B,
        FLOAT_VECTOR = 0x3C,
    }
}
