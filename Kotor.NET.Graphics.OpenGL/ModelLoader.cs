using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics.OpenGL;

public class ModelLoader
{
    public KModel LoadModel(byte[] data)
    {
        data = data.Skip(12).ToArray();
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        reader.BaseStream.Position = 8;
        var name = reader.ReadString(32);

        var rootNodeOffset = reader.ReadByte();

        reader.BaseStream.Position = 184;
        var nameOffsetsArray = reader.ReadByte();
        var nameOffsetsCount = reader.ReadByte();

        reader.BaseStream.Position = nameOffsetsArray;
        var nameOffsets = new List<int>();
        for (int i = 0; i < nameOffsetsCount; i++)
        {
            nameOffsets.Add(reader.ReadInt32());
        }
        var names = new List<string>();
        foreach (var nameOffset in nameOffsets)
        {
            reader.BaseStream.Position = nameOffset;
            names.Add(reader.ReadTerminatedString('\n'));
        }
    }

    private BaseNode LoadNode(BinaryReader reader)
    {

    }
}
