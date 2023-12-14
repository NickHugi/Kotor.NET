using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL
{
    public class Graphics
    {
        public Dictionary<string, Shader> Shaders { get; } = new();
        public Dictionary<string, Texture> Textures { get; } = new();
        public Dictionary<string, VertexArray> VAOs { get; } = new();

        public VertexArray  GetVertexArray(string name)
        {
            return VAOs[name];
        }

        public Texture GetTextures(string name)
        {
            return Textures[name];
        }

        public VertexArray GetVAO(string name)
        {
            return VAOs[name];
        }
    }
}
