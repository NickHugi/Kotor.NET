using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTS;

public class UTSSoundCollection : IEnumerable<UTSSound>
{
    private GFF _source;
    private GFFList? _soundsList => _source.Root.GetList("Sounds");

    public UTSSoundCollection(GFF source)
    {
        _source = source;
    }

    public UTSSound this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _soundsList?.Count() ?? 0;
    public IEnumerator<UTSSound> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new sound.
    /// </summary>
    public UTSSound Add(ResRef resref)
    {
        var soundList = CreateListIfItDoesNotExist();
        var soundStruct = soundList.Add((uint)soundList.Count);

        return new UTSSound(_source, soundStruct)
        {
            ResRef = resref,
        };
    }

    /// <summary>
    /// Removes all sounds.
    /// </summary>
    public void Clear()
    {
        var soundList = CreateListIfItDoesNotExist();
        soundList.Clear();
    }

    private IEnumerable<UTSSound> All()
    {
        return _soundsList?.Select(x => new UTSSound(_source, x)) ?? new List<UTSSound>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _soundsList ?? _source.Root.SetList("Sounds");
    }
}
