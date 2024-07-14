using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryLTR;

public class LTRBinary
{
    public LTRBinaryFileHeader FileHeader { get; set; } = new();
    public LTRBinaryBlock Singles { get; set; }
    public LTRBinaryBlock[] Doubles { get; set; }
    public LTRBinaryBlock[][] Triples { get; set; }

    public LTRBinary(byte size)
    {
        Singles = new LTRBinaryBlock(size);
        Doubles = Enumerable.Range(0, size).Select(x => new LTRBinaryBlock(size)).ToArray();
        Triples = Enumerable.Range(0, size).Select(x => Enumerable.Range(0, size).Select(x => new LTRBinaryBlock(size)).ToArray()).ToArray();
    }
    public LTRBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new(reader);

        Singles = new(reader, FileHeader.LetterCount);

        Doubles = new LTRBinaryBlock[FileHeader.LetterCount]; 
        for (int i = 0; i < FileHeader.LetterCount; i++)
        {
            Doubles[i] = new LTRBinaryBlock(reader, FileHeader.LetterCount);
        }

        Triples = new LTRBinaryBlock[FileHeader.LetterCount][];
        for (int i = 0; i < FileHeader.LetterCount; i++)
        {
            Triples[i] = new LTRBinaryBlock[FileHeader.LetterCount];
            for (int j = 0; j < FileHeader.LetterCount; j++)
            {
                Triples[i][j] = new LTRBinaryBlock(reader, FileHeader.LetterCount);
            }
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        Singles.Write(writer);

        Doubles = new LTRBinaryBlock[FileHeader.LetterCount];
        for (int i = 0; i < FileHeader.LetterCount; i++)
        {
            Doubles[i].Write(writer);
        }

        Triples = new LTRBinaryBlock[FileHeader.LetterCount][];
        for (int i = 0; i < FileHeader.LetterCount; i++)
        {
            Triples[i] = new LTRBinaryBlock[FileHeader.LetterCount];
            for (int j = 0; j < FileHeader.LetterCount; j++)
            {
                Triples[i][j].Write(writer);
            }
        }
    }

    public void Recalculate()
    {
        FileHeader.LetterCount = (byte)Singles.Start.Count();
    }
}
