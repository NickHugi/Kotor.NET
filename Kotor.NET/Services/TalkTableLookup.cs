using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Localization;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTLK;
using Kotor.NET.Interfaces;

namespace Kotor.NET.Services;

public class TalkTableLookup : ITalkTableLookup
{
    public TalkTableString Locate(string path, StringRef stringref)
    {
        using var stream = File.Open(path, FileMode.Open);
        using var reader = new BinaryReader(stream);

        var fileHeader = new TLKBinaryFileHeader(reader);

        reader.BaseStream.Position = TLKBinaryFileHeader.SIZE + (TLKBinaryEntry.SIZE * stringref);
        var entry = new TLKBinaryEntry(reader);

        reader.BaseStream.Position = fileHeader.OffsetToEntries + entry.OffsetToString;
        var text = reader.ReadString(entry.StringSize);

        return new(stringref, text, entry.SoundResRef);
    }
}


