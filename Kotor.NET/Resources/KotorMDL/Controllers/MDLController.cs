using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public interface IMDLControllerModifiable<in TData>
    where TData : BaseMDLControllerData
{
    public void AddLinear(float time, TData data);
    public void AddBezier(float time, TData point0, TData point1, TData point2);
}

public interface IMDLControllerReadable<out TData> : IEnumerable<IMDLControllerRow<TData>>
    where TData : BaseMDLControllerData
{
    public IEnumerable<IMDLControllerRow<TData>> Rows { get; }
    public bool IsEmpty { get; }
    public bool? IsLinear { get; }
    public bool? IsBezier { get; }
}

public class MDLController<TData> : IMDLControllerReadable<TData>, IMDLControllerModifiable<TData>
    where TData : BaseMDLControllerData
{
    public IEnumerable<IMDLControllerRow<TData>> Rows => _allNodeRows.OfType<IMDLControllerRow<TData>>();
    public bool IsEmpty => !Rows.Any();
    public bool? IsLinear => Rows.Any() ? Rows.Any(row => row.IsLinear) : null;
    public bool? IsBezier => Rows.Any() ? Rows.Any(row => row.IsBezier) : null;

    private List<IMDLControllerRow<BaseMDLControllerData>> _allNodeRows { get; }

    internal MDLController(List<IMDLControllerRow<BaseMDLControllerData>> rows)
    {
        _allNodeRows = rows;
    }

    public IEnumerator<IMDLControllerRow<TData>> GetEnumerator() => Rows.ToList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Rows.ToList().GetEnumerator();

    public void AddLinear(float time, TData data)
    {
        if (IsBezier == true)
            throw new Exception("Controller cannot have a mix of bezier and linear transformations");

        _allNodeRows.Add(MDLControllerRow<TData>.CreateLinear(time, data));
    }

    public void AddBezier(float time, TData point0, TData point1, TData point2)
    {
        if (IsLinear == true)
            throw new Exception("Controller cannot have a mix of bezier and linear transformations");

        _allNodeRows.Add(MDLControllerRow<TData>.CreateBezier(time, point0, point1, point2));
    }
}

public class MDLController : IMDLControllerReadable<BaseMDLControllerData>
{
    public IEnumerable<IMDLControllerRow<BaseMDLControllerData>> Rows => _allNodeRows.Where(x => x.Data.First().GetType() == Type).ToList();
    public Type Type { get; }
    public bool IsEmpty => !Rows.Any();
    public bool? IsLinear => Rows.Any() ? Rows.Any(row => row.IsLinear) : null;
    public bool? IsBezier => Rows.Any() ? Rows.Any(row => row.IsBezier) : null;

    private List<IMDLControllerRow<BaseMDLControllerData>> _allNodeRows { get; }

    internal MDLController(Type type, List<IMDLControllerRow<BaseMDLControllerData>> rows)
    {
        Type = type;
        _allNodeRows = rows;
    }

    public IEnumerator<IMDLControllerRow<BaseMDLControllerData>> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
