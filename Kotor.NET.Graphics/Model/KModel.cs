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
    public ICollection<Controller> Controllers { get; set; }
    public ICollection<Animation> Animations { get; set; }

    public ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform)
    {
        var nodes = GetAllNodes();
        var objects = new List<IRenderObject>();

        Root.GenerateTransform();

        foreach (var node in nodes)
        {
            objects.AddRange(node.Render(assetManager, entityTransform));
        }

        return objects;
    }
    public ICollection<IRenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform, string animation, float timeKey)
    {
        var nodes = GetAllNodes();
        var objects = new List<IRenderObject>();

        Root.GenerateTransform(animation, timeKey);

        foreach (var node in nodes)
        {
            objects.AddRange(node.Render(assetManager, entityTransform, animation, timeKey));
        }

        return objects;
    }

    private IEnumerable<BaseNode> GetAllNodes()
    {
        var nodes = new List<BaseNode>();
        var iterate = new List<BaseNode>() { Root };

        while (iterate.Any())
        {
            var target = iterate.First();
            iterate.RemoveAt(0);

            iterate.AddRange(target.Nodes);
            yield return target;
        }
    }
}
