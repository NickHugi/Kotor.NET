using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryAnimationEvent
{
    public static readonly int SIZE = 36;

    public float ActivationTime { get; set; }
    public string Name { get; set; } = "";

    public MDLBinaryAnimationEvent()
    {
    }
    public MDLBinaryAnimationEvent(MDLBinaryReader reader)
    {
        ActivationTime = reader.ReadSingle();
        Name = reader.ReadString(32);
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(ActivationTime);
        writer.Write(Name.Resize(32), 0);
    }
}
