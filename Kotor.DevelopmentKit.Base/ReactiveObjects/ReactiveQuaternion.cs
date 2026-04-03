using System.Numerics;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ReactiveObjects;

public class ReactiveQuaternion : ReactiveObject
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

    public float Z
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public float W
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveQuaternion()
    {
    }
    public ReactiveQuaternion(Quaternion quaternion)
    {
        X = quaternion.X;
        Y = quaternion.Y;
        Z = quaternion.Z;
        W = quaternion.W;
    }
    public ReactiveQuaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Quaternion ToModel()
    {
        return new Quaternion(X, Y, Z, W);
    }
}
