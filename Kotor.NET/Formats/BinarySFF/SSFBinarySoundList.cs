using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinarySSF;


public class SSFBinarySoundList
{
    public static readonly int SIZE = 160;

    public uint[] Sounds { get; set; } = Enumerable.Repeat(UInt32.MaxValue, 40).ToArray();

    public SSFBinarySoundList()
    {

    }
    public SSFBinarySoundList(BinaryReader reader)
    {
        for (int i = 0; i < 40; i++)
        {
            Sounds[i] = reader.ReadUInt32();
        }
    }

    public void Write(BinaryWriter writer)
    {
        for (int i = 0; i < 40; i++)
        {
            writer.Write(Sounds[i]);
        }
    }
}
