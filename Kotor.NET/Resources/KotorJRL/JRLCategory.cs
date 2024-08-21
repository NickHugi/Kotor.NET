using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorJRL;

public class JRLCategory
{
    private GFF _source { get; }
    private GFFStruct _categorySource { get; }
    private GFFList? _categoryList => _source.Root.GetList("Categories");

    internal JRLCategory(GFF source, GFFStruct categorySource)
    {
        _source = source;
        _categorySource = categorySource;
    }

    /// <summary>
    /// Index of the category on the assigned journal.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _categoryList!.IndexOf(_categorySource);
        }
    }

    /// <summary>
    /// Returns true if the category still exists in the journal.
    /// </summary>
    public bool Exists
    {
        get => _categoryList is not null && _categoryList.Contains(_categorySource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Name</c> field in the JRL category struct.
    /// </remarks>
    public LocalisedString Name
    {
        get => _categorySource.GetLocalisedString("Name") ?? new();
        set => _categorySource.SetLocalisedString("Name", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Priority</c> field in the JRL category struct.
    /// </remarks>
    public uint Priority
    {
        get => _categorySource.GetUInt32("Priority") ?? 0;
        set => _categorySource.SetUInt32("Priority", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Comment</c> field in the JRL category struct.
    /// </remarks>
    public string Comment
    {
        get => _categorySource.GetString("Comment") ?? "";
        set => _categorySource.SetString("Comment", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Tag</c> field in the JRL category struct.
    /// </remarks>
    public string Tag
    {
        get => _categorySource.GetString("Tag") ?? "";
        set => _categorySource.SetString("Tag", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PlotIndex</c> field in the JRL category struct.
    /// </remarks>
    public int PlotIndex
    {
        get => _categorySource.GetInt32("PlotIndex") ?? 0;
        set => _categorySource.SetInt32("PlotIndex", value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>PlanetID</c> field in the JRL category struct.
    /// </remarks>
    public int PlanetID
    {
        get => _categorySource.GetInt32("PlanetID") ?? 0;
        set => _categorySource.SetInt32("PlanetID", value);
    }

    public JRLEntryCollection Entries => new(_categorySource);

    /// <summary>
    /// Removes the category from the journal.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _categoryList!.Remove(_categorySource);

        _categoryList!.Skip(index).ToList().ForEach(x =>
        {
            x.ID = (uint)(index);
            index++;
        });
    }

    private void RaiseIfDoesNotExist()
    {
        if (!Exists)
            throw new ArgumentException("This category no longer exists.");
    }
}
