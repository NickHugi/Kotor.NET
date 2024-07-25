using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterLightningSubDiv : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterLightningSubDiv()
    {
    }
    public MDLControllerDataEmitterLightningSubDiv(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
