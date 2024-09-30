using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorUTI;

public class UTIProperty
{
    private GFF _source { get; }
    private GFFStruct _propertySource { get; }
    private GFFList? _propertyList => _source.Root.GetList("PropertiesList");

    internal UTIProperty(GFF source, GFFStruct propertySource)
    {
        _source = source;
        _propertySource = propertySource;
    }

    /// <summary>
    /// Index of the property on the assigned item.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _propertyList!.IndexOf(_propertySource);
        }
    }

    /// <summary>
    /// Returns true if the property still exists in the item.
    /// </summary>
    public bool Exists
    {
        get
        {
            return _propertyList is not null && _propertyList.Contains(_propertySource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ChanceAppear</c> field in the UTI.
    /// </remarks>
    public byte ChanceAppear
    {
        get => _propertySource.GetUInt8("ChanceAppear") ?? 0;
        set => _propertySource.SetUInt8("ChanceAppear", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CostTable</c> field in the UTI.
    /// </remarks>
    public byte CostTable
    {
        get => _propertySource.GetUInt8("CostTable") ?? 0;
        set => _propertySource.SetUInt8("CostTable", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>CostValue</c> field in the UTI.
    /// </remarks>
    public ushort CostValue
    {
        get => _propertySource.GetUInt16("CostValue") ?? 0;
        set => _propertySource.SetUInt16("CostValue", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Param1</c> field in the UTI.
    /// </remarks>
    public byte Param1
    {
        get => _propertySource.GetUInt8("Param1") ?? 0;
        set => _propertySource.SetUInt8("Param1", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Param1Value</c> field in the UTI.
    /// </remarks>
    public byte Param1Value
    {
        get => _propertySource.GetUInt8("Param1Value") ?? 0;
        set => _propertySource.SetUInt8("Param1Value", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PropertyName</c> field in the UTI.
    /// </remarks>
    public ushort PropertyName
    {
        get => _propertySource.GetUInt16("PropertyName") ?? 0;
        set => _propertySource.SetUInt16("PropertyName", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Subtype</c> field in the UTI.
    /// </remarks>
    public ushort Subtype
    {
        get => _propertySource.GetUInt16("Subtype") ?? 0;
        set => _propertySource.SetUInt16("Subtype", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>UpgradeType</c> field in the UTI.
    /// </remarks>
    public byte UpgradeType
    {
        get => _propertySource.GetUInt8("UpgradeType") ?? 0;
        set => _propertySource.SetUInt8("UpgradeType", value);
    }

    /// <summary>
    /// Removes the property from the item.
    /// </summary>
    public void Remove()
    {
        RaiseIfDoesNotExist();
        _propertyList!.Remove(_propertySource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
            throw new ArgumentException("This property no longer exists.");
    }
}
