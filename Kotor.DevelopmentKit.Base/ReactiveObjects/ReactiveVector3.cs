using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ReactiveObjects;

public class Vector4ViewModel : ReactiveObject
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

    private float _w;
    public float W
    {
        get => _w;
        set => this.RaiseAndSetIfChanged(ref _w, value);
    }

    public Vector4 AsModel()
    {
        return new(X, Y, Z, W);
    }
    public Vector4ViewModel Clone()
    {
        return new() { X = _x, Y = _y, Z = _z, W = _w };
    }
}
