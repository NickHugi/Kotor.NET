using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerRow<TData> : BaseMDLControllerRow<TData> where TData : BaseMDLControllerData
{
    public BaseMDLControllerData Data { get; set; }

    public MDLControllerRow(float startTime, BaseMDLControllerData data) : base(startTime)
    {
        StartTime = startTime;
        Data = data;
    }
}
