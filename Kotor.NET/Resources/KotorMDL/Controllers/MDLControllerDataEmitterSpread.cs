using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterSpread : BaseMDLControllerData
{
    public float Spread { get; set; }

    public MDLControllerDataEmitterSpread()
    {
    }
    public MDLControllerDataEmitterSpread(float spread)
    {
        Spread = spread;
    }

    public override string ToString()
    {
        return $"Spread={Spread}";
    }
}
