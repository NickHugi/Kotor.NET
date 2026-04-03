using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ReactiveObjects;

public class ReactiveVector3 : ReactiveObject
{
    private float _x;
    public float X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    private float _y;
    public float Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }

    private float _z;
    public float Z
    {
        get => _z;
        set => this.RaiseAndSetIfChanged(ref _z, value);
    }

    public ReactiveVector3()
    {
    }
    public ReactiveVector3(Vector3 vector)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }
    public ReactiveVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 AsModel()
    {
        return new(X, Y, Z);
    }
    public Vector3ViewModel Clone()
    {
        return new() { X = _x, Y = _y, Z = _z };
    }

    public Vector3 ToModel()
    {
        return new(X, Y, Z);
    }
}

public class Vector3ViewModel : ReactiveVector3
{
}
