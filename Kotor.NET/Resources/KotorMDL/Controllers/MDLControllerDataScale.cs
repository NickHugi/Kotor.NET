using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataScale : BaseMDLControllerData
{
    public float Scale { get; set; }

    public MDLControllerDataScale()
    {
    }
    public MDLControllerDataScale(float scale)
    {
        Scale = scale;
    }

    public override string ToString()
    {
        return $"Scale={Scale}";
    }
}
