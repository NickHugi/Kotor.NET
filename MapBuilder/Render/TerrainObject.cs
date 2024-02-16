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
using VertexArray = Kotor.NET.Graphics.VertexArray;

namespace MapBuilder.Render
{
    public class TerrainObject : SceneObject
    {
        private Graphics _graphics;
        private VertexArray _vao;

        private TerrainData _terrain;
        private int _terrainHash;


        public TerrainObject(Graphics graphics, TerrainData terrain)
        {
            _terrain = terrain;
            _graphics = graphics;

            _vao = new VertexArray(_graphics.GL);
            _graphics.SetVAO("terrain", _vao);
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

                    var heightMinXMinY = _terrain.Height[x + 0, y + 0];
                    var heightMaxXMinY = _terrain.Height[x + 1, y + 0];
                    var heightMinXMaxY = _terrain.Height[x + 0, y + 1];
                    var heightMaxXMaxY = _terrain.Height[x + 1, y + 1];

                    vertices.Add(new(x + 0, y + 0, heightMinXMinY));
                    vertices.Add(new(x + 1, y + 0, heightMaxXMinY));
                    vertices.Add(new(x + 1, y + 1, heightMaxXMaxY));
                    uvs.Add(new(0, 0));
                    uvs.Add(new(1, 0));
                    uvs.Add(new(1, 1));
                    elements.AddRange(new short[] { (short)(e + 0), (short)(e + 1), (short)(e + 2) });

                    vertices.Add(new(x + 0, y + 0, heightMinXMinY));
                    vertices.Add(new(x + 1, y + 1, heightMaxXMaxY));
                    vertices.Add(new(x + 0, y + 1, heightMinXMaxY));
                    uvs.Add(new(0, 0));
                    uvs.Add(new(1, 1));
                    uvs.Add(new(1, 0));
                    elements.AddRange(new short[] { (short)(e + 3), (short)(e + 4), (short)(e + 5) });
                }

            _vao.Assign(vertices, elements, uv1s: uvs);
        }

        public override List<IRenderable> GetRenderables(Graphics graphics)
        {
            // Does the terrain VAO require updating? 
            if (_terrain.GetHashCode() != _terrainHash)
            {
                UpdateRenderData();
            }

            var vao = graphics.GetVAO("terrain");
            var shader = graphics.GetShader("kotor");
            var texture1 = graphics.GetTextures("lda_grass01");

            return new() { new Renderable(vao, shader, texture1, null) };
        }

        public void UpdateRenderData()
        {
            Gen();
            _terrainHash = _terrain.GetHashCode();
        }
    }
}
