using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterAlphaMiddle : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterAlphaMiddle()
    {
    }
    public MDLControllerDataEmitterAlphaMiddle(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
