using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.NET.Encapsulations;

public class Installation
{
    public string Directory { get; }
    public GameEngine Engine { get; }
    public Platform Platform { get; }

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
}
