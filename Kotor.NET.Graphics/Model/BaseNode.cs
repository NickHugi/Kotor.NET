using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Model;

public abstract class BaseNode
{
    public readonly ICollection<BaseNode> Nodes = new List<BaseNode>();
    public Matrix4x4 Transformation { get; set; }
}
