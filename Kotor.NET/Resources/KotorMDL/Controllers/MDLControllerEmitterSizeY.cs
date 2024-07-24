using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterSizeY : BaseMDLController
{
    public float SizeY { get; set; }

    public MDLControllerEmitterSizeY()
    {
    }
    public MDLControllerEmitterSizeY(float sizeY)
    {
        SizeY = sizeY;
    }

    public override string ToString()
    {
        return $"SizeY={SizeY}";
    }
}
