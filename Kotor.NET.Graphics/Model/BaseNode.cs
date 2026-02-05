using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics.Model;

public abstract class BaseNode
{
    public KModel Model { get; set; }
    public BaseNode Parent { get; init; }
    public readonly ICollection<BaseNode> Nodes = new List<BaseNode>();
    public ICollection<Controller> Controllers { get; set; }
    public int NodeID { get; set; }

    public Vector3 Position
    {
        get; set;
    }

    public Quaternion Orientation
    {
        get; set;
    }

    public Matrix4x4 OriginalWorldTransformation
    {
        get; private set;
    }
    public Matrix4x4 OriginalLocalTransform
    {
        get; private set;
    }

    public Matrix4x4 WorldTransformation
    {
        get; private set;
    }
    public Matrix4x4 LocalTransformation
    {
        get; private set;
    }

    public Matrix4x4 InverseBindMatrix
    {
        get; private set;
    }

    public bool Visible;

    public virtual ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform)
    {
        return [];
    }
    public virtual ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform, string animation, float timeKey)
    {
        return [];
    }

    public void GenerateTransform()
    {
        WorldTransformation = (Parent is null) ? (Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position)) : Parent.WorldTransformation;
        WorldTransformation = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position) * WorldTransformation;

        foreach (var node in Nodes)
        {
            node.GenerateTransform();
        }
    }
    public void GenerateTransform(string animation, float timeKey)
    {
        var test = Matrix4x4.Identity;
        var test2 = Matrix4x4.Identity;

        if (NodeID != 0 && NodeID != 6 && Model.Root.FindNode(6) is { } head && head.WorldTransformation.GetDeterminant() != 0)
        {
            //test = head.WorldTransformation;
            //Matrix4x4.Invert(test, out test2);

        }

        if (NodeID == 6)
        {
            //Position = new(0, 0, 0);
            //Orientation = Quaternion.Identity;
        }

        if (NodeID == 7)
        {

            //test = Matrix4x4.CreateTranslation(123, -120, 20);
            //test = Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(3, 3, 3));

            //var head = Parent.FindNode(6);
            //test = head.LocalTransformation;
            //Matrix4x4.Invert(test, out test2);

            //Orientation = Quaternion.Identity;
            //Position = new(0, 0, 1.2229f);
            //Orientation = Quaternion.Identity;
            //Orientation = Quaternion.Inverse(new(0.452152f, -0.54354f, -0.452152f, 0.543651f));
        }

        var anim = Model.Animations.First(x => x.Name == animation);
        var animNode = anim.Root.FindNode(NodeID);

        var position = animNode?.GetAnimPosition(this, timeKey) ?? Position;
        var rotation = animNode?.GetAnimRotation(this, timeKey) ?? Orientation;
        LocalTransformation = Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        WorldTransformation = (Parent is null) ? LocalTransformation : LocalTransformation * Parent.WorldTransformation;

        //if (NodeID == 7)
        //{
        //    LocalTransformation = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position + new Vector3(0f, 0.5f, 0f));
        //    WorldTransformation = (Parent is null) ? LocalTransformation : LocalTransformation * Parent.WorldTransformation;
        //}
        //else
        //{
        //    LocalTransformation = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);
        //    WorldTransformation = (Parent is null) ? LocalTransformation : LocalTransformation * Parent.WorldTransformation;
        //}

        OriginalLocalTransform = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);
        OriginalWorldTransformation = (Parent is null) ? OriginalLocalTransform : OriginalLocalTransform * Parent.OriginalWorldTransformation;

        InverseBindMatrix = Matrix4x4.Invert(OriginalWorldTransformation, out var value) ? value : Matrix4x4.Identity;

        foreach (var node in Nodes)
        {
            node.GenerateTransform(animation, timeKey);
        }
    }

    public BaseNode? FindNode(int nodeID)
    {
        List<BaseNode> nodes = [this];
        while (nodes.Any())
        {
            var target = nodes.First();
            nodes.AddRange(target.Nodes);
            nodes.Remove(target);

            if (target.NodeID == nodeID)
                return target;
        }

        return null;
    }

    public Vector3? GetAnimPosition(BaseNode original, float timeKey)
    {
        var controller = Controllers.FirstOrDefault(x => x.ControllerType == 8);

        if (controller is not null)
        {
            var p = new Vector3(controller.ControllerData.First().Values);
            return original.Position + p;
        }
        else
        {
            return null;
        }
    }

    public Quaternion? GetAnimRotation(BaseNode original, float timeKey)
    {
        var controller = Controllers.FirstOrDefault(x => x.ControllerType == 20);

        if (controller is not null)
        {
            if (controller.ControllerData.First().Values.Length == 4)
            {
                var q = new Quaternion(
                    controller.ControllerData.First().Values[0],
                    controller.ControllerData.First().Values[1],
                    controller.ControllerData.First().Values[2],
                    controller.ControllerData.First().Values[3]
                );
                return q;
            }
            else
            {
                var q = controller.ControllerData.First().Values[0].UncompressQuaternion();
                return q;
            }
        }
        else
        {
            return null;
        }
    }
}

public static class QuaternionExtensions
{
    public static Quaternion UncompressQuaternion(this float value)
    {
        var data = BitConverter.GetBytes(value);
        return UncompressQuaternion(data);
    }
    public static Quaternion UncompressQuaternion(this byte[] data)
    {
        var temp = BitConverter.ToInt32(data);
        var tmpQuat = new Quaternion();

        var QUAT_X_MASK = 0x07ff;        // 11 bits for X component
        var QUAT_Y_MASK = 0x07ff;        // 11 bits for Y component  
        var QUAT_Z_MASK = 0x3FF;         // 10 bits for Z component
        var QUAT_X_SCALE = 1.0 / 1023.0; // Scale factor for X,Y
        var QUAT_Z_SCALE = 1.0 / 511.0;  // Scale factor for Z
        var QUAT_Y_SHIFT = 11;           // Y component bit shift
        var QUAT_Z_SHIFT = 22;           // Z component bit shift

        var x = ((temp & QUAT_X_MASK) * QUAT_X_SCALE) - 1.0;
        var y = (((temp >> QUAT_Y_SHIFT) & QUAT_Y_MASK) * QUAT_X_SCALE) - 1.0;
        var z = (((temp >> QUAT_Z_SHIFT) & QUAT_Z_MASK) * QUAT_Z_SCALE) - 1.0;

        var fSquares = x * x + y * y + z * z;

        // Early exit for identity quaternion (all components near zero)
        if (fSquares < 1e-10)
        {
            tmpQuat =  new(0, 0, 0, 1);
        }
        else if (fSquares < 1.0)
        {
            tmpQuat = new((float)x, (float)y, (float)z, (float)Math.Sqrt(1.0 - fSquares));
        }
        else
        {
            // Normalize the vector to unit length instead of setting w=0
            var invLength = 1.0 / Math.Sqrt(fSquares);
            tmpQuat = new((float)(x * invLength), (float)(y * invLength), (float)(z * invLength), 0);
        }
        return Quaternion.Normalize(tmpQuat);
    }
}
