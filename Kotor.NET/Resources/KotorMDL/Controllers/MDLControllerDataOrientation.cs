﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

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
    public MDLControllerDataOrientation(Vector4 vector4)
    {
        X = vector4.X;
        Y = vector4.Y;
        Z = vector4.Z;
        W = vector4.W;
    }

    public override string ToString()
    {
        return $"X={X}, Y={Y}, Z={Z}, W={W}";
    }
    public Vector4 ToVector4()
    {
        return new Vector4(X, Y, Z, W);
    }
}
