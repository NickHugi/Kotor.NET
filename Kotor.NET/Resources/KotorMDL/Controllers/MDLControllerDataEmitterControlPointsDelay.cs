using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterControlPointsDelay : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterControlPointsDelay()
    {
    }
    public MDLControllerDataEmitterControlPointsDelay(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
