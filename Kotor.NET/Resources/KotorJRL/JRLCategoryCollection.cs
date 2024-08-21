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

public class JRLCategoryCollection : IEnumerable<JRLCategory>
{
    private GFF _source;
    private GFFList? _categoryList => _source.Root.GetList("Categories");

    public JRLCategoryCollection(GFF source)
    {
        _source = source;
    }

    public JRLCategory this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _categoryList?.Count() ?? 0;
    public IEnumerator<JRLCategory> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new category to the journal.
    /// </summary>
    public JRLCategory Add(LocalisedString name, uint priority, string tag, string comment, int plotIndex, int planetID)
    {
        var categoryList = CreateListIfItDoesNotExist();
        var categoryStruct = categoryList.Add((uint)categoryList.Count);

        return new JRLCategory(_source, categoryStruct)
        {
            Name = name,
            Priority = priority,
            Tag = tag,
            Comment = comment,
            PlotIndex = plotIndex,
            PlanetID = planetID,
        };
    }

    /// <summary>
    /// Removes all categorys from the journal.
    /// </summary>
    public void Clear()
    {
        var categoryList = CreateListIfItDoesNotExist();
        categoryList.Clear();
    }

    private IEnumerable<JRLCategory> All()
    {
        return _categoryList?.Select(x => new JRLCategory(_source, x)) ?? new List<JRLCategory>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _categoryList ?? _source.Root.SetList("Categories");
    }
}
