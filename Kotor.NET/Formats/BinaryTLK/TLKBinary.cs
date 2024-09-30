using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryTLK;

public class TLKBinary
{
    public TLKBinaryFileHeader FileHeader { get; set; } = new();
    public List<TLKBinaryEntry> Entries { get; set; } = new();
    public List<string> Strings { get; set; } = new();

    public TLKBinary()
    {

    }

    public TLKBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);
        FileHeader = new TLKBinaryFileHeader(reader);

        for (int i = 0; i < FileHeader.EntryCount; i++)
        {
            Entries.Add(new TLKBinaryEntry(reader));
        }

        foreach (var entry in Entries)
        {
            reader.BaseStream.Position = entry.OffsetToString + FileHeader.OffsetToEntries;
            var text = reader.ReadString(entry.StringSize);
            Strings.Add(text);
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        foreach (var stringData in Entries)
        {
            stringData.Write(writer);
        }

        foreach (var entry in Strings)
        {
            writer.Write(entry, 0);
        }
    }

    public void Recalculate()
    {
        FileHeader.EntryCount = Entries.Count;

        var offset = TLKBinaryFileHeader.SIZE + (TLKBinaryEntry.SIZE * Entries.Count());
        FileHeader.OffsetToEntries = offset;

        var offsetToString = 0;
        for (int i = 0; i < Entries.Count; i++)
        {
            Entries[i].StringSize = Strings[i].Length;
            Entries[i].OffsetToString = offsetToString;
            offsetToString += Strings[i].Length;
        }
    }
}
