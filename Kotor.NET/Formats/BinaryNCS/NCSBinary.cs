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
        try
        {
            var reader = new BinaryReader(stream);
            FileHeader = new(reader);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                Instructions.Add(new NCSBinaryInstruction(reader));
            }
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to read the 2DA data.", ex);
        }
    }

    public void Write(Stream stream)
    {
        try
        {
            var writer = new BinaryWriter(stream);

            FileHeader.Write(writer);
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to write the 2DA data.", ex);
        }
    }

    public void Recalculate()
    {
        FileHeader.ProgramSize = NCSBinaryFileHeader.SIZE + Instructions.Sum(x => x.Tail.Length) + (Instructions.Count() * 2);
    }
}
