using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTM;

public class UTMItemCollection : IEnumerable<UTMItem>
{
    private GFF _source;
    private GFFList? _itemList => _source.Root.GetList("ItemList");

    public UTMItemCollection(GFF source)
    {
        _source = source;
    }

    public UTMItem this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _itemList?.Count() ?? 0;
    public IEnumerator<UTMItem> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new item to the store.
    /// </summary>
    public UTMItem Add(ResRef resref, bool infinite)
    {
        var itemList = CreateListIfItDoesNotExist();
        var itemStruct = itemList.Add((uint)itemList.Count);

        return new UTMItem(_source, itemStruct)
        {
            ResRef = resref,
            Infinite = infinite,
            Repos_PosX = (ushort)itemList.Count,
        };
    }

    /// <summary>
    /// Removes all items from the store.
    /// </summary>
    public void Clear()
    {
        var itemList = CreateListIfItDoesNotExist();
        itemList.Clear();
    }

    private IEnumerable<UTMItem> All()
    {
        return _itemList?.Select(x => new UTMItem(_source, x)) ?? new List<UTMItem>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _itemList ?? _source.Root.SetList("ItemList");
    }
}
