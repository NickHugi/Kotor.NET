using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryKEY;

public class KEYBinaryFileEntry
{
    public static readonly int SIZE = 12;

    public int FileSize { get; set; }
    public int FilenameOffset { get; set; }
    public short FilenameLength { get; set; }
    public ushort Drives { get; set; }

    public KEYBinaryFileEntry(BinaryReader reader)
    {
        FileSize = reader.ReadInt32();
        FilenameOffset = reader.ReadInt32();
        FilenameLength = reader.ReadInt16();
        Drives = reader.ReadUInt16();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileSize);
        writer.Write(FilenameOffset);
        writer.Write(FilenameLength);
        writer.Write(Drives);
    }
}
