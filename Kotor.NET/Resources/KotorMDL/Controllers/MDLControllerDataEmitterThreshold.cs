using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterThreshold : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterThreshold()
    {
    }
    public MDLControllerDataEmitterThreshold(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
