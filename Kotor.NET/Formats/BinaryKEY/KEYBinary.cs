using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;
using Kotor.NET.Resources;

namespace Kotor.NET.Formats.BinaryKEY;

public class KEYBinary
{
    public KEYBinaryFileHeader FileHeader { get; set; }
    public List<KEYBinaryFileEntry> FileTable { get; set; }
    public List<string> Filenames { get; set; }
    public List<KEYBinaryKeyEntry> Keys { get; set; }

    public KEYBinary(BinaryReader reader)
    {
        FileHeader = new KEYBinaryFileHeader(reader);

        FileTable = new List<KEYBinaryFileEntry>();
        reader.BaseStream.Position = FileHeader.OffsetToFileEntries;
        for (int i = 0; i < FileHeader.FileCount; i++)
        {
            FileTable.Add(new KEYBinaryFileEntry(reader));
        }

        Filenames = new List<string>();
        foreach (var file in FileTable)
        {
            reader.BaseStream.Position = file.FilenameOffset;
            var filename = reader.ReadString(file.FilenameLength);
            Filenames.Add(filename);
        }

        Keys = new List<KEYBinaryKeyEntry>();
        reader.BaseStream.Position = FileHeader.OffsetToKeyEntries;
        for (int i = 0; i < FileHeader.KeyCount; i++)
        {
            Keys.Add(new KEYBinaryKeyEntry(reader));
        }
    }

    public void Write(BinaryWriter writer)
    {
        FileHeader.Write(writer);

        writer.BaseStream.Position = FileHeader.OffsetToFileEntries;
        foreach (var file in FileTable)
        {
            file.Write(writer);
        }

        for (int i = 0; i < FileTable.Count; i++)
        {
            writer.BaseStream.Position = FileTable[i].FilenameOffset;
            writer.Write(Filenames[i], 0);
        }

        foreach (var key in Keys)
        {
            key.Write(writer);
        }
    }

    public void Recalculate()
    {
        FileHeader.FileCount = FileTable.Count;
        FileHeader.KeyCount = Keys.Count;

        var offset = KEYBinaryFileHeader.SIZE;
        FileHeader.OffsetToFileEntries = offset;

        offset += KEYBinaryFileEntry.SIZE * FileHeader.FileCount;
        for (int i = 0; i < FileTable.Count; i++)
        {
            FileTable[i].FilenameOffset = offset;
            FileTable[i].FilenameLength = (short)Filenames[i].Length;
            offset += Filenames[i].Length;
        }

        FileHeader.OffsetToKeyEntries = offset;
    }
}
