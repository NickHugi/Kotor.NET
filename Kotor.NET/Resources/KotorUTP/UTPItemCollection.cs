using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTP;

public class UTPItemCollection : IEnumerable<UTPItem>
{
    private GFF _source;
    private GFFList? _itemList => _source.Root.GetList("ItemList");

    public UTPItemCollection(GFF source)
    {
        _source = source;
    }

    public UTPItem this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _itemList?.Count() ?? 0;
    public IEnumerator<UTPItem> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new item to the placeable.
    /// </summary>
    public UTPItem Add(ResRef resref)
    {
        var itemList = CreateListIfItDoesNotExist();
        var itemStruct = itemList.Add((uint)itemList.Count);

        return new UTPItem(_source, itemStruct)
        {
            ResRef = resref,
            Repos_PosX = (ushort)itemList.Count,
        };
    }

    /// <summary>
    /// Removes all items from the placeable.
    /// </summary>
    public void Clear()
    {
        var itemList = CreateListIfItDoesNotExist();
        itemList.Clear();
    }

    private IEnumerable<UTPItem> All()
    {
        return _itemList?.Select(x => new UTPItem(_source, x)) ?? new List<UTPItem>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _itemList ?? _source.Root.SetList("ItemList");
    }
}
