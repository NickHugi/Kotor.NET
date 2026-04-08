using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;

namespace Kotor.NET.Graphics.Renderers;

public interface IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height);
}
