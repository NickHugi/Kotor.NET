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

    public void Render(IRenderFrame frame, Matrix4x4 transformation)
    {
        var iterate = new List<BaseNode>() { Root };

        while (iterate.Any())
        {
            var target = iterate.First();
            iterate.RemoveAt(0);

            if (target.Visible)
            {
                iterate.AddRange(target.Nodes);

                target.Render(frame, transformation);
            }
        }
    }
}
