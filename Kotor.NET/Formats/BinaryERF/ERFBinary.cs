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
        try
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

            writer.BaseStream.Position = FileHeader.OffsetToKeyList;
            foreach (var entry in KeyEntries)
            {
                entry.Write(writer);
            }

            writer.BaseStream.Position = FileHeader.OffsetToResourceList;
            foreach (var entry in ResourceEntries)
            {
                entry.Write(writer);
            }

            for (int i = 0; i < ResourceData.Count; i++)
            {
                writer.BaseStream.Position = ResourceEntries[i].Offset;
                writer.Write(ResourceData[i]);
            }
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to write the 2DA data.", ex);
        }
    }

    public void Recalculate()
    {
        FileHeader.EntryCount = ResourceData.Count;

        var offset = ERFBinaryFileHeader.SIZE;
        FileHeader.OffsetToKeyList = offset;

        offset += KeyEntries.Count * ERFBinaryKeyEntry.SIZE;
        FileHeader.OffsetToResourceList = offset;

        offset += KeyEntries.Count * ERFBinaryResourceEntry.SIZE;
        ResourceEntries.Clear();
        for (int i = 0; i < ResourceData.Count; i++)
        {
            ResourceEntries.Add(new ERFBinaryResourceEntry
            {
                Offset = offset,
                Size = ResourceData[i].Length,
            });
            offset += ResourceData[i].Length;
        }
    }
}
