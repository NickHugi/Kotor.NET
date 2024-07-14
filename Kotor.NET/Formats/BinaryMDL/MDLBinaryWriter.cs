using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryWriter : BinaryWriter
{
    public MDLBinaryWriter(Stream input) : base(input)
    {
    }

    public void SetStreamPosition(int position)
    {
        BaseStream.Position = position + 12;
    }
}
