using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataOrientation : BaseMDLControllerData
{
    public float W { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public MDLControllerDataOrientation()
    {
    }
    public MDLControllerDataOrientation(float w, float x, float y, float z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return $"X={X}, Y={Y}, Z={Z}, W={W}";
    }
}
