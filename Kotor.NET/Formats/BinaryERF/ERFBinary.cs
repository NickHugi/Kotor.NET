using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;

namespace Kotor.NET.Formats.BinaryERF;

public class ERFBinary
{
    public ERFBinaryFileHeader FileHeader { get; set; } = new();
    public List<ERFBinaryKeyEntry> KeyEntries { get; set; } = new();
    public List<ERFBinaryResourceEntry> ResourceEntries { get; set; } = new();
    public List<byte[]> ResourceData { get; set; } = new();

    public ERFBinary()
    {

    }
    public ERFBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new ERFBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToKeyList;
        for (int i = 0; i < FileHeader.EntryCount; i++)
        {
            KeyEntries.Add(new ERFBinaryKeyEntry(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToResourceList;
        for (int i = 0; i < FileHeader.EntryCount; i++)
        {
            ResourceEntries.Add(new ERFBinaryResourceEntry(reader));
        }

        foreach (var entry in ResourceEntries)
        {
            reader.BaseStream.Position = entry.Offset;
            ResourceData.Add(reader.ReadBytes(entry.Size));
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        foreach (var entry in KeyEntries)
        {
            entry.Write(writer);
        }

        foreach (var entry in ResourceEntries)
        {
            entry.Write(writer);
        }

        foreach (var data in ResourceData)
        {
            writer.Write(data);
        }
    }

    public void Recalculate()
    {
        FileHeader.EntryCount = ResourceData.Count;

        var offset = ERFBinaryFileHeader.SIZE;
        FileHeader.OffsetToKeyList = offset;

        offset += KeyEntries.Count * ERFBinaryKeyEntry.SIZE;
        FileHeader.OffsetToResourceList = offset;

        offset += ResourceEntries.Count * ERFBinaryResourceEntry.SIZE;
        for (int i = 0; i < ResourceData.Count; i++)
        {
            ResourceEntries[i].Offset = offset;
            ResourceEntries[i].Size = ResourceData[i].Length;
            offset += ResourceData[i].Length;
        }
    }
}
