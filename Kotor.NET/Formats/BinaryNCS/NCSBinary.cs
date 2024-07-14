using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryNCS;

public class NCSBinary
{
    public NCSBinaryFileHeader FileHeader { get; set; } = new();
    public List<NCSBinaryInstruction> Instructions { get; set; } = new();

    public NCSBinary()
    {
    }
    public NCSBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);
        FileHeader = new(reader);

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            Instructions.Add(new NCSBinaryInstruction(reader));
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);
    }

    public void Recalculate()
    {
        FileHeader.ProgramSize = NCSBinaryFileHeader.SIZE + Instructions.Sum(x => x.Tail.Length) + (Instructions.Count() * 2);
    }
}
