using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryERF;

namespace Kotor.NET.Formats.BinaryRIM;

public class RIMBinary
{
    public RIMBinaryFileHeader FileHeader { get; set; } = new();
    public List<RIMBinaryResourceEntry> ResourceEntries { get; set; } = new();
    public List<byte[]> ResourceData { get; set; } = new();

    public RIMBinary()
    {

    }
    public RIMBinary(BinaryReader reader)
    {
        FileHeader = new RIMBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToResources;
        for (int i = 0; i < FileHeader.ResourceCount; i++)
        {
            ResourceEntries.Add(new RIMBinaryResourceEntry(reader));
        }

        foreach (var entry in ResourceEntries)
        {
            reader.BaseStream.Position = entry.Offset;
            ResourceData.Add(reader.ReadBytes(entry.Size));
        }
    }

    public void Write(BinaryWriter writer)
    {
        FileHeader.Write(writer);

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
        FileHeader.ResourceCount = ResourceData.Count;

        var offset = RIMBinaryFileHeader.SIZE;
        FileHeader.OffsetToResources = offset;

        offset += ResourceEntries.Count * RIMBinaryResourceEntry.SIZE;
        for (int i = 0; i < ResourceData.Count; i++)
        {
            ResourceEntries[i].Offset = offset;
            ResourceEntries[i].Size = ResourceData[i].Length;
            offset += ResourceData[i].Length;
        }
    }
}
