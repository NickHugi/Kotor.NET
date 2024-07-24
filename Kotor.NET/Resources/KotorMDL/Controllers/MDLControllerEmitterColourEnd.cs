using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerEmitterColourEnd : BaseMDLController
{
    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }

    public MDLControllerEmitterColourEnd()
    {
    }
    public MDLControllerEmitterColourEnd(float red, float green, float blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public override string ToString()
    {
        return $"R={Red} G={Green} B={Blue}";
    }
}
