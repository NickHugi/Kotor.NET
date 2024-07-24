using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerLightMultiplier : BaseMDLController
{
    public float Multiplier { get; set; }

    public MDLControllerLightMultiplier()
    {
    }
    public MDLControllerLightMultiplier(float multiplier)
    {
        Multiplier = multiplier;
    }

    public override string ToString()
    {
        return $"Multiplier={Multiplier}";
    }
}
