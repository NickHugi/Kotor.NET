using System.Collections;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCFeatCollection : IEnumerable<UTCFeat>
{
    private GFF _source;
    private GFFList? _featList => _source.Root.GetList("FeatList");

    public UTCFeatCollection(GFF source)
    {
        _source = source;
    }

    public UTCFeat this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _featList?.Count() ?? 0;
    public IEnumerator<UTCFeat> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new class of the specified ID and level to the creature.
    /// </summary>
    public UTCFeat Add(ushort featID)
    {
        var itemList = CreateListIfItDoesNotExist();
        var itemStruct = itemList.Add((uint)itemList.Count);

        return new UTCFeat(_source, itemStruct)
        {
            FeatID = featID
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

    private IEnumerable<UTCFeat> All()
    {
        return _featList?.Select(x => new UTCFeat(_source, x)) ?? new List<UTCFeat>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _featList ?? _source.Root.SetList("FeatList");
    }
}
