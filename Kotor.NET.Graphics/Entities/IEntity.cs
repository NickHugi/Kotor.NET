using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Entities;

public interface IEntity
{
    public Matrix4x4 Transformation { get; }

    public void Render(RenderFrame frame, IAssetManager assetManager);

    public void Update(IAssetManager assetManager, float delta);
}
