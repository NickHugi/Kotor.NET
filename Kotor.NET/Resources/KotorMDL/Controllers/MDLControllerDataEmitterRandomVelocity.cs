using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterRandomVelocity : BaseMDLControllerData
{
    public float Speed { get; set; }

    public MDLControllerDataEmitterRandomVelocity()
    {
    }
    public MDLControllerDataEmitterRandomVelocity(float speed)
    {
        Speed = speed;
    }

    public override string ToString()
    {
        return $"Speed={Speed}";
    }
}
