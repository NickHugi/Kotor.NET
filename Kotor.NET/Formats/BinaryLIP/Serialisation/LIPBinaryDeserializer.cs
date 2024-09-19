using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorLIP;

namespace Kotor.NET.Formats.BinaryLIP.Serialisation;

public class LIPBinaryDeserializer
{
    private LIPBinary _binary { get; }

    public LIPBinaryDeserializer(LIPBinary binary)
    {
        _binary = binary;
    }

    public LIP Deserialize()
    {
        var lip = new LIP();

        _binary.KeyFrames.ForEach(frame =>
        {
            lip.Add(frame.Time, (LIPMouthShape)frame.Shape);
        });

        return lip;
    }
}
