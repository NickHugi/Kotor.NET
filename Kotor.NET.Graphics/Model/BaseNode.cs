using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Model;

public abstract class BaseNode
{
    public BaseNode Parent { get; init; }
    public readonly ICollection<BaseNode> Nodes = new List<BaseNode>();

    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set
        {
            ResetTransformation();
            _position = value;
        }
    }

    private Quaternion _orientation;
    public Quaternion Orientation
    {
        get => _orientation;
        set
        {
            ResetTransformation();
            _orientation = value;
        }
    }

    private Matrix4x4? _transformation;
    public Matrix4x4 Transformation
    {
        get
        {
            if (_transformation is null)
            {
                _transformation = (Parent is null) ? Matrix4x4.Identity : Parent.Transformation;

                _transformation = Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position) * _transformation;
                //_transformation = _transformation * Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateFromQuaternion(Orientation);
            }

            return _transformation.Value;
        }
    }

    public virtual void Render(IRenderFrame renderFrame)
    {
    }

    private void ResetTransformation()
    {
        _transformation = null;
        foreach (var node in Nodes)
        {
            node.ResetTransformation();
        }
    }
}
