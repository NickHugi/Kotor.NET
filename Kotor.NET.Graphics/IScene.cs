using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics;

public interface IScene
{
    public Camera ActiveCamera { get; }

    public void Update(IAssetManager assets, float timestep);
    public IEnumerable<IDrawCallDescriptor> Render(IAssetManager assets);
}
