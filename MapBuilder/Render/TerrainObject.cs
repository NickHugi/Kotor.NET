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
            List<Vector3> vertices = new();
            for (var x = 0; x < _terrain.Width; x++)
                for (var y = 0; y < _terrain.Length; y++)
                    vertices.Add(new(x, y, 0)); // _terrain.Height[x][y]

            List<short> elements =  new();
            for (var x = 0; x < _terrain.Width-1; x++)
                for (var y = 0; y < _terrain.Length-1; y++)
                {
                    var i0 = (short)(y * _terrain.Length + x);
                    var i1 = (short) (y * _terrain.Length + (x + 1));
                    var i2 = (short)((y + 1) * _terrain.Length + x);
                    var i3 = (short)((y + 1) * _terrain.Length + (x+1));
                    elements.AddRange(new short[] { i0, i1, i2 });
                    elements.AddRange(new short[] { i1, i3, i2 });
                }

            _graphics.SetVAO("terrain", new Kotor.NET.Graphics.VertexArray(_graphics.GL, vertices, elements));
        }

        public override List<IRenderable> GetRenderables(Graphics graphics)
        {
            return new() { new Renderable(graphics.GetVAO("terrain"), graphics.GetShader("kotor"), null, null) };
        }
    }
}
