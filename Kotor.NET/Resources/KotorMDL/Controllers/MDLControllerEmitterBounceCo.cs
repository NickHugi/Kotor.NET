using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterBounceCo : BaseMDLController
{
    public float Value { get; set; }

    public MDLControllerEmitterBounceCo()
    {
    }
    public MDLControllerEmitterBounceCo(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
