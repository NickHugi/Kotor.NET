using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCItemCollection : IEnumerable<UTCItem>
{
    private GFF _source;
    private GFFList? _itemList => _source.Root.GetList("ItemList");

    public UTCItemCollection(GFF source)
    {
        _source = source;
    }

    public UTCItem this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _itemList?.Count() ?? 0;
    public IEnumerator<UTCItem> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new class of the specified ID and level to the creature.
    /// </summary>
    public UTCItem Add(ResRef resref, bool droppable)
    {
        var itemList = CreateListIfItDoesNotExist();
        var itemStruct = itemList.Add((uint)itemList.Count);

        return new UTCItem(_source, itemStruct)
        {
            ResRef = resref,
            Droppable = droppable,
            Repos_PosX = (ushort)itemList.Count,
        };
    }

    /// <summary>
    /// Removes all classes from the creature.
    /// </summary>
    public void Clear()
    {
        var itemList = CreateListIfItDoesNotExist();
        itemList.Clear();
    }

    private IEnumerable<UTCItem> All()
    {
        return _itemList?.Select(x => new UTCItem(_source, x)) ?? new List<UTCItem>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _itemList ?? _source.Root.SetList("ItemList");
    }
}
