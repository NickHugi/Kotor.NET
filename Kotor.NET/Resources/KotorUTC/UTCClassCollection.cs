using System.Collections;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCClassCollection : IEnumerable<UTCClass>
{
    private GFF _source;

    public UTCClassCollection(GFF source)
    {
        _source = source;
    }

    public UTCClass this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _source.Root.GetList("ClassList")?.Count() ?? 0;
    public IEnumerator<UTCClass> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new class of the specified ID and level to the creature.
    /// </summary>
    public UTCClass Add(int classID, short level)
    {
        var classList = CreateListIfItDoesNotExist();
        var classStruct = classList.Add(2);

        return new UTCClass(_source, classStruct)
        {
            ClassID = classID,
            Level = level,
        };
    }

    /// <summary>
    /// Removes all classes from the creature.
    /// </summary>
    public void Clear()
    {
        var classList = CreateListIfItDoesNotExist();
        classList.Clear();
    }

    private IEnumerable<UTCClass> All()
    {
        return _source.Root.GetList("ClassList")?.Select(x => new UTCClass(_source, x)) ?? new List<UTCClass>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _source.Root.GetList("ClassList") ?? _source.Root.SetList("ClassList");
    }
}
