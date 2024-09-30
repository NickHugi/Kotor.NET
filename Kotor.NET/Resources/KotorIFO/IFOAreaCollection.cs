using System.Collections;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorIFO;

public class IFOAreaCollection : IEnumerable<IFOArea>
{
    private GFF _source;
    private GFFList? _areaList => _source.Root.GetList("Mod_Area_list");

    public IFOAreaCollection(GFF source)
    {
        _source = source;
    }

    public IFOArea this[int index]
    {
        get => All().ElementAt(index);
    }

    public int Count => _areaList?.Count() ?? 0;
    public IEnumerator<IFOArea> GetEnumerator() => All().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => All().GetEnumerator();

    /// <summary>
    /// Adds a new area.
    /// </summary>
    public IFOArea Add(ResRef resref)
    {
        var areaList = CreateListIfItDoesNotExist();
        var areaStruct = areaList.Add(structID: 6);

        return new IFOArea(_source, areaStruct)
        {
            ResRef = resref,
        };
    }

    /// <summary>
    /// Removes all areas.
    /// </summary>
    public void Clear()
    {
        var areaList = CreateListIfItDoesNotExist();
        areaList.Clear();
    }

    private IEnumerable<IFOArea> All()
    {
        return _areaList?.Select(x => new IFOArea(_source, x)) ?? new List<IFOArea>();
    }
    private GFFList CreateListIfItDoesNotExist()
    {
        return _areaList ?? _source.Root.SetList("Mod_Area_list");
    }
}
