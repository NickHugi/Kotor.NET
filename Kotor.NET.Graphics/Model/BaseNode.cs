using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Extensions;
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
        var anim = Model.Animations.First(x => x.Name == animation);
        var animNode = anim.Root.FindNode(NodeID);

        var position = animNode?.GetAnimPosition(this, timeKey) ?? Position;
        var rotation = animNode?.GetAnimRotation(this, timeKey) ?? Orientation;
        LocalTransformation = Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        WorldTransformation = (Parent is null) ? LocalTransformation : LocalTransformation * Parent.WorldTransformation;

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
