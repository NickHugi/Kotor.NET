using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterP2PBezier3 : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterP2PBezier3()
    {
    }
    public MDLControllerEmitterP2PBezier3(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
