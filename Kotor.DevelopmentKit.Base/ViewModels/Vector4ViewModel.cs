using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class Vector3ViewModel : ReactiveObject
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
}
