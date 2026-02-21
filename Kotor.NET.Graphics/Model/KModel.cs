using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.Model.Nodes;

namespace Kotor.NET.Graphics.OpenGL.Model;

public class KModel : IDisposable
{
    public BaseNode Root { get; set; }
    public ICollection<Animation> Animations { get; set; }

    public ICollection<RenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform, ICollection<AnimationItem> animations)
    {
        var nodes = GetAllNodes();
        var objects = new List<RenderObject>();

        Root.GenerateTransform(animations);

        foreach (var node in nodes)
        {
            objects.AddRange(node.Render(assetManager, entityTransform));
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

    public ICollection<string> GetAllTextures()
    {
        var nodes = GetAllNodes();
        var textures = nodes.OfType<MeshNode>().Select(x => x.Texture1).Distinct().ToList();
        var lightmaps = nodes.OfType<MeshNode>().Select(x => x.Texture2).Distinct().ToList();

        return textures.Concat(lightmaps)
            .Where(x => !string.IsNullOrEmpty(x))
            .Where(x => x.ToUpper() != "NULL")
            .ToList();
    }

    public void Dispose()
    {
        GetAllNodes().ToList().ForEach(x => x.Dispose());
    }
}
