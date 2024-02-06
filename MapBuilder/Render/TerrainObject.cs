using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.SceneObjects;
using MapBuilder.Data;
using Silk.NET.OpenGLES;

namespace MapBuilder.Render
{
    public class TerrainObject : SceneObject
    {
        private Terrain _terrain;
        private uint _id;
        private Graphics _graphics;

        public TerrainObject(Graphics graphics, Terrain terrain)
        {
            _terrain = terrain;
            _graphics = graphics;
            Gen();
        }

        private void Gen()
        {
            List<short> elements = new();
            List<Vector3> vertices = new();
            List<Vector2> uvs = new();

            for (var x = 0; x < _terrain.Width-1; x++)
                for (var y = 0; y < _terrain.Length-1; y++)
                {
                    var e = (short)elements.Count();

                    vertices.Add(new(x + 0, y + 0, 0));
                    vertices.Add(new(x + 1, y + 0, 0));
                    vertices.Add(new(x + 1, y + 1, 0));
                    vertices.Add(new(x + 0, y + 0, 0));
                    vertices.Add(new(x + 1, y + 1, 0));
                    vertices.Add(new(x + 0, y + 1, 0));

                    uvs.Add(new(0, 0));
                    uvs.Add(new(1, 0));
                    uvs.Add(new(1, 1));

                    uvs.Add(new(0, 0));
                    uvs.Add(new(1, 1));
                    uvs.Add(new(1, 0));

                    elements.AddRange(new short[] { (short)(e + 0), (short)(e + 1), (short)(e + 2) });
                    elements.AddRange(new short[] { (short)(e + 3), (short)(e + 4), (short)(e + 5) });
                }

            _graphics.SetVAO("terrain", new Kotor.NET.Graphics.VertexArray(_graphics.GL, vertices, elements, uv1s: uvs));
        }

        public override List<IRenderable> GetRenderables(Graphics graphics)
        {
            var vao = graphics.GetVAO("terrain");
            var shader = graphics.GetShader("kotor");
            var texture1 = graphics.GetTextures("lda_grass01");

            return new() { new Renderable(vao, shader, texture1, null) };
        }
    }
}
