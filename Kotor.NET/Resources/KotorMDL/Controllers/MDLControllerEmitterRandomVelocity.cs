using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterRandomVelocity : BaseMDLController
{
    public float Speed { get; set; }

    public MDLControllerEmitterRandomVelocity()
    {
    }
    public MDLControllerEmitterRandomVelocity(float speed)
    {
        Speed = speed;
    }

    public override string ToString()
    {
        return $"Speed={Speed}";
    }
}
