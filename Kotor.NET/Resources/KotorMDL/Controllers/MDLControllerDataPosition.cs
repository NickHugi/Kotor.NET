using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerDataPosition : BaseMDLControllerData
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public MDLControllerDataPosition()
    {
    }
    public MDLControllerDataPosition(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public override string ToString()
    {
        return $"X={X}, Y={Y}, Z={Z}";
    }
}
