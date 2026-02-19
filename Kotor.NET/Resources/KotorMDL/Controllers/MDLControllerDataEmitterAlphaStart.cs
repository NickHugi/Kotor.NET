using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterAlphaStart : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterAlphaStart()
    {
    }
    public MDLControllerDataEmitterAlphaStart(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
