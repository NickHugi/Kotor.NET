using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerLightShadowRadius : BaseMDLController
{
    public float Radius { get; set; }

    public MDLControllerLightShadowRadius()
    {
    }
    public MDLControllerLightShadowRadius(float radius)
    {
        Radius = radius;
    }

    public override string ToString()
    {
        return $"Radius={Radius}";
    }
}
