using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterCombineTime : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterCombineTime()
    {
    }
    public MDLControllerDataEmitterCombineTime(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
