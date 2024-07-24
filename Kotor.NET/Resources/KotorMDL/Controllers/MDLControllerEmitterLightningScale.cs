using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterLightningScale : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterLightningScale()
    {
    }
    public MDLControllerEmitterLightningScale(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
