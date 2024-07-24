using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Controllers;

public class MDLControllerPosition : BaseMDLController
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public MDLControllerPosition()
    {
    }
    public MDLControllerPosition(float x, float y, float z)
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
