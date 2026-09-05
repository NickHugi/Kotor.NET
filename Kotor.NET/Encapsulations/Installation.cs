using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Encapsulations;

public class Installation
{
    public string Directory { get; }
    public GameEngine Engine { get; }
    public Platform Platform { get; }

    public IEncapsulation Chitin => _chitin;

    private List<IEncapsulation> _additional { get; } = new();
    private List<IEncapsulation> _texturePackDirectory { get; } = new();
    private List<IEncapsulation> _modulesDirectory { get; } = new();
    private List<IEncapsulation> _rimsDirectory { get; } = new();
    private List<IEncapsulation> _lipsDirectory { get; } = new();
    private IEncapsulation _overrideDirectory { get; }
    private IEncapsulation _soundsDirectory { get; }
    private IEncapsulation _moviesDirectory { get; }
    private IEncapsulation _musicDirectory { get; }
    private IEncapsulation _voDirectory { get; }
    private IEncapsulation _chitin { get; }

    private IEnumerable<ResourceInfo> _allResources
    {
        get =>
        [
            .. _additional.SelectMany(x => x),
            .. _texturePackDirectory.SelectMany(x => x),
            .. _modulesDirectory.SelectMany(x => x),
            .. _rimsDirectory.SelectMany(x => x),
            .. _lipsDirectory.SelectMany(x => x),
            .. _overrideDirectory,
            .. _soundsDirectory,
            .. _moviesDirectory,
            .. _musicDirectory,
            .. _voDirectory,
            .. _chitin,
        ];
    }

    public Installation(string directory, GameEngine engine, Platform platform)
    {
        Directory = directory;
        Engine = engine;
        Platform = platform;

        _chitin = new KEYEncapsulation(Path.Combine(Directory, "chitin.key"));
        _overrideDirectory = new FolderEncapsulation(Path.Combine(Directory, "override"));
    }

    public IEncapsulation? Module(string id)
    {
        var erf = _modulesDirectory
            .OfType<ERFEncapsulation>()
            .SingleOrDefault(x => Path.Equals(Path.GetFileNameWithoutExtension(x.Path), id));

        if (erf is not null)
            return erf;

        return _modulesDirectory
            .OfType<RIMEncapsulation>()
            .SingleOrDefault(x => Path.Equals(Path.GetFileNameWithoutExtension(x.Path), id));
    }

    public IEncapsulation Override()
    {
        return _overrideDirectory;
    }

    public TwoDA Get2DA(ResRef filename)
    {
        var resource = _overrideDirectory.FirstOrDefault(x => x.ResRef == filename && x.Type == ResourceType.TWODA)
            ?? _chitin.FirstOrDefault(x => x.ResRef == filename && x.Type == ResourceType.TWODA);

        var data = resource.ReadData();
        return TwoDA.FromBytes(data);
    }
}
