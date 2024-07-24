using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerScale : BaseMDLController
{
    public float Scale { get; set; }

    public MDLControllerScale()
    {
    }
    public MDLControllerScale(float scale)
    {
        Scale = scale;
    }

    public override string ToString()
    {
        return $"Scale={Scale}";
    }
}
