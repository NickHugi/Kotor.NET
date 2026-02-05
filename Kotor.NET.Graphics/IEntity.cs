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

    public void Render(IRenderFrame frame, IAssetManager assetManager);
    void Render(IRenderFrame frame, IAssetManager assetManager, string animation, float timeKey);
    public void Update(float delta);
}
