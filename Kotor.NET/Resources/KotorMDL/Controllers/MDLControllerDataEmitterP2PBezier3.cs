using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterP2PBezier3 : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerDataEmitterP2PBezier3()
    {
    }
    public MDLControllerDataEmitterP2PBezier3(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
