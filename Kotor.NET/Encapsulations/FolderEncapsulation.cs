using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Encapsulations;

public class FolderEncapsulation : IEncapsulation
{
    public string Path { get; private set; }
    public int Count { get; private set; }
    public bool TrackERFs { get; private set; }

    private readonly List<ResourceInfo> _resources = new List<ResourceInfo>();
    private readonly List<IEncapsulation> _erfs = new List<IEncapsulation>();

    public FolderEncapsulation(string path, bool trackERFs = false)
    {
        Path = path;
        TrackERFs = trackERFs;

        Reload();
    }

    public void Delete(string resref, ResourceType type)
    {
        // TODO Handle ERF

        var filepath = System.IO.Path.Combine(Path, resref + "." + type.Extension);
        File.Delete(filepath);
    }

    public ResourceInfo Find(string resref, ResourceType type)
    {
        // TODO Handle ERF

        return _resources.FirstOrDefault(x => x.ResRef.ToLower() == resref.ToLower() && x.Type == type);
    }

    public byte[] Read(string resref, ResourceType type)
    {
        // TODO Handle ERF

        var filepath = System.IO.Path.Combine(Path, resref + "." + type.Extension);
        return File.ReadAllBytes(filepath);
    }

    public void Reload()
    {
        var files = new DirectoryInfo(Path).EnumerateFiles();
        foreach (var file in files)
        {
            var resource = new ResourceInfo()
            {
                FilePath = file.FullName,
                ResRef = System.IO.Path.GetFileNameWithoutExtension(file.Name),
                Type = ResourceType.FromFilepath(file.FullName),
                Size = (int)file.Length,
                Offset = 0
            };
            _resources.Add(resource);
        }

        if (TrackERFs)
        {
            // TODO
        }
    }

    public void Write(string resref, ResourceType type, byte[] data)
    {
        // TODO Handle ERF

        var filepath = System.IO.Path.Combine(Path, resref + "." + type.Extension);
        File.WriteAllBytes(filepath, data);
    }

    public IEnumerator<ResourceInfo> GetEnumerator() => _erfs.SelectMany(x => x).Concat(_resources).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
