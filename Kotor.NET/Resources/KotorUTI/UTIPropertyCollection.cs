using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTI;

public class UTIPropertyCollection : IEnumerable<UTIProperty>
{
    private GFF _source;
    private GFFList? _propertyList => _source.Root.GetList("PropertiesList");

    public UTIPropertyCollection(GFF source)
    {
        _source = source;
    }

    public UTIProperty this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _propertyList?.Count() ?? 0;
    public IEnumerator<UTIProperty> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new property to the item.
    /// </summary>
    public UTIProperty Add(byte costTable, ushort costValue, byte param1, byte param1Value, ushort propertyName, ushort subtype, byte upgradeType)
    {
        var propertyList = CreateListIfItDoesNotExist();
        var propertyStruct = propertyList.Add();

        return new UTIProperty(_source, propertyStruct)
        {
            ChanceAppear = 100,
            CostTable = costTable,
            CostValue = costValue,
            Param1 = param1,
            Param1Value = param1Value,
            PropertyName = propertyName,
            Subtype = subtype,
            UpgradeType = upgradeType,
        };
    }

    /// <summary>
    /// Removes all properties from the item.
    /// </summary>
    public void Clear()
    {
        var propertyList = CreateListIfItDoesNotExist();
        propertyList.Clear();
    }

    private IEnumerable<UTIProperty> All()
    {
        return _propertyList?.Select(x => new UTIProperty(_source, x)) ?? new List<UTIProperty>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _propertyList ?? _source.Root.SetList("PropertiesList");
    }
}
