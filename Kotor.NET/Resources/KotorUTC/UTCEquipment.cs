using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTC;

public class UTCEquipment
{
    private GFF _source;
    private GFFList? _itemList => _source.Root.GetList("Equip_ItemList");

    public UTCEquipment(GFF source)
    {
        _source = source;
    }

    public UTCEquippedItem HeadGear => new(_source, 1);
    public UTCEquippedItem Armor => new(_source, 2);
    public UTCEquippedItem Gauntlet => new(_source, 4);
    public UTCEquippedItem RightHand => new(_source, 16);
    public UTCEquippedItem LeftHand => new(_source, 32);
    public UTCEquippedItem RightArm => new(_source, 128);
    public UTCEquippedItem LeftArm => new(_source, 256);
    public UTCEquippedItem Implant => new(_source, 512);
    public UTCEquippedItem Belt => new(_source, 1024);
    public UTCEquippedItem Claw1 => new(_source, 16384);
    public UTCEquippedItem Claw2 => new(_source, 32768);
    public UTCEquippedItem Claw3 => new(_source, 65536);
    public UTCEquippedItem Hide => new(_source, 131072);

    public void Clear()
    {
        CreateListIfItDoesNotExist().Clear();
    }

    private GFFList CreateListIfItDoesNotExist()
    {
        return _itemList ?? _source.Root.SetList("Equip_ItemList");
    }
}
