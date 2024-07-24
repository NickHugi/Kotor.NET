using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerLightVerticalDisplacement : BaseMDLController
{
    public float Displacement { get; set; }

    public MDLControllerLightVerticalDisplacement()
    {
    }
    public MDLControllerLightVerticalDisplacement(float displacement)
    {
        Displacement = displacement;
    }

    public override string ToString()
    {
        return $"Displacement={Displacement}";
    }
}
