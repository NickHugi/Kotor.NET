using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataLightVerticalDisplacement : BaseMDLControllerData
{
    public float Displacement { get; set; }

    public MDLControllerDataLightVerticalDisplacement()
    {
    }
    public MDLControllerDataLightVerticalDisplacement(float displacement)
    {
        Displacement = displacement;
    }

    public override string ToString()
    {
        return $"Displacement={Displacement}";
    }
}
