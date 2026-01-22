using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public class StaticModel : IEntity
{
    public IModel Model { get; set; }
    public Matrix4x4 Transformation { get; set; }

    public void Render(IRenderFrame frame)
    {
        Model.Render(frame, Transformation);
        
    }

    public void Update(float delta)
    {

    }
}
