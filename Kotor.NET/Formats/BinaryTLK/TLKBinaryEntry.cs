using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryTLK;

public class TLKBinaryEntry
{
    public static readonly int SIZE = 40;

    public uint Flags { get; set; }
    public ResRef SoundResRef { get; set; }
    public uint VolumeVariance { get; set; }
    public uint PitchVariance { get; set; }
    public int OffsetToString { get; set; }
    public int StringSize { get; set; }
    public float Length { get; set; }

    public TLKBinaryEntry()
    {
        SoundResRef = "";
    }

    public TLKBinaryEntry(BinaryReader reader)
    {
        Flags = reader.ReadUInt32();
        SoundResRef = reader.ReadResRef();
        VolumeVariance = reader.ReadUInt32();
        PitchVariance = reader.ReadUInt32();
        OffsetToString = reader.ReadInt32();
        StringSize = reader.ReadInt32();
        Length = reader.ReadSingle();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Flags);
        writer.Write(SoundResRef, false);
        writer.Write(VolumeVariance);
        writer.Write(PitchVariance);
        writer.Write(OffsetToString);
        writer.Write(StringSize);
        writer.Write(Length);
    }
}
