using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryReferenceHeader
{
    public static readonly int SIZE = 36;

    public string ModelResRef { get; set; } = "";
    public uint Reattachable { get; set; }

    public MDLBinaryReferenceHeader()
    {
    }
    public MDLBinaryReferenceHeader(MDLBinaryReader reader)
    {
        ModelResRef = reader.ReadString(32);
        Reattachable = reader.ReadUInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(ModelResRef.Resize(32));
        writer.Write(Reattachable);
    }
}
