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

    public ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 transformation);
}
