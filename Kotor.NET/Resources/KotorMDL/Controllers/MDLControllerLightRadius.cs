using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerLightRadius : BaseMDLController
{
    public float Radius { get; set; }

    public MDLControllerLightRadius()
    {
    }
    public MDLControllerLightRadius(float radius)
    {
        Radius = radius;
    }

    public override string ToString()
    {
        return $"Radius={Radius}";
    }
}
