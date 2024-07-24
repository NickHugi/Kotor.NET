using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterSpread : BaseMDLController
{
    public float Spread { get; set; }

    public MDLControllerEmitterSpread()
    {
    }
    public MDLControllerEmitterSpread(float spread)
    {
        Spread = spread;
    }

    public override string ToString()
    {
        return $"Spread={Spread}";
    }
}
