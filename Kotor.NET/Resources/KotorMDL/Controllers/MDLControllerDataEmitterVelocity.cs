using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterVelocity : BaseMDLControllerData
{
    public float Speed { get; set; }

    public MDLControllerDataEmitterVelocity()
    {
    }
    public MDLControllerDataEmitterVelocity(float speed)
    {
        Speed = speed;
    }

    public override string ToString()
    {
        return $"Speed={Speed}";
    }
}
