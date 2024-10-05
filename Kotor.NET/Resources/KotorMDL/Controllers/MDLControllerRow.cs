using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public interface IMDLControllerRow<out TData>
    where TData : BaseMDLControllerData
{
    public float StartTime { get; set; }
    public TData[] Data { get; }

    public bool IsLinear { get; }
    public bool IsBezier { get; }
}

public class MDLControllerRow<TData> : IMDLControllerRow<TData>
    where TData : BaseMDLControllerData
{
    public float StartTime { get; set; }
    public TData[] Data { get; }

    public bool IsLinear => Data.Length == 1;
    public bool IsBezier => Data.Length == 3;

    internal MDLControllerRow(float startTime, TData[] data)
    {
        StartTime = startTime;
        Data = data;
    }
    public static MDLControllerRow<TData> CreateLinear(float startTime, TData data)
    {
        return new(startTime, [data]);
    }
    public static MDLControllerRow<TData> CreateBezier(float startTime, TData point0, TData point1, TData point2)
    {
        return new (startTime, [point0, point1, point2]);
    }
}
