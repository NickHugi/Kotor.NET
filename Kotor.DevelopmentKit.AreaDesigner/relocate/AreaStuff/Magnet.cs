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
    public MagnetType Type { get; set; }

    public Vector3 LocalPosition { get; init; }
    public Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(LocalPosition, Parent.GlobalOrientation) + Parent.GlobalPosition;
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * Parent.GlobalOrientation);
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public Magnet(IWorldObject obj)
    {
        Parent = obj;
    }
}
