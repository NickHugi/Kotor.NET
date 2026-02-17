using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataEmitterSizeX : BaseMDLControllerData
{
    public float SizeX { get; set; }

    public MDLControllerDataEmitterSizeX()
    {
    }
    public MDLControllerDataEmitterSizeX(float sizeX)
    {
        SizeX = sizeX;
    }

    public override string ToString()
    {
        return $"SizeX={SizeX}";
    }
}
