using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.GPU;

public interface IModel
{
    public ICollection<Animation> Animations { get; set; }
    public ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 transformation);
    ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform, string animation, float timeKey);
}
