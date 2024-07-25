using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerRowBezier<TData> : BaseMDLControllerRow<TData> where TData : BaseMDLControllerData
{
    public BaseMDLControllerData[] Data { get; }

    public MDLControllerRowBezier(float startTime, BaseMDLControllerData data1, BaseMDLControllerData data2, BaseMDLControllerData data3) : base(startTime)
    {
        Data = new[] { data1, data2, data3 };
    }
}
