using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using Kotor.NET.Resources.KotorUTE;

namespace Kotor.NET.Resources.KotorJRL;

public class JRLEntryCollection : IEnumerable<JRLEntry>
{
    private GFFStruct _categoryStruct;
    private GFFList? _entryList => _categoryStruct.GetList("EntryList");

    public JRLEntryCollection(GFFStruct categorySource)
    {
        _categoryStruct = categorySource;
    }

    public JRLEntry this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _entryList?.Count() ?? 0;
    public IEnumerator<JRLEntry> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new entry to the category.
    /// </summary>
    public JRLEntry Add(uint id, bool end, LocalisedString text, float xpPercentage)
    {
        var entryList = CreateListIfItDoesNotExist();
        var entryStruct = entryList.Add((uint)entryList.Count);

        return new JRLEntry(_categoryStruct, entryStruct)
        {
            ID = id,
            End = end,
            Text = text,
            XPPercentage = xpPercentage,
        };
    }

    /// <summary>
    /// Removes all entrys from the category.
    /// </summary>
    public void Clear()
    {
        var entryList = CreateListIfItDoesNotExist();
        entryList.Clear();
    }

    private IEnumerable<JRLEntry> All()
    {
        return _entryList?.Select(x => new JRLEntry(_categoryStruct, x)) ?? new List<JRLEntry>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _entryList ?? _categoryStruct.SetList("EntryList");
    }
}
