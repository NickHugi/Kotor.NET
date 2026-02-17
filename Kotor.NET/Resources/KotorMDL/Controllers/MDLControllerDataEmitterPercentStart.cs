using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterPercentStart : BaseMDLControllerData
{
    public float Value { get; set; }

    public MDLControllerEmitterPercentStart()
    {
    }
    public MDLControllerEmitterPercentStart(float value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Value={Value}";
    }
}
