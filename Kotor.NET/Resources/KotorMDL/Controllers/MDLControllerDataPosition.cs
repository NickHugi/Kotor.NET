﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

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
    public MDLControllerDataPosition(Vector3 vector3)
    {
        X = vector3.X;
        Y = vector3.Y;
        Z = vector3.Z;
    }
    
    public override string ToString()
    {
        return $"X={X}, Y={Y}, Z={Z}";
    }
    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}
