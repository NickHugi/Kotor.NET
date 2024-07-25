using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public abstract class BaseMDLControllerRow<TData>(float startTime) where TData : BaseMDLControllerData
{
    public float StartTime { get; set; } = startTime;
}

//public abstract class BaseMDLControllerRow(float startTime) : BaseMDLControllerRow<BaseMDLControllerData>(startTime) { }
