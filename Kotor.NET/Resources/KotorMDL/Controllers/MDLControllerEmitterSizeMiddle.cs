using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterSizeMiddle : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterSizeMiddle()
    {
    }
    public MDLControllerEmitterSizeMiddle(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
