using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTM;

public class UTMItem
{
    private GFF _source { get; }
    private GFFStruct _itemSource { get; }
    private GFFList? _itemList => _source.Root.GetList("ItemList");

    internal UTMItem(GFF source, GFFStruct itemSource)
    {
        _source = source;
        _itemSource = itemSource;
    }

    /// <summary>
    /// Index of the item on the assigned store's inventory.
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
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>InventoryRes</c> field in the UTM.
    /// </remarks>
    public ResRef ResRef
    {
        get => _itemSource.GetResRef("InventoryRes") ?? "";
        set => _itemSource.SetResRef("InventoryRes", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Infinite</c> field.
    /// </remarks>
    public bool Infinite
    {
        get => (_itemSource.GetUInt8("Infinite") ?? 0) != 0;
        set => _itemSource.SetUInt8("Infinite", Convert.ToByte(value));
    }

    /// <summary>
    /// Removes the item from the store's inventory.
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
            index--;
        });
    }

    /// <summary>
    /// Returns true if the item still exists in the store's inventory.
    /// </summary>
    public bool Exists()
    {
        var classList = _itemList;
        return classList is null || !classList.Contains(_itemSource);
    }

    internal ushort Repos_PosX
    {
        get => _itemSource.GetUInt16("Repos_PosX") ?? 0;
        set => _itemSource.SetUInt16("Repos_PosX", value);
    }

    private void RaiseIfDoesNotExist()
    {
        if (Exists())
        {
            throw new ArgumentException("This item no longer exists.");
        }
    }
}
