using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorJRL;

public class JRLEntry
{
    private GFFStruct _categorySource { get; }
    private GFFStruct _entrySource { get; }
    private GFFList? _entryList => _categorySource.GetList("EntryList");

    internal JRLEntry(GFFStruct categorySource, GFFStruct entrySource)
    {
        _categorySource = categorySource;
        _entrySource = entrySource;
    }

    /// <summary>
    /// Index of the entry on the assigned category.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _entryList!.IndexOf(_entrySource);
        }
    }

    /// <summary>
    /// Returns true if the entry still exists in the category.
    /// </summary>
    public bool Exists
    {
        get => _entryList is not null && _entryList.Contains(_entrySource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>ID</c> field in the JRL entry struct.
    /// </remarks>
    public uint ID
    {
        get => _entrySource.GetUInt32("ID") ?? 0;
        set => _entrySource.SetUInt32("ID", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>End</c> field in the JRL entry struct.
    /// </remarks>
    public bool End
    {
        get => _entrySource.GetUInt16("End") != 0;
        set => _entrySource.SetUInt16("End", Convert.ToUInt16(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Text</c> field in the JRL entry struct.
    /// </remarks>
    public LocalisedString Text
    {
        get => _entrySource.GetLocalisedString("Text") ?? new();
        set => _entrySource.SetLocalisedString("Text", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>XP_Percentage</c> field in the JRL entry struct.
    /// </remarks>
    public float XPPercentage
    {
        get => _entrySource.GetSingle("XP_Percentage") ?? 0.0f;
        set => _entrySource.SetSingle("XP_Percentage", value);
    }

    /// <summary>
    /// Removes the entry from the category.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _entryList!.Remove(_entrySource);

        _entryList!.Skip(index).ToList().ForEach(x =>
        {
            x.ID = (uint)(index);
            index++;
        });
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
            throw new ArgumentException("This entry no longer exists.");
    }
}
