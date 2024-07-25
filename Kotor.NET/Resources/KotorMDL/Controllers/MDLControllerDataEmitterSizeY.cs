using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterSizeY : BaseMDLControllerData
{
    public float SizeY { get; set; }

    public MDLControllerDataEmitterSizeY()
    {
    }
    public MDLControllerDataEmitterSizeY(float sizeY)
    {
        SizeY = sizeY;
    }

    public override string ToString()
    {
        return $"SizeY={SizeY}";
    }
}
