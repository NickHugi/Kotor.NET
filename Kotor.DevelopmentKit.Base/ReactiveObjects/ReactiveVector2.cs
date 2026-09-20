using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ReactiveObjects;

public class ReactiveVector : ReactiveObject
{
    public float X
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public float Y
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public Vector2 AsModel()
    {
        return new(X, Y);
    }
    public Vector4ViewModel Clone()
    {
        return new() { X = X, Y = Y };
    }
}
