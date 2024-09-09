using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorLIP;

namespace Kotor.NET.Formats.BinaryLIP;

public class LIPBinarySerializer
{
    private LIP _lip { get; }

    public LIPBinarySerializer(LIP lip)
    {
        _lip = lip;
    }

    public LIPBinary Serialize()
    {
        var binary = new LIPBinary();

        binary.FileHeader.FileType = LIPBinaryFileHeader.FILE_TYPES[0];
        binary.FileHeader.FileVersion = LIPBinaryFileHeader.FILE_VERSION;

        foreach (var frame in _lip)
        {
            binary.KeyFrames.Add(new LIPBinaryKeyFrame
            {
                Time = frame.Time,
                Shape = (byte)frame.Shape,
            });
        }
        
        binary.Recalculate();

        return binary;
    }
}
