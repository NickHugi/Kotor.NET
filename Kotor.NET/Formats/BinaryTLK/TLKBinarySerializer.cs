using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Formats.BinaryTLK;

public class TLKBinarySerializer
{
    private TLK _tlk { get; }

    public TLKBinarySerializer(TLK tlk)
    {
        _tlk = tlk;
    }

    public TLKBinary Serialize()
    {
        var binary = new TLKBinary();

        binary.FileHeader.FileType = TLKBinaryFileHeader.FILE_TYPES[0];
        binary.FileHeader.FileVersion = TLKBinaryFileHeader.FILE_VERSION;

        foreach (var entry in _tlk)
        {
            binary.Strings.Add(entry.Text);
            binary.Entries.Add(new()
            {
                SoundResRef = entry.Sound,
            });
        }

        binary.Recalculate();

        return binary;
    }
}
