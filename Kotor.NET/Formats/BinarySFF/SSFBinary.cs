using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinarySSF;

public class SSFBinary
{
    public SSFBinaryFileHeader FileHeader { get; set; } = new();
    public SSFBinarySoundList SoundList { get; set; } = new();

    public SSFBinary()
    {

    }
    public SSFBinary(BinaryReader reader)
    {
        FileHeader = new SSFBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToSounds;
        SoundList = new SSFBinarySoundList(reader);
    }

    public void Write(BinaryWriter writer)
    {
        FileHeader.Write(writer);
        SoundList.Write(writer);
    }

    public void Recalculate()
    {
        FileHeader.OffsetToSounds = SSFBinaryFileHeader.SIZE;

        if (SoundList.Sounds.Length > 40)
        {
            SoundList.Sounds = SoundList.Sounds.Take(40).ToArray();
        }
        else if (SoundList.Sounds.Length < 40)
        {
            SoundList.Sounds = SoundList
                .Sounds.ToList()
                .Concat(Enumerable.Repeat(UInt32.MaxValue, 40- SoundList.Sounds.Length))
                .ToArray();
        }
    }
}
