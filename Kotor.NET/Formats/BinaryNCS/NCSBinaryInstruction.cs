using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryNCS;

public class NCSBinaryInstruction
{
    public byte ByteCode { get; set; }
    public byte Type { get; set; }
    public byte[] Tail { get; set; } = new byte[0];

    public NCSBinaryInstruction()
    {

    }
    public NCSBinaryInstruction(BinaryReader reader)
    {
        ByteCode = reader.ReadByte();
        Type = reader.ReadByte();

        var byteCode = (NCSBinaryByteCode)ByteCode;
        var type = (NCSBinaryType)Type;

        Tail = byteCode switch
        {
            NCSBinaryByteCode.CPDOWNSP => reader.ReadBytes(6),
            NCSBinaryByteCode.RSADD => reader.ReadBytes(0),
            NCSBinaryByteCode.CPTOPSP => reader.ReadBytes(6),
            NCSBinaryByteCode.CONST => (type == NCSBinaryType.String) ? ReadStringBytes(reader) : reader.ReadBytes(4),
            NCSBinaryByteCode.ACTION => reader.ReadBytes(3),
            NCSBinaryByteCode.LOGAND => reader.ReadBytes(0),
            NCSBinaryByteCode.LOGOR => reader.ReadBytes(0),
            NCSBinaryByteCode.INCOR => reader.ReadBytes(0),
            NCSBinaryByteCode.EXCOR => reader.ReadBytes(0),
            NCSBinaryByteCode.BOOLAND => reader.ReadBytes(0),
            NCSBinaryByteCode.EQUAL => (type == NCSBinaryType.StructureStructure) ? reader.ReadBytes(2) : reader.ReadBytes(0),
            NCSBinaryByteCode.NEQUAL => (type == NCSBinaryType.StructureStructure) ? reader.ReadBytes(2) : reader.ReadBytes(0),
            NCSBinaryByteCode.GEQ => reader.ReadBytes(0),
            NCSBinaryByteCode.GT => reader.ReadBytes(0),
            NCSBinaryByteCode.LT => reader.ReadBytes(0),
            NCSBinaryByteCode.LEQ => reader.ReadBytes(0),
            NCSBinaryByteCode.SHLEFT => reader.ReadBytes(0),
            NCSBinaryByteCode.SHRIGHT => reader.ReadBytes(0),
            NCSBinaryByteCode.USHRIGHT => reader.ReadBytes(0),
            NCSBinaryByteCode.ADD => reader.ReadBytes(0),
            NCSBinaryByteCode.SUB => reader.ReadBytes(0),
            NCSBinaryByteCode.MUL => reader.ReadBytes(0),
            NCSBinaryByteCode.DIV => reader.ReadBytes(0),
            NCSBinaryByteCode.MOD => reader.ReadBytes(0),
            NCSBinaryByteCode.MOVSP => reader.ReadBytes(4),
            NCSBinaryByteCode.JMP => reader.ReadBytes(4),
            NCSBinaryByteCode.JSR => reader.ReadBytes(4),
            NCSBinaryByteCode.JZ => reader.ReadBytes(4),
            NCSBinaryByteCode.RETN => reader.ReadBytes(0),
            NCSBinaryByteCode.DESTRUCT => reader.ReadBytes(6),
            NCSBinaryByteCode.NOT => reader.ReadBytes(0),
            NCSBinaryByteCode.DECSP => reader.ReadBytes(4),
            NCSBinaryByteCode.INCSP => reader.ReadBytes(4),
            NCSBinaryByteCode.JNZ => reader.ReadBytes(4),
            NCSBinaryByteCode.CPDOWNBP => reader.ReadBytes(6),
            NCSBinaryByteCode.CPTOPBP => reader.ReadBytes(6),
            NCSBinaryByteCode.DECBP => reader.ReadBytes(4),
            NCSBinaryByteCode.INCBP => reader.ReadBytes(4),
            NCSBinaryByteCode.SAVEBP => reader.ReadBytes(0),
            NCSBinaryByteCode.RESTOREBP => reader.ReadBytes(0),
            NCSBinaryByteCode.STORE_STATE => reader.ReadBytes(8),
            NCSBinaryByteCode.NOP => reader.ReadBytes(0),
            _ => throw new Exception()
        };
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Tail);
    }

    private byte[] ReadStringBytes(BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        var n = BinaryPrimitives.ReadInt16BigEndian(bytes);
        return bytes.Concat(reader.ReadBytes(n)).ToArray();
    }
}
