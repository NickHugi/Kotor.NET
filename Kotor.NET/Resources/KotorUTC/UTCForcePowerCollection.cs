using System.Collections;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCForcePowerCollection : IEnumerable<UTCForcePower>
{
    private GFF _source;
    private GFFStruct _classSource;
    private GFFList? _forcePowerList => _classSource.GetList("KnownList0");

    public UTCForcePowerCollection(GFF source, GFFStruct classSource)
    {
        _source = source;
        _classSource = classSource;
    }

    public UTCForcePower this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _classSource.GetList("KnownList0")?.Count() ?? 0;
    public IEnumerator<UTCForcePower> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new class of the specified ID and level to the creature.
    /// </summary>
    public UTCForcePower Add(ushort forcePowerID)
    {
        var powerList = CreateListIfItDoesNotExist();
        var powerStruct = powerList.Add(2);

        return new UTCForcePower(_source, powerStruct)
        {
            ForcePowerID = forcePowerID,
        };
    }

    /// <summary>
    /// Removes all force powers from the class.
    /// </summary>
    public void Clear()
    {
        var powerList = CreateListIfItDoesNotExist();
        powerList.Clear();
    }

    private IEnumerable<UTCForcePower> All()
    {
        return _forcePowerList?.Select(x => new UTCForcePower(_source, x)) ?? new List<UTCForcePower>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _forcePowerList ?? _source.Root.SetList("ClassList");
    }
}
