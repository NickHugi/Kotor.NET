using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Tests.Encapsulation;

public class ResourceInfo
{
    public required string FilePath { get; init; }
    public required string ResRef { get; init; }
    public required ResourceType Type { get; init; }
    public required int Size { get; init; }
    public required int Offset { get; init; }

    public ResourceInfo()
    {
    }
    public ResourceInfo(string filePath, string resRef, ResourceType type, int size, int offset)
    {
        FilePath = filePath;
        ResRef = resRef;
        Type = type;
        Size = size;
        Offset = offset;
    }
    [SetsRequiredMembers]
    public ResourceInfo(string filePath)
    {
        FilePath = filePath;
        ResRef = Path.GetFileNameWithoutExtension(filePath);
        Type = ResourceType.FromFilepath(filePath);
        Size = (int)new FileInfo(filePath).Length;
        Offset = 0;
    }

    public Stream OpenStream()
    {
        return File.Open(FilePath, FileMode.Open);
    }

    public byte[] ReadData()
    {
        using var stream = OpenStream();
        using var reader = new BinaryReader(stream);
        stream.Position = Offset;
        return reader.ReadBytes(Size);
    }
}
