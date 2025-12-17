using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.GPU;

public interface IModel
{
    public Matrix4x4 Transformation { get; }

    void Render(IRenderFrame frame);
}
