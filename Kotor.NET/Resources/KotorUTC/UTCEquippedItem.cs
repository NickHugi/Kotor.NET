using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCEquippedItem
{
    private GFF _source { get; }
    private uint _structID { get; }
    private GFFList? _itemList => _source.Root.GetList("Equip_ItemList");

    internal UTCEquippedItem(GFF source, uint structID)
    {
        _source = source;
        _structID = structID;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>EquippedRes</c> field.
    /// </remarks>
    public ResRef ResRef
    {
        get => _itemSource.GetResRef("EquippedRes") ?? "";
        set => _itemSource.SetResRef("EquippedRes", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Dropable</c> field.
    /// </remarks>
    public bool Droppable
    {
        get => _itemSource.GetUInt8("Dropable") != 0;
        set => _itemSource.SetUInt8("Dropable", Convert.ToByte(value));
    }

    /// <summary>
    /// Removes the item from the creature.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _itemList!.Remove(_itemSource);
    }

    /// <summary>
    /// Returns true if the item still exists in the creature's equipped item list.
    /// </summary>
    public bool Exists()
    {
        return _itemList is not null && _itemList.OfStructID(_structID).Any();
    }

    internal GFFStruct _itemSource
    {
        get
        {
            var itemList = _itemList ?? _source.Root.SetList("Equip_ItemList");
            var itemStruct = itemList.OfStructID(_structID).FirstOrDefault() ?? itemList.Add(_structID);
            if (itemStruct.GetResRef("EquippedRes") is null)
                itemStruct.SetResRef("EquippedRes", "");
            return itemStruct;
        }
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists())
        {
            throw new ArgumentException("This item no longer exists.");
        }
    }
}
