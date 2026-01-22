using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.OpenGL.Model;

public class KModel : IModel
{
    public BaseNode Root { get; set; }
    public Matrix4x4 Transformation => Matrix4x4.Identity;

    public ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 transformation)
    {
        var objects = new List<IRenderObject>();
        var iterate = new List<BaseNode>() { Root };

        while (iterate.Any())
        {
            var target = iterate.First();
            iterate.RemoveAt(0);

            if (target.Visible)
            {
                iterate.AddRange(target.Nodes);
                objects.AddRange(target.Render(assetManager, transformation));
            }
        }

        return objects;
    }
}
