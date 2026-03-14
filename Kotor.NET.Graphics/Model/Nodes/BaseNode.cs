using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Extensions;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics.Model.Nodes;

public abstract class BaseNode : IDisposable
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

    public virtual ICollection<MeshDescriptor> GetMeshDescriptors(BaseEntity entity)
    {
        return [];
    }

    public void GenerateTransform(ICollection<ActiveAnimation> animations)
    {
        var blendSum = animations.Sum(x => x.BlendFactor);
        var transforms = new List<Matrix4x4>();

        foreach (var live in animations)
        {
            var animation = Model.Animations.First(x => x.Name == live.Name);
            var animNode = animation?.Root.FindNode(NodeID);
            var position = animNode?.GetAnimPosition(this, live.CurrentTime) ?? Position;
            var rotation = animNode?.GetAnimRotation(this, live.CurrentTime) ?? Orientation;
            var transform = Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
            transforms.Add(transform);
        }

        Matrix4x4 blend = transforms.FirstOrDefault();
        for (int i = 0; i < transforms.Count - 1; i++)
        {
            blend = Matrix4x4.Lerp(blend, transforms[i + 1], animations.ElementAt(i + 1).BlendFactor / blendSum);
        }

        OriginalLocalTransform = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);
        OriginalWorldTransformation = Parent is null ? OriginalLocalTransform : OriginalLocalTransform * Parent.OriginalWorldTransformation;

        LocalTransformation = transforms.Any() ? blend : OriginalLocalTransform;
        WorldTransformation = Parent is null ? LocalTransformation : LocalTransformation * Parent.WorldTransformation;

        InverseBindMatrix = Matrix4x4.Invert(OriginalWorldTransformation, out var value) ? value : Matrix4x4.Identity;

        foreach (var node in Nodes)
        {
            node.GenerateTransform(animations);
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

        if (controller is not null && controller.Data.Count > 0)
        {
            var rowA = controller.Data.OrderByDescending(x => x.TimeKey).FirstOrDefault(x => x.TimeKey < timeKey) ?? controller.Data.Last();
            var rowB = controller.Data.FirstOrDefault(x => x.TimeKey > rowA.TimeKey);

            if (rowA is null)
            {
                return null;
            }
            else
            {
                var positionA = new Vector3(rowA.Values);

                if (rowB is null)
                {
                    return original.Position + positionA;
                }
                else
                {
                    var positionB = new Vector3(rowB.Values);
                    var timeSinceA = timeKey - rowA.TimeKey;
                    var durationBetweenRows = rowB.TimeKey - rowA.TimeKey;
                    var weight = timeSinceA / durationBetweenRows;
                    return original.Position + Vector3.Lerp(positionA, positionB, weight);
                }
            }
        }
        else
        {
            return null;
        }
    }

    public Quaternion? GetAnimRotation(BaseNode original, float timeKey)
    {
        var controller = Controllers.FirstOrDefault(x => x.ControllerType == 20);

        if (controller is not null && controller.Data.Count > 0)
        {
            var rowA = controller.Data.OrderByDescending(x => x.TimeKey).FirstOrDefault(x => x.TimeKey < timeKey) ?? controller.Data.Last();
            var rowB = controller.Data.FirstOrDefault(x => x.TimeKey > rowA.TimeKey);

            if (rowA is null)
            {
                return null;
            }
            else
            {
                var rotationA = GetQuaternion(rowA);

                if (rowB is null)
                {
                    return rotationA;
                }
                else
                {
                    var rotationB = GetQuaternion(rowB);
                    var timeSinceA = timeKey - rowA.TimeKey;
                    var durationBetweenRows = rowB.TimeKey - rowA.TimeKey;
                    var weight = timeSinceA / durationBetweenRows;
                    return Quaternion.Lerp(rotationA, rotationB, weight);
                }
            }
        }
        else
        {
            return null;
        }
    }
    private Quaternion GetQuaternion(ControllerDataRow row)
    {
        if (row.Values.Length == 4)
        {
            var q = new Quaternion(
                row.Values[0],
                row.Values[1],
                row.Values[2],
                row.Values[3]
            );
            return q;
        }
        else
        {
            var q = row.Values[0].UncompressQuaternion();
            return q;
        }
    }

    public virtual void Dispose()
    {

    }
}
