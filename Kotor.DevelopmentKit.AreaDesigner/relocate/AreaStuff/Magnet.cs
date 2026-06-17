using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Magnet
{
    public IWorldObject Parent { get; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    {
        get => Matrix4x4.Decompose(GlobalTransform, out _, out _, out var value) ? value : new();
    }
    public Quaternion GlobalOrientation
    {
        get => Matrix4x4.Decompose(GlobalTransform, out _, out var value, out _) ? value : new();
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public MagnetType Type { get; set; }

    public Magnet(IWorldObject obj)
    {
        Parent = obj;
    }
}
