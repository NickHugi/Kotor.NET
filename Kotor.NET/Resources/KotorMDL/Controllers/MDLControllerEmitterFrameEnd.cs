using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterFrameEnd : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterFrameEnd()
    {
    }
    public MDLControllerEmitterFrameEnd(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
