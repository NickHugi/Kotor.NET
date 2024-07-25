using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataLightRadius : BaseMDLControllerData
{
    public float Radius { get; set; }

    public MDLControllerDataLightRadius()
    {
    }
    public MDLControllerDataLightRadius(float radius)
    {
        Radius = radius;
    }

    public override string ToString()
    {
        return $"Radius={Radius}";
    }
}
