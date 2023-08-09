using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryERF;

public class ERFBinaryKeyEntry
{
    public static readonly int SIZE = 24;

    public ResRef ResRef { get; set; } = "";
    public uint ResID { get; set; }
    public ushort ResType { get; set; }

    public ERFBinaryKeyEntry()
    {

    }
    public ERFBinaryKeyEntry(BinaryReader reader)
    {
        ResRef = reader.ReadResRef();
        ResID = reader.ReadUInt32();
        ResType = reader.ReadUInt16();
        reader.BaseStream.Position += 2;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(ResRef, false);
        writer.Write(ResID);
        writer.Write(ResType);
        writer.Write((short)0);
    }
}
