using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public interface IEntity
{
    public Matrix4x4 Transformation { get; }

    public void Render(IRenderFrame frame);
    public void Update(float delta);
}
