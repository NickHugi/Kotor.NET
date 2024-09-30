using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTP;

public class UTPItem
{
    private GFF _source { get; }
    private GFFStruct _itemSource { get; }
    private GFFList? _itemList => _source.Root.GetList("ItemList");

    internal UTPItem(GFF source, GFFStruct itemSource)
    {
        _source = source;
        _itemSource = itemSource;
    }

    /// <summary>
    /// Index of the item on the assigned placeable's inventory.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _itemList!.IndexOf(_itemSource);
        }
    }

    /// <summary>
    /// Returns true if the item still exists in the placeable's inventory.
    /// </summary>
    public bool Exists
    {
        get
        {
            return _itemList is not null && _itemList.Contains(_itemSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>InventoryRes</c> field in the UTP.
    /// </remarks>
    public ResRef ResRef
    {
        get => _itemSource.GetResRef("InventoryRes") ?? "";
        set => _itemSource.SetResRef("InventoryRes", value);
    }

    /// <summary>
    /// Removes the item from the placeables's inventory.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _itemList!.Remove(_itemSource);

        _source.Root.GetList("ItemList")!.Skip(index).ToList().ForEach(x =>
        {
            x.ID = (uint)(index);
            x.SetUInt16("Repos_PosX", (ushort)index);
            index++;
        });
    }

    internal ushort Repos_PosX
    {
        get => _itemSource.GetUInt16("Repos_PosX") ?? 0;
        set => _itemSource.SetUInt16("Repos_PosX", value);
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
            throw new ArgumentException("This item no longer exists.");
    }
}
