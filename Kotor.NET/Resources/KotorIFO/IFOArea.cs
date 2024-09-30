using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorIFO;

public class IFOArea
{
    private GFF _source { get; }
    private GFFStruct _areaSource { get; }
    private GFFList? _areaList => _source.Root.GetList("Mod_Area_list");

    internal IFOArea(GFF source, GFFStruct areaSource)
    {
        _source = source;
        _areaSource = areaSource;
    }

    /// <summary>
    /// Index of the area on the area in the list.
    /// </summary>
    public int Index
    {
        get
        {
            RaiseIfDoesNotExist();
            return _areaList!.IndexOf(_areaSource);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is the value stored in the <c>Area_Name</c> field in the IFO.
    /// </remarks>
    public ResRef ResRef
    {
        get => _areaSource.GetResRef("Area_Name") ?? "";
        set => _areaSource.SetResRef("Area_Name", value);
    }

    /// <summary>
    /// Removes the area.
    /// </summary>
    public void Remove()
    {
        var index = Index;

        RaiseIfDoesNotExist();
        _areaList!.Remove(_areaSource);

        _source.Root.GetList("Mod_Area_list")!.Skip(index).ToList().ForEach(x =>
        {
            x.ID = (uint)(index);
            x.SetUInt16("Repos_PosX", (ushort)index);
            index--;
        });
    }

    /// <summary>
    /// Returns true if the area still exists.
    /// </summary>
    public bool Exists()
    {
        var classList = _areaList;
        return classList is null || !classList.Contains(_areaSource);
    }

    private void RaiseIfDoesNotExist()
    {
        if (Exists())
        {
            throw new ArgumentException("This area no longer exists.");
        }
    }
}
