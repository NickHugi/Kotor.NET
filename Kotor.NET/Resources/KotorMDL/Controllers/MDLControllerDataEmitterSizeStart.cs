using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterSizeStart : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterSizeStart()
    {
    }
    public MDLControllerDataEmitterSizeStart(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
