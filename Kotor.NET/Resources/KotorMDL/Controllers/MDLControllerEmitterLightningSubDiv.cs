using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterLightningSubDiv : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterLightningSubDiv()
    {
    }
    public MDLControllerEmitterLightningSubDiv(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
