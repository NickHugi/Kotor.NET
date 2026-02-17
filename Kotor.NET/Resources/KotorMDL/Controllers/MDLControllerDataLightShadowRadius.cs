using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataLightShadowRadius : BaseMDLControllerData
{
    public float Radius { get; set; }

    public MDLControllerDataLightShadowRadius()
    {
    }
    public MDLControllerDataLightShadowRadius(float radius)
    {
        Radius = radius;
    }

    public override string ToString()
    {
        return $"Radius={Radius}";
    }
}
